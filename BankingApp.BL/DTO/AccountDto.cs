using BankingApp.DAL.Models;

namespace BankingApp.BLL.DTO
{
    public class AccountDto : BaseDto
    {
        public Account? Account { get; set; }

        public AccountDto()
        {
            
        }
        public AccountDto(Account account)
        {
            this.Account = account;
            this.HasError = false;
            this.ErrorMessage = string.Empty;
        }
    }
}
