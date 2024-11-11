using BankingApp.DAL.Models;

namespace BankingApp.DAL.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByNameAsync(string name);
        Task AddUserAsync(string userName);
    }
}
