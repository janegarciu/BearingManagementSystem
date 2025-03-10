using BearingManagementApi.Models.Api.Requests;
using BearingManagementApi.Models.Api.Responses;
using BearingManagementApi.Models.AuthModels;
using BearingManagementApi.Repositories.Abstractions;
using BearingManagementApi.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace BearingManagementApi.Services.Implementations;

public class AuthService(ITokenManager tokenManager, IUserRepository userRepository) : IAuthService
{
    public async Task<LoginResponse?> LoginUserAsync(LoginRequest request)
    {
        var user = await userRepository.GetUserAsync(new UserModel
        {
            Username = request.Username,
            Password = request.Password
        });
        if (user == null) return null;
        var passwordHasher = new PasswordHasher<object>();
        var result = passwordHasher.VerifyHashedPassword(user.Username, user.PasswordHash, request.Password);

        if (result != PasswordVerificationResult.Success) return null;
        var token = tokenManager.GenerateToken(user);
        return new LoginResponse
        {
            Id = user.Id,
            Name = user.Username,
            Token = token
        };
    }
    
    public async Task<bool> RegisterUser(RegisterRequest request)
    {
        var userModel = new UserModel
        {
            Username = request.Username,
            Password = request.Password
        };
        var user = await userRepository.GetUserAsync(userModel);
        if (user != null) return false;
        user = await userRepository.AddUserAsync(userModel);
        return user != null;
    }
}