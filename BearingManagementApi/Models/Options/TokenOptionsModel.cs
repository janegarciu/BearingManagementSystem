namespace BearingManagementApi.Models.Options;

public class TokenOptionsModel
{
    public string? JwtAudience { get; set; }
    public int JwtExpiryHours { get; set; }
    public string? JwtIssuer { get; set; }
    public string? JwtToken { get; set; }
}