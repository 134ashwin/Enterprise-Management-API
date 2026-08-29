using DMello.Application.Auth;
using DMello.Application.Common.Interfaces;
using DMello.Application.Common.Options;
using DMello.Domain.Interfaces;
using DMello.Infrastructure.Authentication;
using DMello.Infrastructure.Data;
using DMello.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

#region All Regisetered Service via DI
// 2. Registering ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IJwtService, JwtService>();

// 2. UserRepository (Data access layer)
builder.Services.AddScoped<IUserRepository, UserRepository>();

// 3. AuthService (Business logic layer)
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

// Bind appsettings.json "Jwt" section directly to JwtOptions class
builder.Services.Configure<JwtOptions>
    (builder.Configuration.GetSection(JwtOptions.SectionName));

#region // Added necessary JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKey1234567890123456")
        )
    };
});

#endregion



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


var app = builder.Build();


#region Configure Http request pipeline
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseSwaggerUI(options =>
    {
        // Points Swagger UI to the Native .NET OpenAPI JSON endpoint
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.RoutePrefix = "swagger"; // Opens UI at /swagger
    });
}
#endregion

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
