using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SoccerManager.Application.Interfaces;
using SoccerManager.WebAPI.Models;

namespace SoccerManager.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/team")]
public class SoccerTeamController : ControllerBase
{
    private readonly ISoccerTeamService _soccerTeamService;
    private readonly IMapper _mapper;

    public SoccerTeamController(ISoccerTeamService soccerTeamService, IMapper mapper)
    {
        _soccerTeamService = soccerTeamService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetTeam(int id)
    {
        var team = await _soccerTeamService.GetById(id);
        if (team == null)
        {
            return NotFound();
        }
        
        return Ok(_mapper.Map<SoccerTeamResponse>(team));
    }
    
    [HttpGet]
    [Route("my")]
    public async Task<SoccerTeamResponse> GetUserTeam()
    {
        var team = await _soccerTeamService.FindByUserId(User.Identity!.Name);
        return _mapper.Map<SoccerTeamResponse>(team);
    }
    
    [HttpPut]
    [Route("{teamId:int}")]
    public async Task<IActionResult> UpdateTeam(int teamId, [FromBody]ChangeSoccerTeamRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await _soccerTeamService.Update(teamId, request.Name, request.Country);
        return Ok(_mapper.Map<SoccerTeamResponse>(result));
    }
    
    
}