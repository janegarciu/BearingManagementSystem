using BearingManagementApi.DbConfigurations;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BearingManagementApi.Repositories.Implementations;

public class BearingsRepository(BearingDbContext context) : IBearingsRepository
{
    public async Task<IEnumerable<Bearing>> GetAllAsync() => await context.Bearings.ToListAsync();
    public async Task<Bearing?> GetByIdAsync(int id) => await context.Bearings.FindAsync(id);
    public async Task AddAsync(Bearing bearing)
    {
        bearing.CreatedAt = DateTime.UtcNow;
        bearing.UpdatedAt = DateTime.UtcNow;
        context.Bearings.Add(bearing);
        await context.SaveChangesAsync();
    }
    public async Task<bool> UpdateAsync(Bearing bearing)
    {
        var bearings = await context.Bearings.AnyAsync(b => b.Id == bearing.Id);
        if (!bearings) return false;
        context.Bearings.Update(bearing);
        await context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var bearing = await context.Bearings.FindAsync(id);
        if (bearing == null) return false;
        var numberOfRowsAffected = await context.Bearings.Where(x => x.Id == id).ExecuteDeleteAsync();
        return numberOfRowsAffected > 0;
    }
}