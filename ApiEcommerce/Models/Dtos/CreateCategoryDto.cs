using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "El nombre es requerido.")]
    [MaxLength(50, ErrorMessage = "El nombre no puede tener mas de 50 caracteres")]
    [MinLength(3, ErrorMessage = "El nombre no puede tener menos de 50 caracteres")]
    public string Name { get; set; } = string.Empty;
}
