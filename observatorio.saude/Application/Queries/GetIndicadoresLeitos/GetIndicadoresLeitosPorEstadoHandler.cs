using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosPorEstadoHandler : IRequestHandler<GetIndicadoresLeitosPorEstadoQuery,
    IEnumerable<IndicadoresLeitosEstadoDto>>
{
    private readonly IIbgeApiClient _ibgeApiClient;
    private readonly ILeitosRepository _leitoRepository;

    public GetIndicadoresLeitosPorEstadoHandler(ILeitosRepository leitoRepository, IIbgeApiClient ibgeApiClient)
    {
        _leitoRepository = leitoRepository;
        _ibgeApiClient = ibgeApiClient;
    }

    public async Task<IEnumerable<IndicadoresLeitosEstadoDto>> Handle(GetIndicadoresLeitosPorEstadoQuery request,
        CancellationToken cancellationToken)
    {
        var anoParaBuscar = request.Ano ?? DateTime.Now.Year;

        List<long>? codUfs = null;
        if (request.Ufs != null && request.Ufs.Any())
        {
            var ufsFromIbge = await _ibgeApiClient.FindUfsAsync();
            var requestUfsUpper = request.Ufs.Select(u => u.ToUpperInvariant()).ToList();

            codUfs = ufsFromIbge
                .Where(uf => requestUfsUpper.Contains(uf.Sigla.ToUpperInvariant()))
                .Select(uf => uf.Id)
                .ToList();
        }

        var indicadoresPorEstado = await _leitoRepository.GetIndicadoresPorEstadoAsync(anoParaBuscar, codUfs, request.Tipo);

        var populacaoTask = _ibgeApiClient.FindPopulacaoUfAsync(anoParaBuscar);
        var ufsTask = _ibgeApiClient.FindUfsAsync();
        await Task.WhenAll(populacaoTask, ufsTask);

        var dadosIbgeUf = await populacaoTask;
        var dadosUfs = await ufsTask;

        var mapaPopulacao = dadosIbgeUf
            .SelectMany(r => r.Resultados)
            .SelectMany(res => res.Series)
            .ToDictionary(
                serie => long.Parse(serie.Localidade.Id),
                serie => long.Parse(serie.SerieData.GetValueOrDefault(anoParaBuscar.ToString(), "0"))
            );

        var mapaUfData = dadosUfs.ToDictionary(
            uf => uf.Id,
            uf => (uf.Nome, uf.Sigla, Regiao: uf.Regiao.Nome)
        );

        foreach (var item in indicadoresPorEstado)
        {
            if (mapaPopulacao.TryGetValue(item.CodUf, out var populacao))
                item.Populacao = populacao;

            if (mapaUfData.TryGetValue(item.CodUf, out var ufData))
            {
                item.NomeUf = ufData.Nome;
                item.SiglaUf = ufData.Sigla;
                item.Regiao = ufData.Regiao;
            }

            item.OcupacaoMedia = item.TotalLeitos > 0
                ? Math.Round((double)(item.TotalLeitos - item.LeitosDisponiveis) / item.TotalLeitos * 100, 2)
                : 0;

            item.CoberturaLeitosPor1kHab = item.Populacao > 0
                ? Math.Round((double)item.TotalLeitos / item.Populacao * 1000, 2)
                : 0;
        }

        return indicadoresPorEstado.OrderByDescending(x => x.TotalLeitos);
    }
}