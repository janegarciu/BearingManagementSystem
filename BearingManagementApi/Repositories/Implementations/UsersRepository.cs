using BearingManagementApi.DbConfigurations;
using BearingManagementApi.Models.AuthModels;
using BearingManagementApi.Models.DbEntities;
using BearingManagementApi.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BearingManagementApi.Repositories.Implementations;

public class UsersRepository : IUserRepository
{
    private readonly BearingDbContext _context;
    public UsersRepository(BearingDbContext context) => _context = context;

    public async Task<User?> GetUserAsync(UserModel user) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Username == user.Username);

    public async Task<User> AddUserAsync(UserModel user)
    {
        var passwordHasher = new PasswordHasher<object>();
        var hashedPassword = passwordHasher.HashPassword(user.Username, user.Password);
        var newUser = new User()
        {
            Username = user.Username,
            PasswordHash = hashedPassword,
            CreatedDate = DateTime.UtcNow
        };
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }
}