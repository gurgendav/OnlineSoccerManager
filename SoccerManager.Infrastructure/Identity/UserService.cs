using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Interfaces;
using SoccerManager.Application.Models;
using SoccerManager.Infrastructure.Identity.Options;

namespace SoccerManager.Infrastructure.Identity;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ISoccerTeamService _soccerTeamService;
    private readonly JwtSettings _jwtSettings;

    public UserService(UserManager<ApplicationUser> userManager,
        ISoccerTeamService soccerTeamService,
        IOptions<JwtSettings> jwtSettings)
    {
        _userManager = userManager;
        _soccerTeamService = soccerTeamService;
        _jwtSettings = jwtSettings.Value;
    }

    public async Task<UserRegisterResult> Register(string email, string password)
    {
        var userExists = await _userManager.FindByEmailAsync(email);
        if (userExists != null)
        {
            return new UserRegisterResult(new[] { "User with this email already exists" });
        }

        var user = new ApplicationUser
        {
            Email = email,
            UserName = email
        };
        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return new UserRegisterResult(result.Errors.Select(e => e.Description));
        }
        
        await _soccerTeamService.Create(user.Id);

        return new UserRegisterResult(user.Id);
    }

    public async Task<UserLoginResult> Login(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return new UserLoginResult("User not found");
        }

        if (!await _userManager.CheckPasswordAsync(user, password))
        {
            return new UserLoginResult("Incorrect password");
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Id)
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var token = new JwtSecurityToken(
            _jwtSettings.ValidIssuer,
            _jwtSettings.ValidAudience,
            expires: DateTime.Now.AddHours(_jwtSettings.TokenValidityInHours),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new UserLoginResult(user.Id, tokenString, token.ValidTo);
    }
}