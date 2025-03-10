using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BearingManagementApi.Models.Api.Requests;

namespace BearingManagementApi_IntegrationTests;

[TestFixture]
    public class BearingsControllerTests
    {
        private CustomWebApplicationFactory<Program> _factory;
        private HttpClient _client;
        private string _jwtToken;

        [SetUp]
        public void Setup()
        {
            _factory = new CustomWebApplicationFactory<Program>();
            _client = _factory.CreateClient();
            _jwtToken = _factory.GenerateTestJwtToken();
        }

        private void AddAuthHeader()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        [Test]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("/bearings");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public async Task GetById_ReturnsNotFound_WhenBearingDoesNotExist()
        {
            AddAuthHeader();
            var response = await _client.GetAsync("/bearings/999");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Create_ReturnsCreatedResponse()
        {
            AddAuthHeader();
            var newBearing = new BearingRequest
            {
                Name = "Test Bearing",
                Type = "Type A",
                Manufacturer = "Company XYZ",
                Size = "10x20"
            };

            var response = await _client.PostAsJsonAsync("/bearings", newBearing);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenNameOrTypeIsMissing()
        {
            AddAuthHeader();
            var invalidBearing = new BearingRequest
            {
                Name = "",
                Type = "Type A",
                Manufacturer = "Company XYZ",
                Size = "10x20"
            };

            var response = await _client.PostAsJsonAsync("/bearings", invalidBearing);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public async Task Update_ReturnsNotFound_WhenBearingDoesNotExist()
        {
            AddAuthHeader();
            var updatedBearing = new BearingRequest
            {
                Name = "Updated Bearing",
                Type = "Type B",
                Manufacturer = "Updated Manufacturer",
                Size = "15x25"
            };

            var response = await _client.PutAsJsonAsync("/bearings/999", updatedBearing);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenBearingDoesNotExist()
        {
            AddAuthHeader();
            var response = await _client.DeleteAsync("/bearings/999");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TearDown]
        public void Cleanup()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }