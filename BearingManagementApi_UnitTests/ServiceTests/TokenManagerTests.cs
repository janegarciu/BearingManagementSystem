using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Models.Options;
using BearingManagementApi.Services.Implementations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;

namespace BearingManagementApi_UnitTests.ServiceTests;

[TestFixture]
    public class TokenManagerTests
    {
        private TokenManager _tokenManager;
        private Mock<IOptions<TokenOptionsModel>> _mockOptions;

        [SetUp]
        public void Setup()
        {
            _mockOptions = new Mock<IOptions<TokenOptionsModel>>();
            _mockOptions.Setup(opt => opt.Value).Returns(new TokenOptionsModel
            {
                JwtToken = "supersecretkey12345678901234567890", // Minimum 32 characters for HMACSHA256
                JwtExpiryHours = 1,
                JwtIssuer = "testIssuer",
                JwtAudience = "testAudience"
            });
            _tokenManager = new TokenManager(_mockOptions.Object);
        }

        [Test]
        public void GenerateToken_ValidUser_ReturnsValidJwtToken()
    {
        var user = new User { Id = 1, Username = "testuser" };
            var token = _tokenManager.GenerateToken(user);

            Assert.That(token, Is.Not.Null);
            Assert.That(token, Is.Not.Empty);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_mockOptions.Object.Value.JwtToken);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _mockOptions.Object.Value.JwtIssuer,
                ValidAudience = _mockOptions.Object.Value.JwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            Assert.That(validatedToken, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(validatedToken, Is.InstanceOf<JwtSecurityToken>());
            Assert.That(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value, Is.EqualTo(user.Id.ToString()));
            Assert.That(principal.FindFirst(ClaimTypes.Name)?.Value, Is.EqualTo(user.Username));
        });
    }

    [Test]
        public void GenerateToken_NullUser_ReturnsNull()
        {
            var token = _tokenManager.GenerateToken(null);
            Assert.That(token, Is.Null);
        }
    }