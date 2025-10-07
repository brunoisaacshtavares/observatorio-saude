using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using observatorio.saude.Application.Queries.GetIndicadoresLeitos;

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
    public async Task<IActionResult> GetIndicadoresPorEstado([FromQuery] int? ano, [FromQuery] List<string> ufs)
    {
        var query = new GetIndicadoresLeitosPorEstadoQuery { Ano = ano, Ufs = ufs };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}