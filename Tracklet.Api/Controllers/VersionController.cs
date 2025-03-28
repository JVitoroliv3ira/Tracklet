using Microsoft.AspNetCore.Mvc;

namespace Tracklet.Api.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class VersionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            Version = "Em desenvolvimento"
        });
    }
}