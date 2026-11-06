namespace backend.DTOs.Tasks;
using System.ComponentModel.DataAnnotations;


public class CreateTaskDto
{
    [Required]
    [StringLength(120, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [Required] 
    [StringLength(30)] 
    public string Status { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Label { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Priority { get; set; } = string.Empty;
}