using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.Api.Responses;

namespace BearingManagementApi.Services.Abstractions;

public interface IAuthService
{
    Task<LoginResponse?> LoginUserAsync(LoginRequest request);
    Task<bool> RegisterUser(RegisterRequest request);
}