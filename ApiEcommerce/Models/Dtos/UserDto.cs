using System;

namespace ApiEcommerce.Models.Dtos;

public class UserDto
{
    public string Id { get; set; } = string.Empty;

    public string? Name { get; set; }
    public string? UserName { get; set; }    
}
