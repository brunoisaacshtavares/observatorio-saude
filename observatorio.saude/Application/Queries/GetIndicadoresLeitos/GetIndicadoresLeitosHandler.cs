using MediatR;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetIndicadoresLeitos;

public class GetIndicadoresLeitosHandler : IRequestHandler<GetIndicadoresLeitosQuery, IndicadoresLeitosDto>
{
    private readonly ILeitosRepository _leitoRepository;

    public GetIndicadoresLeitosHandler(ILeitosRepository leitoRepository)
    {
        _leitoRepository = leitoRepository;
    }

    public async Task<IndicadoresLeitosDto> Handle(GetIndicadoresLeitosQuery request,
        CancellationToken cancellationToken)
    {
        var dadosAgregados = await _leitoRepository.GetLeitosAgregadosAsync(request.Ano);

        if (dadosAgregados == null)
            return new IndicadoresLeitosDto();

        var ocupacaoMedia = dadosAgregados.TotalLeitos > 0
            ? (double)(dadosAgregados.TotalLeitos - dadosAgregados.TotalLeitosSus) / dadosAgregados.TotalLeitos * 100
            : 0;

        return new IndicadoresLeitosDto
        {
            TotalLeitos = dadosAgregados.TotalLeitos,
            LeitosDisponiveis = dadosAgregados.TotalLeitosSus,
            OcupacaoMedia = Math.Round(ocupacaoMedia, 2),
            Criticos = dadosAgregados.TotalUti
        };
    }
}