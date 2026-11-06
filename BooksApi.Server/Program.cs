using BooksApi.Server;
using BooksApi.Server.Data;
using BooksApi.Server.Extensions;
using BooksApi.Server.Interfaces;
using BooksApi.Server.Models;
using BooksApi.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("myPostgres"), o => o.MigrationsHistoryTable("MigrationsHistory", "BooksSchema")));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<TokenServiceOptions>(builder.Configuration.GetSection("TokenServiceOptions"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,           // ← должно быть true
            

            // ↓ ЭТОГО НЕ ХВАТАЕТ ↓
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["TokenServiceOptions:SecretKey"]!))
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    opt.AddPolicy("Moderator", policy => policy.RequireRole("Admin", "Moderator"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

var newApi = app.MapGroup("/newApi");
newApi.MapGet("returnHello", () =>  "Hello");

app.MapControllers();
app.MapDefaultEndpoints();

app.UseFileServer();

app.Run();

