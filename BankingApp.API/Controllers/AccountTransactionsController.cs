using BankingApp.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountTransactionsController : Controller
    {
        private IAccountTransactionsService _accountTransactionsService;
        public AccountTransactionsController(IAccountTransactionsService accountTransactionsService)
        {
            _accountTransactionsService = accountTransactionsService;
        }

        [HttpPatch]
        public async Task<IActionResult> DepositFunds(int accountId, decimal depositFunds)
        {
            var result = await _accountTransactionsService.DepositFundsIntoAccount(accountId, depositFunds);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> WithdrawFunds(int accountId, decimal withdrawFunds)
        {
            var result = await _accountTransactionsService.WithdrawFundsFromAccount(accountId, withdrawFunds);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> TransferFundsBetweenAccounts(int fromAccountId, int toAccountId, decimal transferAmount)
        {
            var result = await _accountTransactionsService.TransferFunds(fromAccountId, toAccountId, transferAmount);
            if (result.HasError)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok();
        }
    }
}
