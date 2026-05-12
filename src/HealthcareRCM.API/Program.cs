using HealthcareRCM.API.Data;
using HealthcareRCM.API.Middleware;
using HealthcareRCM.API.Repositories;
using HealthcareRCM.API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().WriteTo.Console()
    .WriteTo.File("logs/rcm-.log", rollingInterval: RollingInterval.Day).CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

builder.Services.AddDbContext<RcmDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("RcmDB")));

builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseSwagger(); app.UseSwaggerUI();
app.MapControllers();
app.Run();
