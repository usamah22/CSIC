using Domain;

namespace Application.Common.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}