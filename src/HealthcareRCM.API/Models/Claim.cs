namespace HealthcareRCM.API.Models;

public class Claim
{
    public int ClaimId { get; set; }
    public string PatientId { get; set; } = string.Empty;
    public string ProviderId { get; set; } = string.Empty;
    public string InsurancePlanId { get; set; } = string.Empty;
    public DateTime ServiceDate { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public decimal BilledAmount { get; set; }
    public decimal? AllowedAmount { get; set; }
    public decimal? PaidAmount { get; set; }
    public string Status { get; set; } = "Pending";
    public string? DenialCode { get; set; }
    public string? DenialReason { get; set; }
    public int RetryCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Patient Patient { get; set; } = null!;
    public Provider Provider { get; set; } = null!;
}

public class Patient
{
    public string PatientId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string InsuranceMemberId { get; set; } = string.Empty;
    public ICollection<Claim> Claims { get; set; } = [];
}

public class Provider
{
    public string ProviderId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string NPI { get; set; } = string.Empty;
    public ICollection<Claim> Claims { get; set; } = [];
}
