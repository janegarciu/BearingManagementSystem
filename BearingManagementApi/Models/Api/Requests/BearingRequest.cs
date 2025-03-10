namespace BearingManagementApi.Models.Api.Requests;

public record BearingRequest
{
    public string Name { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Manufacturer { get; init; } = string.Empty;
    public string? Size { get; init; }
}