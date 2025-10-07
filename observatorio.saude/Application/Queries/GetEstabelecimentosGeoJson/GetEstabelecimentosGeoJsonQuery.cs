using MediatR;
using observatorio.saude.Domain.Dto;

namespace observatorio.saude.Application.Queries.GetEstabelecimentosGeoJson;

/// <summary>
///     Represents a query to retrieve hospital data in GeoJSON format.
/// </summary>
public class GetEstabelecimentosGeoJsonQuery : IRequest<GeoJsonFeatureCollection>
{
    /// <summary>
    ///     Filters the query by the state abbreviation (e.g., "SP", "RJ", "MG").
    /// </summary>
    public string? Uf { get; set; }

    /// <summary>
    ///     The minimum latitude for the geographical bounding box.
    /// </summary>
    public double? MinLatitude { get; set; }

    /// <summary>
    ///     The maximum latitude for the geographical bounding box.
    /// </summary>
    public double? MaxLatitude { get; set; }

    /// <summary>
    ///     The minimum longitude for the geographical bounding box.
    /// </summary>
    public double? MinLongitude { get; set; }

    /// <summary>
    ///     The maximum longitude for the geographical bounding box.
    /// </summary>
    public double? MaxLongitude { get; set; }

    /// <summary>
    ///     The zoom level, which influences the number of features returned.
    /// </summary>
    public int Zoom { get; set; }
}