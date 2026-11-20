
using FinanceMarketSimulator.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceMarketSimulator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? string.Empty;
        }

        [HttpGet("last-30-days")]
        public async Task<IActionResult> GetLast30DaysReport()
        {
            var report = await _reportService
                .GetLast30DaysReportAsync(GetUserId());

            return Ok(report);
        }

        [HttpGet("last-90-days")]
        public async Task<IActionResult> GetLast90DaysReport()
        {
            var report = await _reportService
                .GetLast90DaysReportAsync(GetUserId());

            return Ok(report);
        }

        [HttpGet("custom")]
        public async Task<IActionResult> GetCustomReport(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var report = await _reportService
                    .GetCustomReportAsync(
                        GetUserId(),
                        startDate,
                        endDate);

                return Ok(report);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
