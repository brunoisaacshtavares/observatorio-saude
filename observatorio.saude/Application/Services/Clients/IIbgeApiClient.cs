namespace observatorio.saude.Application.Services.Clients;

public interface IIbgeApiClient
{
    Task<PopulacaoUfResultado> FindPopulacaoUfAsync(int? ano = null);

    Task<IEnumerable<UfDataResponse>> FindUfsAsync();
}