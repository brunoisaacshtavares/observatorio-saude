namespace observatorio.saude.Application.Services.Clients;

public interface IIbgeApiClient
{
    Task<List<IbgeUfResponse>> FindPopulacaoUfAsync(int? ano = null);

    Task<IEnumerable<UfDataResponse>> FindUfsAsync();
}