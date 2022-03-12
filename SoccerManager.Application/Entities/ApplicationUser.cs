using Microsoft.AspNetCore.Identity;

namespace SoccerManager.Application.Entities;

public class ApplicationUser : IdentityUser
{
    public int SoccerTeamId { get; set; }
    public SoccerTeam SoccerTeam { get; set; }
}