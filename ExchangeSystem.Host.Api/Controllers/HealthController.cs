using Microsoft.AspNetCore.Mvc;

namespace ExchangeSystem.Host.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok();
    }
}