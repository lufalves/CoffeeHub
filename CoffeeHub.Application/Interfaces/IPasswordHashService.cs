using CoffeeHub.Domain.User;

namespace CoffeeHub.Application.Interfaces;

public interface IPasswordHashService
{
    string HashPassword(User user, string password);
    bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}
