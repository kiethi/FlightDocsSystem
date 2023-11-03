using FlightDocsSystem.Models.Model;

namespace FlightDocsSystem.Services
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExistAsync(string email);
        Task CreateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<List<User>> GetAllUsersAsync(); 
        ValueTask<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task<User?> LoginAsync(LoginModel model);

        Task ChangeAvatarAsync(int id, byte[]? avatar);
    }
}
