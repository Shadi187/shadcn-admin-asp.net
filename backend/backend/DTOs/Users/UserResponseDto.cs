using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Users;

public class UserResponseDto
{
    [Required]
    [StringLength(20)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    public string Status { get; set; } = string.Empty;

}