using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SoccerManager.Application.Entities;
using SoccerManager.Application.Interfaces;
using SoccerManager.Infrastructure.Identity;
using SoccerManager.Infrastructure.Identity.Options;
using SoccerManager.Infrastructure.Persistence;

namespace SoccerManager.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        var jwtSettingsSection = configuration.GetSection(JwtSettings.Field);
        services.Configure<JwtSettings>(jwtSettingsSection);
        var jwtSettings = jwtSettingsSection.Get<JwtSettings>();
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ValidAudience = jwtSettings.ValidAudience,
                    ValidIssuer = jwtSettings.ValidIssuer
                };
            });

        services.AddScoped<IUserService, UserService>();
        services.AddHttpContextAccessor();

        return services;
    }
    
}