using BankingApp.DAL.Models;

namespace BankingApp.DAL.Repositories
{
    public interface IAccountRepository
    {
        Task<Account?> GetByAccountIdAsync(int accountId);
        Task AddAccountAsync(int userId, decimal amount);
        Task<bool> AddAmountIntoAccount(int accountId, decimal amount);
        Task<IEnumerable<Account>?> GetAllUserAccountsAsync(int userId);
    }
}
