using AutoMapper;
using HealthcareRCM.API.DTOs;
using HealthcareRCM.API.Exceptions;
using HealthcareRCM.API.Models;
using HealthcareRCM.API.Repositories;
using HealthcareRCM.API.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;

namespace HealthcareRCM.Tests;

public class ClaimServiceTests
{
    private readonly Mock<IClaimRepository> _repo = new();
    private readonly IMapper _mapper;

    public ClaimServiceTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<CreateClaimDto, Claim>();
            cfg.CreateMap<Claim, ClaimResponseDto>()
                .ForMember(d => d.PatientName, o => o.MapFrom(s => s.Patient != null ? s.Patient.FirstName + " " + s.Patient.LastName : ""))
                .ForMember(d => d.ProviderName, o => o.MapFrom(s => s.Provider != null ? s.Provider.Name : ""));
        });
        _mapper = config.CreateMapper();
    }

    private ClaimService CreateSut() => new(_repo.Object, _mapper, NullLogger<ClaimService>.Instance);

    [Fact]
    public async Task GetClaimAsync_ThrowsNotFound_WhenMissing()
    {
        _repo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Claim?)null);
        var sut = CreateSut();
        await Assert.ThrowsAsync<NotFoundException>(() => sut.GetClaimAsync(99));
    }

    [Fact]
    public async Task UpdateStatusAsync_IncrementsRetryCount_OnDenial()
    {
        var claim = new Claim { ClaimId = 1, Status = "Pending", RetryCount = 0,
            Patient = new Patient { FirstName = "John", LastName = "Doe" },
            Provider = new Provider { Name = "Dr. Smith" } };
        _repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(claim);
        _repo.Setup(r => r.UpdateAsync(It.IsAny<Claim>())).Returns(Task.CompletedTask);

        var sut = CreateSut();
        await sut.UpdateStatusAsync(1, new UpdateClaimStatusDto("Denied", null, null, "CO-97", "Service not covered"));

        _repo.Verify(r => r.UpdateAsync(It.Is<Claim>(c => c.RetryCount == 1 && c.Status == "Denied")), Times.Once);
    }

    [Fact]
    public async Task CreateClaimAsync_SetsSubmissionDate()
    {
        var dto = new CreateClaimDto("P1", "PR1", "IP1", DateTime.Today, 1500m, "I21.0", "93010");
        var saved = new Claim { ClaimId = 5, PatientId = "P1", BilledAmount = 1500m,
            Patient = new Patient { FirstName = "Jane", LastName = "Doe" },
            Provider = new Provider { Name = "Dr. Jones" } };

        _repo.Setup(r => r.AddAsync(It.IsAny<Claim>())).ReturnsAsync(saved);
        var sut = CreateSut();
        var result = await sut.CreateClaimAsync(dto);

        Assert.Equal(5, result.ClaimId);
        _repo.Verify(r => r.AddAsync(It.Is<Claim>(c => c.SubmissionDate != null)), Times.Once);
    }
}
