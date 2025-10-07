// observatorio.saude.Application/Queries/GetIndicadoresLeitos/GetIndicadoresLeitosHandler.cs

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
        // 1. Chama o repositório para buscar os dados brutos.
        var dadosAgregados = await _leitoRepository.GetLeitosAgregadosAsync(request.Ano);

        if (dadosAgregados == null)
            // Se não houver dados, retorna um DTO vazio.
            return new IndicadoresLeitosDto();

        // 2. Executa a lógica de negócio (cálculos) aqui no handler.
        var ocupacaoMedia = dadosAgregados.TotalLeitos > 0
            ? (double)(dadosAgregados.TotalLeitos - dadosAgregados.TotalLeitosSus) / dadosAgregados.TotalLeitos * 100
            : 0;

        // 3. Mapeia os dados brutos e calculados para o DTO final da API.
        return new IndicadoresLeitosDto
        {
            TotalLeitos = dadosAgregados.TotalLeitos,
            LeitosDisponiveis = dadosAgregados.TotalLeitosSus,
            OcupacaoMedia = Math.Round(ocupacaoMedia, 2),
            Criticos = dadosAgregados.TotalUti
        };
    }
}