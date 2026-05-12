using HealthcareRCM.API.Models;
using HealthcareRCM.API.DTOs;

namespace HealthcareRCM.API.Repositories;

public interface IClaimRepository
{
    Task<Claim?> GetByIdAsync(int claimId);
    Task<IEnumerable<Claim>> GetByPatientAsync(string patientId);
    Task<IEnumerable<Claim>> GetByStatusAsync(string status, int page, int pageSize);
    Task<IEnumerable<Claim>> GetDeniedForRetryAsync(int maxRetries = 3);
    Task<ClaimSummaryDto> GetSummaryAsync(DateTime startDate, DateTime endDate);
    Task<Claim> AddAsync(Claim claim);
    Task UpdateAsync(Claim claim);
    Task<bool> ExistsAsync(int claimId);
}
