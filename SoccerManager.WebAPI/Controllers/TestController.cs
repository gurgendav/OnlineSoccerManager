using Microsoft.AspNetCore.Mvc;

namespace SoccerManager.WebAPI.Controllers;

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