using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Domain.Interface;

public interface ILeitosRepository
{
    Task<LeitosAgregadosDto?> GetLeitosAgregadosAsync(int? ano = null);

    Task<IEnumerable<IndicadoresLeitosEstadoDto>> GetIndicadoresPorEstadoAsync(int? ano = null,
        List<long>? codUfs = null);
}