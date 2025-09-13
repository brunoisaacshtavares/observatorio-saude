using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;

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

        var dadosIbgeUf = await _ibgeApiClient.FindPopulacaoUfAsync();

        var mapaDadosUf = dadosIbgeUf
            .SelectMany(r => r.Resultados)
            .SelectMany(res => res.Series)
            .ToDictionary(
                serie => long.Parse(serie.Localidade.Id),
                serie => (
                    serie.Localidade.Nome,
                    Serie: long.Parse(serie.SerieData["2025"])
                )
            );

        foreach (var item in contagemPorEstado)
        {
            if (mapaDadosUf.TryGetValue(item.CodUf, out var dadosUf))
            {
                item.NomeUf = dadosUf.Nome;
                item.Populacao = dadosUf.Serie;
            }

            if (item.Populacao <= 0)
            {
                item.CoberturaEstabelecimentos = 0;
                continue;
            }

            item.CoberturaEstabelecimentos =
                (double)item.TotalEstabelecimentos / item.Populacao * 100000;

            item.CoberturaEstabelecimentos = Math.Round(item.CoberturaEstabelecimentos, 2);

            item.SiglaUf = IbgeUfMap.GetSigla(item.CodUf);
        }

        return contagemPorEstado;
    }
}