
using FinanceMarketSimulator.BLL.DTOs;
using FinanceMarketSimulator.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceMarketSimulator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FinancialAccountController : ControllerBase
    {
        private readonly IFinancialAccountService _accountService;

        public FinancialAccountController(
            IFinancialAccountService accountService)
        {
            _accountService = accountService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAccounts()
        {
            var accounts = await _accountService
                .GetAllAccountsAsync(GetUserId());

            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById(int id)
        {
            var account = await _accountService
                .GetAccountByIdAsync(id, GetUserId());

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(
            CreateFinancialAccountDto dto)
        {
            var account = await _accountService
                .CreateAccountAsync(dto, GetUserId());

            return CreatedAtAction(
                nameof(GetAccountById),
                new { id = account.Id },
                account);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var deleted = await _accountService
                .DeleteAccountAsync(id, GetUserId());

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
