using Microsoft.AspNetCore.Mvc;

namespace observatorio.saude.Application.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
}