using Microsoft.AspNetCore.Identity;

namespace SoccerManager.Application.Entities;

public class ApplicationUser : IdentityUser
{
    public SoccerTeam SoccerTeam { get; set; }
}