using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Domain.Interface;

public interface IEstabelecimentoRepository
{
    Task<IEnumerable<NumeroEstabelecimentoEstadoDto>> GetContagemPorEstadoAsync();
    Task<NumeroEstabelecimentosDto> GetContagemTotalAsync();
}