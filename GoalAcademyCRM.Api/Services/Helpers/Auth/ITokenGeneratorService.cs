using GoalAcademyCRM.Api.Models.Users;

namespace GoalAcademyCRM.Api.Services.Helpers.Auth
{
    public interface ITokenGeneratorService
    {
        string GenerateToken(User user);
    }
}
