namespace SoccerManager.Application.Models;

public class UserRegisterResult
{
    public bool Succeeded { get; }
    
    public IEnumerable<string> Errors { get; }
    
    public string UserId { get; }
    
    public UserRegisterResult(IEnumerable<string> errors)
    {
        Succeeded = false;
        Errors = errors;
    }

    public UserRegisterResult(string userId)
    {
        Succeeded = true;
        UserId = userId;
    }
}