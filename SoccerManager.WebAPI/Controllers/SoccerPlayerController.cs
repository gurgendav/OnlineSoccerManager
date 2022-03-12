using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Interfaces;
using SoccerManager.WebAPI.Models;

namespace SoccerManager.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/player/{id}")]
public class SoccerPlayerController : ControllerBase
{
    private readonly ISoccerPlayerService _soccerPlayerService;
    private readonly IMapper _mapper;

    public SoccerPlayerController(ISoccerPlayerService soccerPlayerService, IMapper mapper)
    {
        _soccerPlayerService = soccerPlayerService;
        _mapper = mapper;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetPlayer(int id)
    {
        var soccerPlayer = await _soccerPlayerService.GetById(id);
        
        if (soccerPlayer == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<SoccerPlayerResponse>(soccerPlayer));
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePlayer(int id, [FromBody] ChangeSoccerPlayerRequest request)
    {
        var soccerPlayer = await _soccerPlayerService.Update(id, request.FirstName, request.LastName, request.Country);
        return Ok(_mapper.Map<SoccerPlayerResponse>(soccerPlayer));
    }
    
    
    
}