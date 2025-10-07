using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using observatorio.saude.Application.Queries.GetIndicadoresLeitos;
using observatorio.saude.Application.Queries.GetLeitosPaginados;
using observatorio.saude.Application.Queries.GetTopLeitos;
using observatorio.saude.Domain.Dto;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class LeitosController : BaseController
{
    private readonly IMediator _mediator;

    public LeitosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("indicadores")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIndicadores([FromQuery] int? ano)
    {
        var query = new GetIndicadoresLeitosQuery { Ano = ano };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("indicadores-por-estado")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIndicadoresPorEstado([FromQuery] int ano, [FromQuery] List<string> ufs)
    {
        var query = new GetIndicadoresLeitosPorEstadoQuery { Ano = ano, Ufs = ufs };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<LeitosHospitalarDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLeitos([FromQuery] GetLeitosPaginadosQuery paginadosQuery)
    {
        var result = await _mediator.Send(paginadosQuery);
        return Ok(result);
    }
    
    [HttpGet("top-leitos")]
    [ProducesResponseType(typeof(List<LeitosHospitalarDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopLeitos([FromQuery] GetTopLeitosQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}