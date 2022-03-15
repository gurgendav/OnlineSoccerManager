using SoccerManager.Application.Interfaces;

namespace SoccerManager.Infrastructure.Generators;

public class NumberGenerator : INumberGenerator
{
    public int GenerateInt(int min, int max)
    {
        return Random.Shared.Next(min, max + 1);
    }
}