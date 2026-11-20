
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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(
            ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _transactionService
                .GetAllTransactionsAsync(GetUserId());

            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var transaction = await _transactionService
                .GetTransactionByIdAsync(id, GetUserId());

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(
            CreateTransactionDto dto)
        {
            try
            {
                var transaction = await _transactionService
                    .CreateTransactionAsync(dto, GetUserId());

                return CreatedAtAction(
                    nameof(GetTransactionById),
                    new { id = transaction.Id },
                    transaction);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            var deleted = await _transactionService
                .DeleteTransactionAsync(id, GetUserId());

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
