using BearingManagementApi.Models.AuthModels;
using BearingManagementApi.Models.DbEntities;

namespace BearingManagementApi.Repositories.Abstractions;

public interface IUserRepository
{
    Task<User?> GetUserAsync(UserModel user);
    Task<User> AddUserAsync(UserModel user);
}