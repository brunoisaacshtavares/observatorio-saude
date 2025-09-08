using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using observatorio.saude.Application.Queries.GetEstabelecimentosPaginados;
using observatorio.saude.Domain.Entities;
using observatorio.saude.Domain.Utils;

namespace observatorio.saude.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class EstabelecimentoController(IMediator mediator) : BaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<Estabelecimento>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEstabelecimentos([FromQuery] GetEstabelecimentosPaginadosQuery paginadosQuery)
    {
        var result = await mediator.Send(paginadosQuery);
        return Ok(result);
    }
}