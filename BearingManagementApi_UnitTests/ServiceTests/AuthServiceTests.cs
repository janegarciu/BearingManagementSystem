using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.AuthModels;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Abstractions;
using BearingManagementApi.Services.Implementations;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace BearingManagementApi_UnitTests.ServiceTests;

[TestFixture]
    public class AuthServiceTests
    {
        private Mock<ITokenManager> _mockTokenManager;
        private Mock<IUserRepository> _mockUserRepository;
        private AuthService _service;

        [SetUp]
        public void Setup()
        {
            _mockTokenManager = new Mock<ITokenManager>();
            _mockUserRepository = new Mock<IUserRepository>();
            _service = new AuthService(_mockTokenManager.Object, _mockUserRepository.Object);
        }

        [Test]
        public async Task LoginUserAsync_ValidCredentials_ReturnsLoginResponse()
    {
        var request = new LoginRequest { Username = "testuser", Password = "password123" };
            var user = new User() { Id = 1, Username = "testuser", PasswordHash = new PasswordHasher<object>().HashPassword(null, "password123") };
            
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<UserModel>())).ReturnsAsync(user);
            _mockTokenManager.Setup(token => token.GenerateToken(user)).Returns("valid_token");
            
            var result = await _service.LoginUserAsync(request);
            
            Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(user.Id));
            Assert.That(result.Name, Is.EqualTo(user.Username));
            Assert.That(result.Token, Is.EqualTo("valid_token"));
        });
    }

    [Test]
        public async Task LoginUserAsync_InvalidPassword_ReturnsNull()
        {
            var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };
            var user = new User { Id = 1, Username = "testuser", PasswordHash = new PasswordHasher<object>().HashPassword(null, "password123") };
            
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<UserModel>())).ReturnsAsync(user);
            
            var result = await _service.LoginUserAsync(request);
            
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task LoginUserAsync_UserNotFound_ReturnsNull()
        {
            var request = new LoginRequest { Username = "unknownuser", Password = "password123" };
            
            _mockUserRepository.Setup(repo => repo.GetUserAsync(It.IsAny<UserModel>())).ReturnsAsync((User)null);
            
            var result = await _service.LoginUserAsync(request);
            
            Assert.IsNull(result);
        }

        [Test]
        public async Task RegisterUser_ValidRequest_ReturnsTrue()
        {
            var request = new RegisterRequest { Username = "newuser", Password = "password123" };
            var user = new User() { Id = 2, Username = "newuser", PasswordHash = "hashed_password" };
            
            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<UserModel>())).ReturnsAsync(user);
            
            var result = await _service.RegisterUser(request);
            
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task RegisterUser_InvalidRequest_ReturnsFalse()
        {
            var request = new RegisterRequest { Username = "", Password = "" };
            
            _mockUserRepository.Setup(repo => repo.AddUserAsync(It.IsAny<UserModel>()))!.ReturnsAsync((User)null);
            
            var result = await _service.RegisterUser(request);
            
            Assert.That(result, Is.False);
        }
    }