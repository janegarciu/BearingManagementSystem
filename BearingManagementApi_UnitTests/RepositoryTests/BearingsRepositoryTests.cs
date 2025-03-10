using BearingManagementApi.DbConfigurations;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace BearingManagementApi_UnitTests.RepositoryTests
{
    [TestFixture]
    public class BearingsRepositoryTests
    {
        private BearingDbContext _context;
        private BearingsRepository _repository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<BearingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new BearingDbContext(options);
            _repository = new BearingsRepository(_context);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllBearings()
        {
            _context.Bearings.AddRange(new List<Bearing>
            {
                new Bearing { Id = 1, Name = "Bearing A", Type = "Type A" },
                new Bearing { Id = 2, Name = "Bearing B", Type = "Type B" }
            });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsBearing()
        {
            var bearing = new Bearing { Id = 1, Name = "Bearing A", Type = "Type A" };
            _context.Bearings.Add(bearing);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(bearing.Id));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(99);
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidBearing_SavesSuccessfully()
        {
            var bearing = new Bearing { Name = "New Bearing", Type = "New Type" };
            await _repository.AddAsync(bearing);

            var result = await _context.Bearings.FirstOrDefaultAsync(b => b.Name == "New Bearing");
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_ExistingBearing_UpdatesSuccessfully()
        {
            var bearing = new Bearing { Id = 1, Name = "Old Bearing", Type = "Old Type" };
            _context.Bearings.Add(bearing);
            await _context.SaveChangesAsync();

            bearing.Name = "Updated Bearing";
            var result = await _repository.UpdateAsync(bearing);

            Assert.That(result, Is.True);
            var updatedBearing = await _context.Bearings.FindAsync(1);
            Assert.That(updatedBearing.Name, Is.EqualTo("Updated Bearing"));
        }

        [Test]
        public async Task UpdateAsync_NonExistingBearing_ReturnsFalse()
        {
            var bearing = new Bearing { Id = 99, Name = "NonExisting", Type = "Type" };
            var result = await _repository.UpdateAsync(bearing);
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            var result = await _repository.DeleteAsync(99);
            Assert.That(result, Is.False);
        }
    }
}