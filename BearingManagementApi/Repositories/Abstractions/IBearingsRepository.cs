using BearingManagementApi.Models.DbEntities;

namespace BearingManagementApi.Repositories.Abstractions;

public interface IBearingsRepository
{
    Task<IEnumerable<Bearing>> GetAllAsync();
    Task<Bearing?> GetByIdAsync(int id);
    Task AddAsync(Bearing bearing);
    Task<bool> UpdateAsync(Bearing bearing);
    Task<bool> DeleteAsync(int id);
}