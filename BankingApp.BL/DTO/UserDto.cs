using BankingApp.DAL.Models;

namespace BankingApp.BLL.DTO
{
    public class UserDto : BaseDto
    {
        public User? User { get; set; }

        public UserDto()
        {
            User = null;
        }
        public UserDto(User user)
        {
            this.User = user;
            this.HasError = false;
            this.ErrorMessage = string.Empty;
        }
    }
}
