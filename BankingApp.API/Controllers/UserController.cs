using BankingApp.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(string userName)
        {
            var result = await _userService.RegisterUserAsync(userName);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetUserInfo(string userName)
        {
            var result = await _userService.GetUserInfoAsync(userName);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.User);
        }
    }
}
