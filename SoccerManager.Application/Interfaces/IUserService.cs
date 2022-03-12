using SoccerManager.Application.Models;

namespace SoccerManager.Application.Interfaces;

public interface IUserService
{
    Task<UserRegisterResult> Register(string email, string password);
    Task<UserLoginResult> Login(string email, string password);
}