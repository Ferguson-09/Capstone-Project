
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
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetService _budgetService;

        public BudgetController(IBudgetService budgetService)
        {
            _budgetService = budgetService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBudgets()
        {
            var budgets = await _budgetService
                .GetAllBudgetsAsync(GetUserId());

            return Ok(budgets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBudgetById(int id)
        {
            var budget = await _budgetService
                .GetBudgetByIdAsync(id, GetUserId());

            if (budget == null)
            {
                return NotFound();
            }

            return Ok(budget);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBudget(
            CreateBudgetDto dto)
        {
            try
            {
                var budget = await _budgetService
                    .CreateBudgetAsync(dto, GetUserId());

                return CreatedAtAction(
                    nameof(GetBudgetById),
                    new { id = budget.Id },
                    budget);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var deleted = await _budgetService
                .DeleteBudgetAsync(id, GetUserId());

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
