using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BankingEcosystem.Backend.Data;
using BankingEcosystem.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// ─── Services Registration ───
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// CORS — allow Flutter mobile client (emulator & physical device)
builder.Services.AddCors(options =>
{
    options.AddPolicy("MobileClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// EF Core
builder.Services.AddDbContext<BankingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Application Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<TransactionService>();
builder.Services.AddScoped<CardService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<FavoriteTransferService>();
builder.Services.AddScoped<BankService>();

// JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? "BankingEcosystemSuperSecretKey2026!@#$%^&*()";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "BankingEcosystem",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "BankingEcosystemClient",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });
builder.Services.AddAuthorization();

// ─── App Pipeline ───
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("MobileClient");
// NOTE: UseHttpsRedirection removed — mobile client calls plain HTTP
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
