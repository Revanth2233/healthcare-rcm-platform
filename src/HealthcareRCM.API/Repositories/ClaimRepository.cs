using HealthcareRCM.API.Data;
using HealthcareRCM.API.DTOs;
using HealthcareRCM.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HealthcareRCM.API.Repositories;

public class ClaimRepository(RcmDbContext db) : IClaimRepository
{
    public async Task<Claim?> GetByIdAsync(int claimId) =>
        await db.Claims
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .FirstOrDefaultAsync(c => c.ClaimId == claimId);

    public async Task<IEnumerable<Claim>> GetByPatientAsync(string patientId) =>
        await db.Claims
            .Include(c => c.Provider)
            .Where(c => c.PatientId == patientId)
            .OrderByDescending(c => c.ServiceDate)
            .ToListAsync();

    public async Task<IEnumerable<Claim>> GetByStatusAsync(string status, int page, int pageSize) =>
        await db.Claims
            .Include(c => c.Patient)
            .Include(c => c.Provider)
            .Where(c => c.Status == status)
            .OrderByDescending(c => c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    public async Task<IEnumerable<Claim>> GetDeniedForRetryAsync(int maxRetries = 3) =>
        await db.Claims
            .Where(c => c.Status == "Denied" && c.RetryCount < maxRetries)
            .Include(c => c.Patient)
            .ToListAsync();

    public async Task<ClaimSummaryDto> GetSummaryAsync(DateTime startDate, DateTime endDate)
    {
        var claims = await db.Claims
            .Where(c => c.ServiceDate >= startDate && c.ServiceDate <= endDate)
            .ToListAsync();

        var total = claims.Count;
        var billed = claims.Sum(c => c.BilledAmount);
        var collected = claims.Sum(c => c.PaidAmount ?? 0);
        return new ClaimSummaryDto(
            total, billed, collected,
            claims.Count(c => c.Status == "Pending"),
            claims.Count(c => c.Status == "Denied"),
            total > 0 ? Math.Round(collected / billed * 100, 2) : 0
        );
    }

    public async Task<Claim> AddAsync(Claim claim)
    {
        db.Claims.Add(claim);
        await db.SaveChangesAsync();
        return claim;
    }

    public async Task UpdateAsync(Claim claim)
    {
        claim.UpdatedAt = DateTime.UtcNow;
        db.Claims.Update(claim);
        await db.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int claimId) =>
        await db.Claims.AnyAsync(c => c.ClaimId == claimId);
}
