using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;

public class GetContagemEstabelecimentosPorEstadoQueryHandler : IRequestHandler<GetNumerostabelecimentosPorEstadoQuery,
    IEnumerable<NumeroEstabelecimentoEstadoDto>>
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;
    private readonly IIbgeApiClient _ibgeApiClient;

    public GetContagemEstabelecimentosPorEstadoQueryHandler(
        IEstabelecimentoRepository estabelecimentoRepository,
        IIbgeApiClient ibgeApiClient)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
        _ibgeApiClient = ibgeApiClient;
    }

    public async Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> Handle(
        GetNumerostabelecimentosPorEstadoQuery request, CancellationToken cancellationToken)
    {
        var contagemPorEstado = await _estabelecimentoRepository.GetContagemPorEstadoAsync();

        var populacaoTask = _ibgeApiClient.FindPopulacaoUfAsync();
        var ufsTask = _ibgeApiClient.FindUfsAsync();

        await Task.WhenAll(populacaoTask, ufsTask);

        var dadosIbgeUf = await populacaoTask;
        var dadosUfs = await ufsTask;

        var mapaPopulacao = dadosIbgeUf
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

            if (item.Populacao > 0)
                item.CoberturaEstabelecimentos = Math.Round(
                    (double)item.TotalEstabelecimentos / item.Populacao * 100000, 2);
            else
                item.CoberturaEstabelecimentos = 0;
        }

        return contagemPorEstado;
    }
}