using BearingManagementApi.Models.DbEntities;

namespace BearingManagementApi.DbConfigurations;

public static class SeedData
{
    public static void Initialize(BearingDbContext context)
    {
        if (!context.Bearings.Any())
        {
            context.Bearings.AddRange(
                new Bearing { Name = "Ball Bearing", Type = "Ball", Manufacturer = "SKF", Size = "50mm", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Bearing { Name = "Roller Bearing", Type = "Roller", Manufacturer = "Timken", Size = "75mm", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Bearing { Name = "Thrust Bearing", Type = "Thrust", Manufacturer = "NSK", Size = "100mm", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            ); 
        }

        if (!context.Users.Any())
        {
            context.Users.AddRange(
                new User
                {
                    Username = "janegarciu",
                    PasswordHash = "AQAAAAIAAYagAAAAEDDzLnbyrhg/N9FCJzyL8GAMZBy9jbC9Xp5Gy/BFFbfPmnJf6kkdKqJ6m4TnRiFpuw==", //Pass:Jane123
                    CreatedDate = DateTime.UtcNow,
                }
            ); 
        }
        
        context.SaveChanges();
    }
}