using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace observatorio.saude.Application.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class HealthController : BaseController
{
    [HttpGet]
    public IActionResult GetHealth()
    {
        return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
    }
}