namespace SoccerManager.Application.Interfaces;

public interface INameGenerator
{
    string GenerateFirstName();
    string GenerateLastName();
    string GenerateCountryName();
}