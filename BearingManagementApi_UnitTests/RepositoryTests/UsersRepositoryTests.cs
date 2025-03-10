using BearingManagementApi.DbConfigurations;
using BearingManagementApi.Models.AuthModels;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BearingManagementApi_UnitTests.RepositoryTests;

[TestFixture]
public class UsersRepositoryTests
{
    private BearingDbContext _context;
    private UsersRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<BearingDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new BearingDbContext(options);
        _repository = new UsersRepository(_context);
    }

    [Test]
    public async Task GetUserAsync_ExistingUser_ReturnsUser()
    {
        var user = new User { Username = "testuser", PasswordHash = "hashedpassword" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var request = new UserModel { Username = "testuser", Password = "password123" };
        var result = await _repository.GetUserAsync(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(user.Username));
    }

    [Test]
    public async Task GetUserAsync_NonExistingUser_ReturnsNull()
    {
        var request = new UserModel { Username = "unknownuser", Password = "password123" };
        var result = await _repository.GetUserAsync(request);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task AddUserAsync_ValidUser_CreatesUserSuccessfully()
    {
        var request = new UserModel { Username = "newuser", Password = "securepassword" };
        var result = await _repository.AddUserAsync(request);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo(request.Username));
        Assert.That(result.PasswordHash, Is.Not.Empty);
    }
}