using BearingManagementApi.Models.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace BearingManagementApi.DbConfigurations;

public class BearingDbContext(DbContextOptions<BearingDbContext> options) : DbContext(options)
{
    public DbSet<Bearing> Bearings { get; set; }
    public DbSet<User> Users { get; set; }
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=bearings.db");
        }
    }
}