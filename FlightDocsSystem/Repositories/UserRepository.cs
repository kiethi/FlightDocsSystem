using FlightDocsSystem.Helpers;
using FlightDocsSystem.Models.Model;
using Microsoft.IdentityModel.Tokens;

namespace FlightDocsSystem.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> IsEmailExistAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async ValueTask<User?> GetUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task UpdateUserAsync(User user)
        {
            var dbUser = await GetUserByIdAsync(user.UserId);
            if (dbUser != null)
            {
                if (!string.IsNullOrEmpty(user.Name))
                    dbUser.Name = user.Name;
                if (!string.IsNullOrEmpty(user.Email))
                    dbUser.Email = user.Email;
                if (!string.IsNullOrEmpty(user.Phone))
                    dbUser.Phone = user.Phone;

                _context.Users.Update(dbUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User?> LoginAsync(LoginModel model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(p => p.Email == model.Email);
            if (user != null && Utils.CheckPassword(model.Password, user.Password))
            {
                return user;
            }
            return null;
        }

        public async Task ChangeAvatarAsync(int id, byte[]? avatar)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Avatar = avatar;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }
    }
}
