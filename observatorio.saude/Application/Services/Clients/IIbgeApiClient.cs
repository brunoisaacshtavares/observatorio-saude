namespace observatorio.saude.Application.Services.Clients;

public interface IIbgeApiClient
{
    Task<List<IbgeUfResponse>> FindPopulacaoUfAsync();

    Task<IEnumerable<UfDataResponse>> FindUfsAsync();
}