using BankingApp.BLL.DTO;

namespace BankingApp.BLL.Services
{
    public interface IAccountManagementService
    {
        public Task<BaseDto> CreateNewAccountWithInitialBalanceAsync(int userId, decimal initialBalance);
        public Task<AccountDto> GetAccountDetailsAsync(int accountId);
        public Task<AccountsDto> GetAllUserAccountsAsync(int userId);
    }
}
