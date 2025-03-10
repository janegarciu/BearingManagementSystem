using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Abstractions;

namespace BearingManagementApi.Services.Implementations;

public class BearingsService : IBearingsService
{
    private readonly IBearingsRepository _bearingsRepository;

    public BearingsService(IBearingsRepository bearingsRepository)
    {
        _bearingsRepository = bearingsRepository;
    }

    public async Task<bool> CreateAsync(BearingRequest request)
    {
        try
        {
            await _bearingsRepository.AddAsync(new Bearing
            {
                Name = request.Name,
                Type = request.Type,
                Manufacturer = request.Manufacturer,
                Size = request.Size,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(int id, BearingRequest request)
    {
        var bearing = await _bearingsRepository.GetByIdAsync(id);
        if (bearing == null) return false;
        bearing.Name = request.Name;
        bearing.Type = request.Type;
        bearing.Manufacturer = request.Manufacturer;
        bearing.Size = request.Size;
        bearing.UpdatedAt = DateTime.UtcNow;

        return await _bearingsRepository.UpdateAsync(bearing);
    }
}