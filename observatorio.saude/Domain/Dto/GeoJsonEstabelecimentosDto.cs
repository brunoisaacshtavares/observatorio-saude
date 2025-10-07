namespace observatorio.saude.Domain.Dto;

/// <summary>
/// Represents a GeoJSON Point geometry.
/// </summary>
/// <param name="Type">The geometry type, which is always "Point".</param>
/// <param name="Coordinates">The coordinates as an array [longitude, latitude].</param>
public record GeoJsonPoint(string Type, double[] Coordinates)
{
    public GeoJsonPoint(double longitude, double latitude) : this("Point", new[] { longitude, latitude })
    {
    }
}

/// <summary>
/// Represents a GeoJSON Feature, containing a geometry and properties.
/// </summary>
/// <param name="Type">The feature type, which is always "Feature".</param>
/// <param name="Geometry">The geometric shape of the feature.</param>
/// <param name="Properties">A dictionary of key-value pairs with additional information about the feature.</param>
public record GeoJsonFeature(string Type, GeoJsonPoint Geometry, Dictionary<string, object> Properties)
{
    public GeoJsonFeature(GeoJsonPoint geometry, Dictionary<string, object> properties) : this("Feature", geometry,
        properties)
    {
    }
}

/// <summary>
/// Represents a GeoJSON FeatureCollection, a list of GeoJSON features.
/// </summary>
/// <param name="Type">The collection type, which is always "FeatureCollection".</param>
/// <param name="Features">A list of GeoJSON features.</param>
public record GeoJsonFeatureCollection(string Type, List<GeoJsonFeature> Features)
{
    public GeoJsonFeatureCollection(List<GeoJsonFeature> features) : this("FeatureCollection", features)
    {
    }
}

/// <summary>
/// Represents raw geographic data for a feature.
/// </summary>
public class GeoFeatureData
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? NomeFantasia { get; set; }
    public string? Endereco { get; set; }
    public long? Numero { get; set; }
    public string? Bairro { get; set; }
    public long? Cep { get; set; }
}