namespace HealthcareRCM.API.DTOs;

public record CreateClaimDto(
    string PatientId,
    string ProviderId,
    string InsurancePlanId,
    DateTime ServiceDate,
    decimal BilledAmount,
    string DiagnosisCode,
    string ProcedureCode
);

public record UpdateClaimStatusDto(
    string Status,
    decimal? AllowedAmount,
    decimal? PaidAmount,
    string? DenialCode,
    string? DenialReason
);

public record ClaimResponseDto(
    int ClaimId,
    string PatientId,
    string PatientName,
    string ProviderId,
    string ProviderName,
    DateTime ServiceDate,
    decimal BilledAmount,
    decimal? PaidAmount,
    string Status,
    string? DenialCode,
    int RetryCount,
    DateTime CreatedAt
);

public record ClaimSummaryDto(
    int TotalClaims,
    decimal TotalBilled,
    decimal TotalCollected,
    int PendingCount,
    int DeniedCount,
    decimal CollectionRate
);
