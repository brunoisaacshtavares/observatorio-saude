using System.Reflection;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;
using observatorio.saude.Domain.Interface;
using observatorio.saude.Domain.Job;
using observatorio.saude.Infra.Data;
using observatorio.saude.Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");

if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddHostedService<EtlScheduledJob>();
builder.Services.AddControllers();
builder.Services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(Program).Assembly); });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<GetEstabelecimentosPaginadosQuery>();
builder.Services.AddScoped<IEstabelecimentoRepository, EstabelecimentoRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()   // Permite qualquer origem
              .AllowAnyMethod()   // Permite qualquer método (GET, POST, etc.)
              .AllowAnyHeader();  // Permite qualquer cabeçalho
    });
});

builder.Services.AddControllers();

builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath, true);
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");  // Use a política liberada

if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Contains("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (var description in provider.ApiVersionDescriptions)
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                $"API {description.GroupName.ToUpperInvariant()}");
    });
}

app.UseAuthorization();
app.MapControllers();
app.Run();
