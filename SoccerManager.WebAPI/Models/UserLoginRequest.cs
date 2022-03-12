using System.ComponentModel.DataAnnotations;

namespace SoccerManager.WebAPI.Models;

public class UserLoginRequest
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
}