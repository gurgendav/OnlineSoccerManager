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
    public async Task<SoccerTeamResponse> GetTeam()
    {
        var team = await _soccerTeamService.FindByUserId(User.Identity!.Name);
        return _mapper.Map<SoccerTeamResponse>(team);
    }
    
    [HttpPut]
    [Route("{teamId:int}")]
    public async Task<SoccerTeamResponse> ChangeTeam(int teamId, [FromBody]ChangeSoccerTeamRequest request)
    {
        var result = await _soccerTeamService.Update(teamId, request.Name, request.Country);
        return _mapper.Map<SoccerTeamResponse>(result);
    }
    
    
}