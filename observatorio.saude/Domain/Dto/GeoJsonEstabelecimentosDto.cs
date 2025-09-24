namespace observatorio.saude.Domain.Dto;

public record GeoJsonPoint(string Type, double[] Coordinates)
{
    public GeoJsonPoint(double longitude, double latitude) : this("Point", new[] { longitude, latitude })
    {
    }
}

public record GeoJsonFeature(string Type, GeoJsonPoint Geometry, Dictionary<string, object> Properties)
{
    public GeoJsonFeature(GeoJsonPoint geometry, Dictionary<string, object> properties) : this("Feature", geometry,
        properties)
    {
    }
}

public record GeoJsonFeatureCollection(string Type, List<GeoJsonFeature> Features)
{
    public GeoJsonFeatureCollection(List<GeoJsonFeature> features) : this("FeatureCollection", features)
    {
    }
}

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