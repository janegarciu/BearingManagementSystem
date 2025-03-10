using BearingManagementApi.Models.DbEntities;

namespace BearingManagementApi.Services.Abstractions;

public interface ITokenManager
{
    string? GenerateToken(User user);
}