using BankingSystemAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystemAPI.Controllers
{
    [ApiController]
    [Route("api/bank")]
    public class BankController : ControllerBase
    {
        private readonly BankService bankService;

        public BankController(BankService bankService)
        {
            this.bankService = bankService;
        }

        [HttpPost("user")]
        public IActionResult CreateUser()
        {
            var user = bankService.CreateUser();
            return Ok(user);
        }

        [HttpPost("user/{userId}/account")]
        public IActionResult CreateAccount(int userId)
        {
            var user = bankService.GetUserById(userId);
            if (user == null)
                return NotFound("User not found.");

            var account = bankService.CreateAccount(user);
            return Ok(account);
        }

        [HttpDelete("user/{userId}/account/{accountId}")]
        public IActionResult DeleteAccount(int userId, int accountId)
        {
            var user = bankService.GetUserById(userId);
            if (user == null)
                return NotFound("User not found.");

            var account = user.Accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account == null)
                return NotFound("Account not found.");

            bankService.DeleteAccount(user, accountId);
            return Ok("Account deleted successfully.");
        }

        [HttpPost("account/{accountId}/deposit")]
        public IActionResult Deposit(int accountId, [FromBody] decimal amount)
        {
            var account = bankService.GetAccountById(accountId);
            if (account == null)
                return NotFound("Account not found.");

            var success = bankService.Deposit(account, amount);
            if (!success)
                return BadRequest("Deposit not allowed. Maximum deposit limit is $10,000.");

            return Ok("Deposit successful.");
        }

        [HttpPost("account/{accountId}/withdraw")]
        public IActionResult Withdraw(int accountId, [FromBody] decimal amount)
        {
            var account = bankService.GetAccountById(accountId);
            if (account == null)
                return NotFound("Account not found.");

            var success = bankService.Withdraw(account, amount);
            if (!success)
                return BadRequest("Withdrawal not allowed. Check balance and withdrawal amount.");

            return Ok("Withdrawal successful.");
        }
    }


}
