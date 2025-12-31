using ApiSitemaClinico.Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
  .MinimumLevel.Debug()
  .WriteTo.Console()
  .WriteTo.File("logs/clinic-.log", rollingInterval: RollingInterval.Day)
  .CreateLogger();
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT (basic placeholder config)
var jwtKey = builder.Configuration["Jwt:Key"] ?? "fallback_secret_key";
var issuer = builder.Configuration["Jwt:Issuer"] ?? "clinic";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
      options.TokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
      };
    });

// Infrastructure & application services
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Application.Interfaces.IAuthService, ApiSitemaClinico.Clinic.Infrastructure.Services.AuthService>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IUserRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IAppointmentRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.AppointmentRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Application.Common.ICurrentUserService, ApiSitemaClinico.Clinic.Infrastructure.Services.CurrentUserService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IExamRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.ExamRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.ITreatmentRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.TreatmentRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IPatientRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.PatientRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IFileRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.FileRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Infrastructure.Services.IFileStorageService, ApiSitemaClinico.Clinic.Infrastructure.Services.FileStorageService>();
// File storage configuration
builder.Configuration["FileStorage:Root"] = builder.Configuration["FileStorage:Root"] ?? Path.Combine(Directory.GetCurrentDirectory(), "storage");

// Notifications & background services
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.INotificationRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.NotificationRepository>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Infrastructure.Services.INotificationService, ApiSitemaClinico.Clinic.Infrastructure.Services.NotificationService>();
builder.Services.AddHostedService<ApiSitemaClinico.Clinic.Infrastructure.Services.AppointmentReminderService>();
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IPaymentRepository, ApiSitemaClinico.Clinic.Infrastructure.Repositories.PaymentRepository>();
// Payments handlers/validators will be discovered by MediatR/FluentValidation scanning
// SMTP configuration (development defaults can be set in appsettings.Development.json)

// DbContext: placeholder - real connection string required (replace with MySQL connection)
// DbContext: configure MySQL connection (local development)
var defaultConn = builder.Configuration.GetConnectionString("DefaultConnection")
                  ?? "Server=localhost;Database=clinic_db;User=root;Password=;";
builder.Services.AddDbContext<ClinicDbContext>(options =>
{
  options.UseMySql(defaultConn, new MySqlServerVersion(new Version(8, 0, 33)));
});

// Data seeder
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Infrastructure.Services.IDataSeeder, ApiSitemaClinico.Clinic.Infrastructure.Services.DataSeeder>();

// Unit of Work
builder.Services.AddScoped<ApiSitemaClinico.Clinic.Domain.Interfaces.IUnitOfWork, ApiSitemaClinico.Clinic.Infrastructure.Persistence.UnitOfWork>();

// MediatR, AutoMapper and FluentValidation wiring require packages and assembly markers in real project
// Register MediatR, AutoMapper and FluentValidation scanning application assembly
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApiSitemaClinico.Clinic.Application.AssemblyMarker).Assembly));
builder.Services.AddAutoMapper(typeof(ApiSitemaClinico.Clinic.Application.AssemblyMarker).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ApiSitemaClinico.Clinic.Application.AssemblyMarker).Assembly);



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Apply EF migrations and seed data at startup (development)
using (var scope = app.Services.CreateScope())
{
  var services = scope.ServiceProvider;
  try
  {
    var db = services.GetRequiredService<ClinicDbContext>();
    // Apply migrations
    db.Database.Migrate();

    // Run seeder
    var seeder = services.GetService<ApiSitemaClinico.Clinic.Infrastructure.Services.IDataSeeder>();
    if (seeder != null)
    {
      seeder.SeedAsync().GetAwaiter().GetResult();
    }
  }
  catch (Exception ex)
  {
    Log.Logger.Error(ex, "An error occurred migrating or seeding the database.");
    throw;
  }
}

app.Run();
