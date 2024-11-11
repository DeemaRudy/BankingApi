using BankingApp.BLL.DTO;

namespace BankingApp.BLL.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterUserAsync(string userName);
        Task<UserDto> GetUserInfoAsync(string userName);
    }
}
