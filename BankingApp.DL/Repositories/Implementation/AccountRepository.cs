using BankingApp.DAL.EF;
using BankingApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingApp.DAL.Repositories.Implementation
{
    internal class AccountRepository : IAccountRepository
    {
        private readonly BankingAppContext _bankingContext;

        public AccountRepository(BankingAppContext bankingContext)
        {
            _bankingContext = bankingContext;
        }

        public async Task<Account?> GetByAccountIdAsync(int accountId)
        {
            return await _bankingContext.Accounts.FindAsync(accountId);
        }

        public async Task AddAccountAsync(int userId, decimal amount)
        {
            var account = new Account { Balance = amount, UserId = userId };
            await _bankingContext.Accounts.AddAsync(account);
        }

        public async Task<bool> AddAmountIntoAccount(int accountId, decimal amount)
        {
            var account = await _bankingContext.Accounts.FindAsync(accountId);
            if (account != null)
            {
                account.Balance += amount;
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Account>?> GetAllUserAccountsAsync(int userId)
        {
            return await _bankingContext.Accounts
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }
    }
}
