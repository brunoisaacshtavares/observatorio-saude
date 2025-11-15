using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Queries.GetLeitosPaginados;

public class GetLeitosPaginadosHandler(ILeitosRepository leitosRepository, IIbgeApiClient ibgeApiClient)
    : IRequestHandler<GetLeitosPaginadosQuery, PaginatedResult<LeitosHospitalarDto>>
{
    private readonly IIbgeApiClient _ibgeApiClient = ibgeApiClient;
    private readonly ILeitosRepository _leitosRepository = leitosRepository;

    public async Task<PaginatedResult<LeitosHospitalarDto>> Handle(
        GetLeitosPaginadosQuery request,
        CancellationToken cancellationToken)
    {
        long? codUf = null;
        if (!string.IsNullOrWhiteSpace(request.Uf))
        {
            var ufs = await _ibgeApiClient.FindUfsAsync();
            var ufEncontrada = ufs.FirstOrDefault(uf =>
                uf.Sigla.Equals(request.Uf, StringComparison.OrdinalIgnoreCase));

            if (ufEncontrada != null) codUf = ufEncontrada.Id;
        }

        var pagedResult = await _leitosRepository.GetPagedLeitosAsync(
            request.PageNumber,
            request.PageSize,
            request.Nome,
            request.CodCnes,
            request.Ano,
            request.Anomes,
            request.Tipo,
            codUf,
            cancellationToken);

        var ufsData = await _ibgeApiClient.FindUfsAsync();
        var ufMap = ufsData.ToDictionary(uf => uf.Id.ToString(), uf => uf.Sigla);

        foreach (var item in pagedResult.Items)
            if (item.LocalizacaoUf != null && ufMap.TryGetValue(item.LocalizacaoUf, out var ufSigla))
                item.LocalizacaoUf = ufSigla;
            else
                item.LocalizacaoUf = "Não Informada";

        return pagedResult;
    }
}