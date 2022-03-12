using System.ComponentModel.DataAnnotations;

namespace SoccerManager.WebAPI.Models;

public class ChangeSoccerPlayerRequest
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    public string Country { get; set; }
}