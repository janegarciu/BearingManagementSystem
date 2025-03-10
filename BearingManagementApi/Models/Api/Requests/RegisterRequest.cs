﻿namespace BearingManagementApi.Models.Api.Requests;

public record RegisterRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}