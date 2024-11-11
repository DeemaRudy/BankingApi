using BankingApp.BLL.DTO;
using BankingApp.DAL.UnitOfWork;

namespace BankingApp.BLL.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> RegisterUserAsync(string userName)
        {
            var user = await _unitOfWork.Users.GetUserByNameAsync(userName);
            if (user == null)
            {
                await _unitOfWork.Users.AddUserAsync(userName);
                await _unitOfWork.CompleteAsync();
                user = await _unitOfWork.Users.GetUserByNameAsync(userName);
                return new UserDto(user);
            }

            return new UserDto { HasError = true, ErrorMessage = "The user already exists" };
        }
        public async Task<UserDto> GetUserInfoAsync(string userName)
        {
            var user = await _unitOfWork.Users.GetUserByNameAsync(userName);
            if (user == null)
            {
                return new UserDto { HasError = true, ErrorMessage = "User doesn't exists in the system" };
            }

            return new UserDto(user);
        }
    }
}
