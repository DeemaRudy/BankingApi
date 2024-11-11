using BankingApp.DAL.Repositories;

namespace BankingApp.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        Task<int> CompleteAsync();
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
