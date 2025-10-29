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
        var dadosAgregados = await _leitoRepository.GetLeitosAgregadosAsync(request.Ano, request.Anomes, request.Tipo);

        if (dadosAgregados == null)
            return new IndicadoresLeitosDto();

        return new IndicadoresLeitosDto
        {
            TotalLeitos = dadosAgregados.TotalLeitos,
            TotalLeitosSus = dadosAgregados.TotalLeitosSus,
            Criticos = dadosAgregados.TotalUti
        };
    }
}