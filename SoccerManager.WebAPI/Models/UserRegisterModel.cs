using System.ComponentModel.DataAnnotations;

namespace SoccerManager.WebAPI.Models;

public class UserRegisterModel
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}