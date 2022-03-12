using System.ComponentModel.DataAnnotations;

namespace SoccerManager.WebAPI.Models;

public class ChangeSoccerTeamRequest
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Country { get; set; }
    
}