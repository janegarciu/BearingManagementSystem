using System.ComponentModel.DataAnnotations;

namespace BearingManagementApi.Models.DbEntities;

public class Bearing
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Type { get; set; } = string.Empty;
    [Required]
    public string Manufacturer { get; set; } = string.Empty;
    public string? Size { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}