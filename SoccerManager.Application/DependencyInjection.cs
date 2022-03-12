using Microsoft.Extensions.DependencyInjection;
using SoccerManager.Application.Interfaces;
using SoccerManager.Application.Services;
using SoccerManager.Application.Utils;

namespace SoccerManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ISoccerTeamService, SoccerTeamService>();
        services.AddTransient<ISoccerPlayerService, SoccerPlayerService>();
        services.AddTransient<INameGenerator, NameGenerator>();

        return services;
    }
}