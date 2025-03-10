﻿using System.ComponentModel.DataAnnotations;

namespace BearingManagementApi.Models.DbEntities;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    
    public DateTime CreatedDate { get; set; }

}