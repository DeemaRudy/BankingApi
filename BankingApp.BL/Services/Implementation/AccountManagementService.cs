using BankingApp.BLL.DTO;
using BankingApp.DAL.UnitOfWork;

namespace BankingApp.BLL.Services.Implementation
{
    public class AccountManagementService : IAccountManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseDto> CreateNewAccountWithInitialBalanceAsync(int userId, decimal initialBalance)
        {
            if (initialBalance < 0)
            {
                return new BaseDto { HasError = true, ErrorMessage = "Balance cannot be less than zero" };
            }

            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new BaseDto { HasError = true, ErrorMessage = $"The user with id={userId} doesn't exists" };
            }

            await _unitOfWork.Accounts.AddAccountAsync(user.UserId, initialBalance);
            await _unitOfWork.CompleteAsync();

            return new BaseDto { HasError = false, ErrorMessage = String.Empty };
        }

        public async Task<AccountDto> GetAccountDetailsAsync(int accountId)
        {
            var account = await _unitOfWork.Accounts.GetByAccountIdAsync(accountId);
            if (account == null)
            {
                return new AccountDto { HasError = true, ErrorMessage = $"Account with id={accountId} doesn't exists" };
            }

            return new AccountDto(account);
        }

        public async Task<AccountsDto> GetAllUserAccountsAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetUserByIdAsync(userId);
            if (user == null)
            {
                return new AccountsDto { HasError = true, ErrorMessage = $"User with id={userId} doesn't exists" };
            }
            var accounts = await _unitOfWork.Accounts.GetAllUserAccountsAsync(userId);
            if (accounts == null || !accounts.Any())
            {
                return new AccountsDto { HasError = true, ErrorMessage = $"User with id={userId} doesn't have any accounts yet" };
            }

            return new AccountsDto(accounts);
        }
    }
}
