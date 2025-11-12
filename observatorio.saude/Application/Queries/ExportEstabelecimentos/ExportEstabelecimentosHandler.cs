using MediatR;
using observatorio.saude.Application.Services;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.ExportEstabelecimentos;

public class ExportEstabelecimentosHandler : IRequestHandler<ExportEstabelecimentosQuery, ExportFileResult>
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;
    private readonly IFileExportService _fileExportService;
    private readonly IIbgeApiClient _ibgeApiClient;

    public ExportEstabelecimentosHandler(
        IEstabelecimentoRepository estabelecimentoRepository,
        IIbgeApiClient ibgeApiClient,
        IFileExportService fileExportService)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
        _ibgeApiClient = ibgeApiClient;
        _fileExportService = fileExportService;
    }

    public async Task<ExportFileResult> Handle(ExportEstabelecimentosQuery request, CancellationToken cancellationToken)
    {
        var ufs = await _ibgeApiClient.FindUfsAsync();
        var mapaUfs = ufs.ToDictionary(u => u.Id, u => u.Sigla);
        long? codUf = null;

        if (!string.IsNullOrWhiteSpace(request.Uf))
        {
            var ufEncontrada = ufs.FirstOrDefault(u => u.Sigla.Equals(request.Uf, StringComparison.OrdinalIgnoreCase));
            if (ufEncontrada != null) codUf = ufEncontrada.Id;
        }

        var contagemPorEstado = await _estabelecimentoRepository.GetContagemPorEstadoAsync(codUf);
        var populacaoTask = _ibgeApiClient.FindPopulacaoUfAsync();
        var ufsTask = _ibgeApiClient.FindUfsAsync();
        await Task.WhenAll(populacaoTask, ufsTask);
        var dadosIbgeUf = await populacaoTask;
        var dadosUfs = await ufsTask;

        var mapaPopulacao = dadosIbgeUf.Dados
            .SelectMany(r => r.Resultados)
            .SelectMany(res => res.Series)
            .ToDictionary(
                serie => long.Parse(serie.Localidade.Id),
                serie => long.Parse(serie.SerieData["2025"])
            );

        var mapaUfData = dadosUfs.ToDictionary(
            uf => uf.Id,
            uf => (uf.Nome, uf.Sigla, Regiao: uf.Regiao.Nome)
        );

        foreach (var item in contagemPorEstado)
        {
            if (mapaPopulacao.TryGetValue(item.CodUf, out var populacao)) item.Populacao = populacao;
            if (mapaUfData.TryGetValue(item.CodUf, out var ufData))
            {
                item.NomeUf = ufData.Nome;
                item.SiglaUf = ufData.Sigla;
                item.Regiao = ufData.Regiao;
            }

            item.CoberturaEstabelecimentos = item.Populacao > 0
                ? Math.Round((double)item.TotalEstabelecimentos / item.Populacao * 100000, 2)
                : 0;
        }

        byte[] fileData;
        string contentType;
        string fileName;
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

        if (request.Formato.ToLower().Equals("csv"))
        {
            fileData = _fileExportService.GenerateCsv(contagemPorEstado);
            contentType = "text/csv";
            fileName = $"resumo_por_estado_{timestamp}.csv";
        }
        else
        {
            fileData = _fileExportService.GenerateExcel(contagemPorEstado);
            contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            fileName = $"resumo_por_estado_{timestamp}.xlsx";
        }

        return new ExportFileResult
        {
            FileData = fileData,
            ContentType = contentType,
            FileName = fileName
        };
    }
}