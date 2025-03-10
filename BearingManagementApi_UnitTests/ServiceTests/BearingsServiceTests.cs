using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Implementations;
using Moq;

namespace BearingManagementApi_UnitTests.ServiceTests;

[TestFixture]
    public class BearingsServiceTests
    {
        private Mock<IBearingsRepository> _mockBearingsRepository;
        private BearingsService _service;

        [SetUp]
        public void Setup()
        {
            _mockBearingsRepository = new Mock<IBearingsRepository>();
            _service = new BearingsService(_mockBearingsRepository.Object);
        }

        [Test]
        public async Task CreateAsync_ValidRequest_ReturnsTrue()
        {
            var request = new BearingRequest
            {
                Name = "Bearing A",
                Type = "Type A",
                Manufacturer = "Company A",
                Size = "10x20"
            };
            
            _mockBearingsRepository.Setup(repo => repo.AddAsync(It.IsAny<Bearing>())).Returns(Task.CompletedTask);

            var result = await _service.CreateAsync(request);
            
            Assert.That(result, Is.True);
            _mockBearingsRepository.Verify(repo => repo.AddAsync(It.IsAny<Bearing>()), Times.Once);
        }

        [Test]
        public async Task CreateAsync_ExceptionThrown_ReturnsFalse()
        {
            var request = new BearingRequest
            {
                Name = "Bearing A",
                Type = "Type A",
                Manufacturer = "Company A",
                Size = "10x20"
            };
            
            _mockBearingsRepository.Setup(repo => repo.AddAsync(It.IsAny<Bearing>())).ThrowsAsync(new Exception("DB error"));

            var result = await _service.CreateAsync(request);
            
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateAsync_ExistingId_ReturnsTrue()
        {
            var request = new BearingRequest
            {
                Name = "Updated Bearing",
                Type = "Updated Type",
                Manufacturer = "Updated Manufacturer",
                Size = "15x25"
            };

            var existingBearing = new Bearing { Id = 1, Name = "Old Bearing", Type = "Old Type", Manufacturer = "Old Manufacturer", Size = "10x20" };
            
            _mockBearingsRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(existingBearing);
            _mockBearingsRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Bearing>())).ReturnsAsync(true);

            var result = await _service.UpdateAsync(1, request);
            
            Assert.That(result, Is.True);
            _mockBearingsRepository.Verify(repo => repo.UpdateAsync(It.Is<Bearing>(b => b.Name == "Updated Bearing")), Times.Once);
        }

        [Test]
        public async Task UpdateAsync_NonExistingId_ReturnsFalse()
        {
            var request = new BearingRequest
            {
                Name = "Updated Bearing",
                Type = "Updated Type",
                Manufacturer = "Updated Manufacturer",
                Size = "15x25"
            };

            _mockBearingsRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Bearing)null);

            var result = await _service.UpdateAsync(1, request);
            
            Assert.That(result, Is.False);
        }
    }