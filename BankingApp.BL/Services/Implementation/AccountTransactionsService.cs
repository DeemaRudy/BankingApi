using BankingApp.BLL.DTO;
using BankingApp.DAL.UnitOfWork;

namespace BankingApp.BLL.Services.Implementation
{
    public class AccountTransactionsService : IAccountTransactionsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountTransactionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseDto> DepositFundsIntoAccount(int accountId, decimal depositFunds)
        {
            if (depositFunds < 0)
            {
                return new BaseDto { HasError = true, ErrorMessage = "Deposit funds cannot be less than zero" };
            }

            var account = await _unitOfWork.Accounts.GetByAccountIdAsync(accountId);
            if (account == null)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"Account with id={accountId} doesn't exists" };
            }

            var isCompleted = await _unitOfWork.Accounts.AddAmountIntoAccount(account.AccountId, depositFunds);
            if (isCompleted)
            {
                await _unitOfWork.CompleteAsync();
            }

            return new BaseDto { HasError = false, ErrorMessage = String.Empty };
        }

        public async Task<BaseDto> WithdrawFundsFromAccount(int accountId, decimal withdrawFunds)
        {
            if (withdrawFunds < 0)
            {
                return new BaseDto { HasError = true, ErrorMessage = "Withdraw funds cannot be negative" };
            }

            var account = await _unitOfWork.Accounts.GetByAccountIdAsync(accountId);
            if (account == null)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"Account with id={accountId} doesn't exists" };
            }

            if (account.Balance < withdrawFunds)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"There are not enough funds in the account with id={accountId} to complete the withdrawal" };
            }
            var isCompleted = await _unitOfWork.Accounts.AddAmountIntoAccount(account.AccountId, -withdrawFunds);
            if (isCompleted)
            {
                await _unitOfWork.CompleteAsync();
            }

            return new BaseDto { HasError = false, ErrorMessage = String.Empty };
        }

        public async Task<BaseDto> TransferFunds(int fromAccountId, int toAccountId, decimal transferAmount)
        {
            if (transferAmount <= 0)
            {
                return new BaseDto { HasError = true, ErrorMessage = "Transfer amount cannot be negative or zero" };
            }
            if (fromAccountId == toAccountId)
            {
                return new BaseDto { HasError = true, ErrorMessage = "There is no reason to transfer the amount between the same account" };
            }

            var fromAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(fromAccountId);
            if (fromAccount == null)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"Account with id={fromAccountId} from which funds are to be transferred doesn't exist" };
            }

            if (fromAccount.Balance < transferAmount)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"There are not enough funds in the account with id={fromAccountId} to complete the transfer" };
            }

            var toAccount = await _unitOfWork.Accounts.GetByAccountIdAsync(toAccountId);
            if (toAccount == null)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"Account with id={toAccountId} doesn't exists" };
            }

            await _unitOfWork.BeginTransactionAsync();

            var isWithdrawFromAccountSuccessful = await _unitOfWork.Accounts.AddAmountIntoAccount(fromAccount.AccountId, -transferAmount);
            var isTransferToAccountSuccessful = await _unitOfWork.Accounts.AddAmountIntoAccount(toAccount.AccountId, transferAmount);
            if (isWithdrawFromAccountSuccessful && isTransferToAccountSuccessful)
            {
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitAsync();
                return new BaseDto { HasError = false, ErrorMessage = string.Empty };
            }

            await _unitOfWork.RollbackAsync();
            return new BaseDto { HasError = true, ErrorMessage = "Transactions failed" };
        }

    }
}
