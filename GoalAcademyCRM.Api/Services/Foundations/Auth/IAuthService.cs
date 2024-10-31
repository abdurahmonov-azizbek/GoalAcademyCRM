using GoalAcademyCRM.Api.Models.Auth;
using GoalAcademyCRM.Api.Models.Users;

namespace GoalAcademyCRM.Api.Services.Foundations.Auth
{
    public interface IAuthService
    {
        ValueTask<string> LoginAsync(LoginDetails loginDetails);
        ValueTask<User> GetCurrentUser(Guid userId);
    }
}
