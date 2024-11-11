using BankingApp.DAL.EF;
using BankingApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DAL.Repositories.Implementation
{
    internal class UserRepository : IUserRepository
    {
        private readonly BankingAppContext _bankingContext;

        public UserRepository(BankingAppContext bankingContext)
        {
            _bankingContext = bankingContext;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _bankingContext.Users.FindAsync(userId);
        }
        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _bankingContext.Users.FirstOrDefaultAsync(user => user.Name == name);
        }
        public async Task AddUserAsync(string userName)
        {
            var user = new User { Name = userName };
            await _bankingContext.Users.AddAsync(user);
        }

    }
}
