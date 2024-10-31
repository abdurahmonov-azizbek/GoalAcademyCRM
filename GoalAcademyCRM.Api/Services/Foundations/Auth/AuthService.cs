using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Models.Auth;
using GoalAcademyCRM.Api.Models.Users;
using GoalAcademyCRM.Api.Services.Helpers.Auth;

namespace GoalAcademyCRM.Api.Services.Foundations.Auth
{
    public class AuthService(
        ITokenGeneratorService tokenGeneratorService,
        IStorageBroker storageBroker) : IAuthService
    {
        public async ValueTask<User> GetCurrentUser(Guid userId)
        {
            var user = await storageBroker.SelectUserByIdAsync(userId)
                ?? throw new Exception("User not found!");

            return user;
        }

        public ValueTask<string> LoginAsync(LoginDetails loginDetails)
        {
            var user = storageBroker.SelectAllUsers().FirstOrDefault(c => c.UserName == loginDetails.UserName)
                ?? throw new Exception("User not found with this username!");

            if (user.Password != loginDetails.Password)
                throw new InvalidOperationException("Invalid username or password!");

            var token = tokenGeneratorService.GenerateToken(user);

            return new(token);
        }
    }
}
