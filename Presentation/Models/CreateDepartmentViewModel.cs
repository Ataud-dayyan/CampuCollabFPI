using System.ComponentModel.DataAnnotations;

namespace Presentation.Models;

public class CreateDepartmentViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}

public class UpdateDepartmentViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string? Description { get; set; }
}
