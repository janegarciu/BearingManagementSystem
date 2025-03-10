using BearingManagementApi.Models.Api.Requests;

namespace BearingManagementApi.Services.Abstractions;

public interface IBearingsService
{
    Task<bool> CreateAsync(BearingRequest request);
    Task<bool> UpdateAsync(int id, BearingRequest request);
}