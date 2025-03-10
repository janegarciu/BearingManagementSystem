using System.Text;
using BearingManagementApi.DbConfigurations;
using BearingManagementApi.Models.Options;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Repositories.Implementations;
using BearingManagementApi.Services.Abstractions;
using BearingManagementApi.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var isTesting = builder.Environment.IsEnvironment("Testing");
var secretKey = Environment.GetEnvironmentVariable("Jwt:Token")
                ?? builder.Configuration["Jwt:Token"]; 
var key = Encoding.ASCII.GetBytes(secretKey ?? string.Empty);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.Configure<TokenOptionsModel>(options =>
{
    options.JwtAudience = Environment.GetEnvironmentVariable("Jwt:Audience")  ?? builder.Configuration["Jwt:Audience"];
    options.JwtExpiryHours = Convert.ToInt32(Environment.GetEnvironmentVariable("Jwt:ExpiryHours")  ?? builder.Configuration["Jwt:ExpiryHours"]);
    options.JwtIssuer = Environment.GetEnvironmentVariable("Jwt:Issuer")  ?? builder.Configuration["Jwt:Issuer"];
    options.JwtToken = Environment.GetEnvironmentVariable("Jwt:Token")  ?? builder.Configuration["Jwt:Token"];
});



builder.Services.AddAuthorization();
if (isTesting)
{
    builder.Services.AddDbContext<BearingDbContext>(options =>
        options.UseInMemoryDatabase("TestDb"));
}
else
{
    builder.Services.AddDbContext<BearingDbContext>(options =>
        options.UseSqlite("Data Source=bearings.db"));
}

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBearingsService, BearingsService>();
builder.Services.AddScoped<IBearingsRepository, BearingsRepository>();
builder.Services.AddScoped<IUserRepository, UsersRepository>();
builder.Services.AddScoped<ITokenManager, TokenManager>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
if (!isTesting)
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BearingDbContext>();
    await context.Database.MigrateAsync();
    SeedData.Initialize(context);
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();

public partial class Program
{
}