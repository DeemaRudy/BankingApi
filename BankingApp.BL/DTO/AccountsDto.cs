using BankingApp.DAL.Models;

namespace BankingApp.BLL.DTO
{
    public class AccountsDto : BaseDto
    {
        public IEnumerable<Account>? Accounts { get; set; }
        public AccountsDto() 
        {
            Accounts = null;
        }
        public AccountsDto(IEnumerable<Account> accounts)
        {
            this.Accounts = accounts;
            this.HasError = false;
            this.ErrorMessage = string.Empty;
        }
    }
}
