using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosGeoJson;

public class GetEstabelecimentosGeoJsonQuery : IRequest<GeoJsonFeatureCollection>
{
    public string? Uf { get; set; }

    public double? MinLatitude { get; set; }
    public double? MaxLatitude { get; set; }
    public double? MinLongitude { get; set; }
    public double? MaxLongitude { get; set; }
}