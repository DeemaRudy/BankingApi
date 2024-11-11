using BankingApp.DAL.EF;
using BankingApp.DAL.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BankingApp.DAL.UnitOfWork.Implementation
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly BankingAppContext _bankingContext;
        public IUserRepository Users { get; }
        public IAccountRepository Accounts { get; }
        private IDbContextTransaction _transaction;

        public UnitOfWork(BankingAppContext bankingContext, IUserRepository userRepository, IAccountRepository accountRepository)
        {
            _bankingContext = bankingContext;
            Users = userRepository;
            Accounts = accountRepository;
        }

        public async Task<int> CompleteAsync() 
        { 
            return await _bankingContext.SaveChangesAsync(); 
        }
        public async Task BeginTransactionAsync()
        {
            _transaction = await _bankingContext.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _bankingContext.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public void Dispose()
        {
            _bankingContext.Dispose();
        }
    }
}
