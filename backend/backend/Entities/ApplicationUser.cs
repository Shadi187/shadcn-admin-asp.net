using Microsoft.AspNetCore.Identity;

namespace backend.Entities;

public class ApplicationUser:IdentityUser
{
    public string FirstName {get; set;} = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public ICollection<TaskItem> AssignedTasks { get; set; }
        = new List<TaskItem>();
    
}

/*
 
 firstName: z.string(),
   lastName: z.string(), 
 */