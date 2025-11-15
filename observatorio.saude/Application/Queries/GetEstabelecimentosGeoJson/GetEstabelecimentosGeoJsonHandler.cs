using MediatR;
using observatorio.saude.Application.Services.Clients;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Interface;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosGeoJson;

public class
    GetEstabelecimentosGeoJsonQueryHandler : IRequestHandler<GetEstabelecimentosGeoJsonQuery, GeoJsonFeatureCollection>
{
    private readonly IEstabelecimentoRepository _estabelecimentoRepository;
    private readonly IIbgeApiClient _ibgeApiClient;

    public GetEstabelecimentosGeoJsonQueryHandler(
        IEstabelecimentoRepository estabelecimentoRepository,
        IIbgeApiClient ibgeApiClient)
    {
        _estabelecimentoRepository = estabelecimentoRepository;
        _ibgeApiClient = ibgeApiClient;
    }

    public async Task<GeoJsonFeatureCollection> Handle(GetEstabelecimentosGeoJsonQuery request,
        CancellationToken cancellationToken)
    {
        long? codUf = null;

        if (!string.IsNullOrEmpty(request.Uf))
        {
            var ufs = await _ibgeApiClient.FindUfsAsync();
            var ufEncontrada = ufs.FirstOrDefault(u => u.Sigla.Equals(request.Uf, StringComparison.OrdinalIgnoreCase));
            if (ufEncontrada != null) codUf = ufEncontrada.Id;
        }

        var estabelecimentosData = await _estabelecimentoRepository.GetWithCoordinatesAsync(
            codUf,
            request.MinLatitude,
            request.MaxLatitude,
            request.MinLongitude,
            request.MaxLongitude,
            request.Zoom);

        var features = estabelecimentosData.Select(est =>
        {
            var point = new GeoJsonPoint((double)est.Longitude, (double)est.Latitude);
            var properties = new Dictionary<string, object>
            {
                { "nome", est.NomeFantasia ?? "Nome não informado" },
                { "endereco", $"{est.Endereco}, {est.Numero}" },
                { "bairro", est.Bairro ?? "Bairro não informado" },
                { "cep", est.Cep as object ?? "CEP não informado" }
            };
            return new GeoJsonFeature(point, properties);
        }).ToList();

        return new GeoJsonFeatureCollection(features);
    }
}