using AutoMapper;
using HealthcareRCM.API.DTOs;
using HealthcareRCM.API.Exceptions;
using HealthcareRCM.API.Models;
using HealthcareRCM.API.Repositories;

namespace HealthcareRCM.API.Services;

public interface IClaimService
{
    Task<ClaimResponseDto> CreateClaimAsync(CreateClaimDto dto);
    Task<ClaimResponseDto> GetClaimAsync(int claimId);
    Task<ClaimResponseDto> UpdateStatusAsync(int claimId, UpdateClaimStatusDto dto);
    Task<IEnumerable<ClaimResponseDto>> GetDeniedClaimsForRetryAsync();
    Task<ClaimSummaryDto> GetSummaryAsync(DateTime startDate, DateTime endDate);
}

public class ClaimService(
    IClaimRepository repo,
    IMapper mapper,
    ILogger<ClaimService> logger) : IClaimService
{
    public async Task<ClaimResponseDto> CreateClaimAsync(CreateClaimDto dto)
    {
        logger.LogInformation("Creating claim for patient {PatientId}", dto.PatientId);
        var claim = mapper.Map<Claim>(dto);
        claim.SubmissionDate = DateTime.UtcNow;
        var saved = await repo.AddAsync(claim);
        logger.LogInformation("Claim {ClaimId} created", saved.ClaimId);
        return mapper.Map<ClaimResponseDto>(saved);
    }

    public async Task<ClaimResponseDto> GetClaimAsync(int claimId)
    {
        var claim = await repo.GetByIdAsync(claimId)
            ?? throw new NotFoundException($"Claim {claimId} not found");
        return mapper.Map<ClaimResponseDto>(claim);
    }

    public async Task<ClaimResponseDto> UpdateStatusAsync(int claimId, UpdateClaimStatusDto dto)
    {
        var claim = await repo.GetByIdAsync(claimId)
            ?? throw new NotFoundException($"Claim {claimId} not found");

        var previousStatus = claim.Status;
        claim.Status = dto.Status;
        claim.AllowedAmount = dto.AllowedAmount;
        claim.PaidAmount = dto.PaidAmount;
        claim.DenialCode = dto.DenialCode;
        claim.DenialReason = dto.DenialReason;

        if (dto.Status == "Denied" && previousStatus != "Denied")
            claim.RetryCount++;

        await repo.UpdateAsync(claim);
        logger.LogInformation("Claim {ClaimId} status updated: {Prev} → {New}", claimId, previousStatus, dto.Status);
        return mapper.Map<ClaimResponseDto>(claim);
    }

    public async Task<IEnumerable<ClaimResponseDto>> GetDeniedClaimsForRetryAsync()
    {
        var claims = await repo.GetDeniedForRetryAsync();
        return mapper.Map<IEnumerable<ClaimResponseDto>>(claims);
    }

    public Task<ClaimSummaryDto> GetSummaryAsync(DateTime startDate, DateTime endDate) =>
        repo.GetSummaryAsync(startDate, endDate);
}
