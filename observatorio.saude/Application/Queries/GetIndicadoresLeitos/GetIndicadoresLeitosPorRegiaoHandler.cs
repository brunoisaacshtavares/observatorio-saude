using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosPorRegiaoHandler : IRequestHandler<GetIndicadoresLeitosPorRegiaoQuery,
    IEnumerable<IndicadoresLeitosRegiaoDto>>
{
    private readonly IMediator _mediator;

    public GetIndicadoresLeitosPorRegiaoHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IEnumerable<IndicadoresLeitosRegiaoDto>> Handle(GetIndicadoresLeitosPorRegiaoQuery request,
        CancellationToken cancellationToken)
    {
        var indicadoresPorEstado = await _mediator.Send(new GetIndicadoresLeitosPorEstadoQuery
            {
                Ano = request.Ano,
                Anomes = request.Anomes,
                Tipo = request.Tipo
            },
            cancellationToken);

        var indicadoresPorRegiao = indicadoresPorEstado
            .GroupBy(estado => estado.Regiao)
            .Select(grupo =>
            {
                var totalLeitosAgregado = grupo.Sum(e => e.TotalLeitos);
                var populacaoAgregada = grupo.Sum(e => e.Populacao);
                var totalleitosSus = grupo.Sum(e => e.LeitosSus);
                return new IndicadoresLeitosRegiaoDto
                {
                    NomeRegiao = grupo.Key,
                    Populacao = populacaoAgregada,
                    TotalLeitos = totalLeitosAgregado,
                    Criticos = grupo.Sum(e => e.Criticos),
                    LeitosSus = totalleitosSus,
                    CoberturaLeitosPor1kHab = populacaoAgregada > 0
                        ? Math.Round((double)totalLeitosAgregado / populacaoAgregada * 1000, 2)
                        : 0
                };
            })
            .ToList();

        if (request.Regioes != null && request.Regioes.Any())
        {
            var requestRegioesUpper = request.Regioes.Select(r => r.ToUpperInvariant()).ToHashSet();
            indicadoresPorRegiao = indicadoresPorRegiao
                .Where(r => requestRegioesUpper.Contains(r.NomeRegiao.ToUpperInvariant()))
                .ToList();
        }

        return indicadoresPorRegiao.OrderByDescending(x => x.TotalLeitos);
    }
}