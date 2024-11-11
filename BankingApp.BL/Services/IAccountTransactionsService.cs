using BankingApp.BLL.DTO;

namespace BankingApp.BLL.Services
{
    public interface IAccountTransactionsService
    {
        public Task<BaseDto> DepositFundsIntoAccount(int accountId, decimal depositFunds);
        public Task<BaseDto> WithdrawFundsFromAccount(int accountId, decimal withdrawFunds);
        public Task<BaseDto> TransferFunds(int fromAccountId, int toAccountId, decimal transferAmount);
    }
}
