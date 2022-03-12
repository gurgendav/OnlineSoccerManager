using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SoccerManager.Application.Interfaces;

namespace SoccerManager.Infrastructure.Identity;

public class UserIdAccessor : IUserIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserIdAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetCurrentUserId() => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;
}