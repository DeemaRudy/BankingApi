using BankingApp.BLL.Services;
using BankingApp.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountManagementController : Controller
    {
        private readonly IAccountManagementService _accountManagementService;

        public AccountManagementController(IAccountManagementService accountManagementService)
        {
            _accountManagementService = accountManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewBankingAccount(int userId, decimal initialBalance)
        {
            var result = await _accountManagementService.CreateNewAccountWithInitialBalanceAsync(userId, initialBalance);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAccountDetailsByAccountId(int accountId)
        {
            var result = await _accountManagementService.GetAccountDetailsAsync(accountId);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }
                
            return Ok(result.Account);
        }

        [HttpGet]
        public async Task<IActionResult> GetListOfAllUserAccounts(int userId)
        {
            var result = await _accountManagementService.GetAllUserAccountsAsync(userId);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Accounts);
        }
    }
}
