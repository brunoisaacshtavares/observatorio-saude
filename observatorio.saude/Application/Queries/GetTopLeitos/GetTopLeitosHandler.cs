using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Utils;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetTopLeitos;

public class GetTopLeitosHandler(ILeitosRepository leitosRepository, IIbgeApiClient ibgeApiClient)
    : IRequestHandler<GetTopLeitosQuery, List<LeitosHospitalarDto>>
{
    private readonly ILeitosRepository _leitosRepository = leitosRepository;
    private readonly IIbgeApiClient _ibgeApiClient = ibgeApiClient;

    public async Task<List<LeitosHospitalarDto>> Handle(
        GetTopLeitosQuery request,
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

        var items = await _leitosRepository.GetTopLeitosAsync(request.Ano, request.Count, codUf, cancellationToken);
        
        var ufsData = await _ibgeApiClient.FindUfsAsync();
        var ufMap = ufsData.ToDictionary(uf => uf.Id.ToString(), uf => uf.Sigla);

        foreach (var item in items)
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

        return items;
    }
}