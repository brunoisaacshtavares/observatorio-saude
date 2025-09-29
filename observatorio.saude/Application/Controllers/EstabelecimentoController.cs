using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using observatorio.saude.Application.Queries.ExportEstabelecimentos;
using observatorio.saude.Application.Queries.GetEstabelecimentosGeoJson;
using observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;
using observatorio.saude.Application.Queries.GetNumeroEstabelecimentos;
using observatorio.saude.Application.Services;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class EstabelecimentoController(IMediator mediator, IFileExportService fileExportService) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(NumeroEstabelecimentosDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumero()
    {
        var query = new GetNumeroEstabelecimentosQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("uf")]
    [ProducesResponseType(typeof(IEnumerable<NumeroEstabelecimentoEstadoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetNumeroPorEstado()
    {
        var query = new GetNumerostabelecimentosPorEstadoQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("info")]
    [ProducesResponseType(typeof(PaginatedResult<Estabelecimento>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstabelecimentos([FromQuery] GetEstabelecimentosPaginadosQuery paginadosQuery)
    {
        var result = await mediator.Send(paginadosQuery);
        return Ok(result);
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportResumoPorEstado([FromQuery] ExportEstabelecimentosQuery query)
    {
        var result = await mediator.Send(query);
        if (result?.FileData == null || result.FileData.Length == 0) return NoContent();
        return File(result.FileData, result.ContentType, result.FileName);
    }

    [HttpGet("export-details")]
    public async Task ExportStream([FromQuery] ExportEstabelecimentosDetalhadosQuery query,
        CancellationToken cancellationToken)
    {
        var dataStream = await mediator.Send(query, cancellationToken);
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        var format = query.Format?.ToLowerInvariant() ?? "csv";

        switch (format)
        {
            case "xlsx":
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Headers.Append("Content-Disposition",
                    $"attachment; filename=\"estabelecimentos_{timestamp}.xlsx\"");
                await fileExportService.GenerateXlsxStreamAsync(dataStream, Response.Body);
                break;

            case "csv":
            default:
                Response.ContentType = "text/csv";
                Response.Headers.Append("Content-Disposition",
                    $"attachment; filename=\"estabelecimentos_{timestamp}.csv\"");
                await fileExportService.GenerateCsvStreamAsync(dataStream, Response.Body);
                break;
        }
    }

    [HttpGet("geojson")]
    public async Task<ActionResult<GeoJsonFeatureCollection>> GetGeoJson(
        [FromQuery] GetEstabelecimentosGeoJsonQuery query)
    {
        var result = await mediator.Send(query);
        return Ok(result);
    }
}