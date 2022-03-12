namespace SoccerManager.Application.Models;

public class UserLoginResult
{
    public bool Succeeded { get; }
    
    public string Error { get; }
    
    public string UserId { get; }
    public string Token { get; }
    public DateTime? Expiration { get; }

    public UserLoginResult(string error)
    {
        Succeeded = false;
        Error = error;
    }

    public UserLoginResult(string userId, string token, DateTime expiration)
    {
        Succeeded = true;
        UserId = userId;
        Token = token;
        Expiration = expiration;
    }
}