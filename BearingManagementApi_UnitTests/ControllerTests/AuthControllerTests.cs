using BearingManagementApi.Controllers;
using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.Api.Responses;
using BearingManagementApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BearingManagementApi_UnitTests.ControllerTests;

[TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private AuthController _controller;

        [SetUp]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _controller = new AuthController(_mockAuthService.Object);
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsOk()
    {
        var request = new LoginRequest { Username = "testuser", Password = "password123" };
            var response = new LoginResponse{ Token = "valid_token" };

            _mockAuthService.Setup(service => service.LoginUserAsync(request)).ReturnsAsync(response);

            var result = await _controller.Login(request) as OkObjectResult;

            Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(response));
        });
    }

    [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        var request = new LoginRequest { Username = "testuser", Password = "wrongpassword" };
            
            _mockAuthService.Setup(service => service.LoginUserAsync(request)).ReturnsAsync((LoginResponse)null);

            var result = await _controller.Login(request) as UnauthorizedObjectResult;

            Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(401));
            Assert.That(result.Value, Is.EqualTo("Invalid username or password"));
        });
    }

    [Test]
        public async Task Register_ValidRequest_ReturnsOk()
        {
            var request = new RegisterRequest { Username = "newuser", Password = "password123"};
            
            _mockAuthService.Setup(service => service.RegisterUser(request)).ReturnsAsync(true);

            var result = await _controller.Register(request) as OkResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));
        }

        [Test]
        public async Task Register_InvalidRequest_ReturnsBadRequest()
        {
            var request = new RegisterRequest { Username = "", Password = ""};
            
            _mockAuthService.Setup(service => service.RegisterUser(request)).ReturnsAsync(false);

            var result = await _controller.Register(request) as BadRequestResult;

            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }
    }