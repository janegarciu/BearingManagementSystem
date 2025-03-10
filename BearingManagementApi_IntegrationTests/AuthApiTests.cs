using System.Net;
using System.Net.Http.Json;
using BearingManagementApi.Models.Api.Requests;

namespace BearingManagementApi_IntegrationTests;

[TestFixture]
public class AuthControllerTests
{
    private CustomWebApplicationFactory<Program> _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Test]
    public async Task Register_ReturnsOk_WhenRegistrationIsSuccessful()
    {
        var newUser = new RegisterRequest
        {
            Username = "testuser5",
            Password = "Test@123"
        };

        var response = await _client.PostAsJsonAsync("/auth/register", newUser);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Register_ReturnsBadRequest_WhenRegistrationFails()
    {
        var existingUser = new RegisterRequest
        {
            Username = "existinguser",
            Password = "Test@123"
        };
        var path = "/auth/register";
        var response = await _client.PostAsJsonAsync(path, existingUser);
        var secondResponse = await _client.PostAsJsonAsync(path, existingUser);
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(secondResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        });
    }

    [Test]
    public async Task Login_ReturnsOk_WithValidCredentials()
    {
        var newUser = new RegisterRequest
        {
            Username = "testuser",
            Password = "Test@123"
        };
        await _client.PostAsJsonAsync("/auth/register", newUser);

        var loginRequest = new LoginRequest
        {
            Username = "testuser",
            Password = "Test@123"
        };

        var response = await _client.PostAsJsonAsync("/auth/login", loginRequest);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Login_ReturnsUnauthorized_WithInvalidCredentials()
    {
        var invalidLogin = new LoginRequest
        {
            Username = "wronguser",
            Password = "WrongPassword"
        };

        var response = await _client.PostAsJsonAsync("/auth/login", invalidLogin);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [TearDown]
    public void Cleanup()
    {
        _client.Dispose();
        _factory.Dispose();
    }
}