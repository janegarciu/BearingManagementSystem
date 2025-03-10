using BearingManagementApi.Controllers;
using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BearingManagementApi_UnitTests.ControllerTests;

[TestFixture]
public class BearingsControllerTests
{
    private Mock<IBearingsService> _mockBearingsService;
    private Mock<IBearingsRepository> _mockBearingsRepository;
    private BearingsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockBearingsService = new Mock<IBearingsService>();
        _mockBearingsRepository = new Mock<IBearingsRepository>();
        _controller = new BearingsController(_mockBearingsService.Object, _mockBearingsRepository.Object);
    }

    [Test]
    public async Task GetAll_ReturnsOkWithBearings()
    {
        var bearings = new List<Bearing> { new() { Id = 1, Name = "Bearing1" } };
        _mockBearingsRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bearings);

        var result = await _controller.GetAll() as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(bearings));
        });
    }

    [Test]
    public async Task GetById_ExistingId_ReturnsOk()
    {
        var bearing = new Bearing { Id = 1, Name = "Bearing1" };
        _mockBearingsRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(bearing);

        var result = await _controller.GetById(1) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo(bearing));
        });
    }

    [Test]
    public async Task GetById_NonExistingId_ReturnsNotFound()
    {
        _mockBearingsRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Bearing)null);

        var result = await _controller.GetById(1);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Create_ValidRequest_ReturnsCreated()
    {
        var request = new BearingRequest { Name = "Bearing1", Type = "Type1" };
        _mockBearingsService.Setup(service => service.CreateAsync(request)).ReturnsAsync(true);

        var result = await _controller.Create(request);

        Assert.That(result, Is.InstanceOf<CreatedResult>());
    }

    [Test]
    public async Task Create_InvalidRequest_ReturnsBadRequest()
    {
        var request = new BearingRequest { Name = "", Type = "" };

        var result = await _controller.Create(request) as BadRequestObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result.StatusCode, Is.EqualTo(400));
    }

    [Test]
    public async Task Update_ExistingId_ReturnsNoContent()
    {
        var request = new BearingRequest { Name = "UpdatedBearing", Type = "UpdatedType" };
        _mockBearingsService.Setup(service => service.UpdateAsync(1, request)).ReturnsAsync(true);

        var result = await _controller.Update(1, request);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Update_NonExistingId_ReturnsNotFound()
    {
        var request = new BearingRequest { Name = "UpdatedBearing", Type = "UpdatedType" };
        _mockBearingsService.Setup(service => service.UpdateAsync(1, request)).ReturnsAsync(false);

        var result = await _controller.Update(1, request);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task Delete_ExistingId_ReturnsNoContent()
    {
        _mockBearingsRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task Delete_NonExistingId_ReturnsNotFound()
    {
        _mockBearingsRepository.Setup(repo => repo.DeleteAsync(1)).ReturnsAsync(false);

        var result = await _controller.Delete(1);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }
}