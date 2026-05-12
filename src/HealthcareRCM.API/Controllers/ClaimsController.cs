using HealthcareRCM.API.DTOs;
using HealthcareRCM.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthcareRCM.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClaimsController(IClaimService svc) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClaimDto dto)
    {
        var result = await svc.CreateClaimAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.ClaimId }, result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id) =>
        Ok(await svc.GetClaimAsync(id));

    [HttpPut("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateClaimStatusDto dto) =>
        Ok(await svc.UpdateStatusAsync(id, dto));

    [HttpGet("denied/retry-queue")]
    public async Task<IActionResult> GetRetryQueue() =>
        Ok(await svc.GetDeniedClaimsForRetryAsync());

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        var start = startDate ?? DateTime.Today.AddDays(-30);
        var end = endDate ?? DateTime.Today;
        return Ok(await svc.GetSummaryAsync(start, end));
    }
}
