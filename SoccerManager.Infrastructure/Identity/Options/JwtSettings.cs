namespace SoccerManager.Infrastructure.Identity.Options;

public class JwtSettings
{
    public const string Field = "JWT";
    
    public string ValidAudience { get; set; }
    public string ValidIssuer { get; set; }
    public string Secret { get; set; }
    public int TokenValidityInHours { get; set; }
}