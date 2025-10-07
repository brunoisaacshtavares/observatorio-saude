using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetLeitosPaginados;

public class GetLeitosPaginadosHandler(ILeitosRepository leitosRepository, IIbgeApiClient ibgeApiClient)
    : IRequestHandler<GetLeitosPaginadosQuery, PaginatedResult<LeitosHospitalarDto>>
{
    private readonly ILeitosRepository _leitosRepository = leitosRepository;
    private readonly IIbgeApiClient _ibgeApiClient = ibgeApiClient;

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

            if (ufEncontrada != null)
            {
                codUf = ufEncontrada.Id;
            }
        }
        
        var pagedResult = await _leitosRepository.GetPagedLeitosAsync(
            request.PageNumber,
            request.PageSize,
            request.Nome,
            request.CodCnes,
            request.Ano,
            codUf,
            cancellationToken);

        var ufsData = await _ibgeApiClient.FindUfsAsync();
        var ufMap = ufsData.ToDictionary(uf => uf.Id.ToString(), uf => uf.Sigla);

        foreach (var item in pagedResult.Items)
        {
            var totalLeitos = item.TotalLeitos;
            var leitosDisponiveis = item.LeitosDisponiveis;
            
            var leitosOcupados = totalLeitos - leitosDisponiveis;
            item.LeitosOcupados = leitosOcupados;
            
            item.PorcentagemOcupacao = totalLeitos > 0
                ? Math.Round((decimal)leitosOcupados / totalLeitos * 100)
                : 0;

            if (ufMap.TryGetValue(item.LocalizacaoUf, out var ufSigla))
            {
                item.LocalizacaoUf = ufSigla;
            }
            else
            {
                item.LocalizacaoUf = "Não Informada";
            }
        }

        return pagedResult;
    }
}