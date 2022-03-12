using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SoccerManager.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "OK";
    }
}