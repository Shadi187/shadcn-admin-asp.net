namespace backend.Entities;

public class TaskItem
{
     public Guid Id { get; set; }
     public string Title { get; set; } = string.Empty;
     public string Status { get; set; } = string.Empty;
     public string Label { get; set; } = string.Empty;
     public string Priority { get; set; } = string.Empty;
     public DateTime CreatedAt { get; set; }
     public DateTime UpdatedAt { get; set; }
     public string? AssignedUserId { get; set; } 
     public ApplicationUser? AssignedUser { get; set; }
     
    
}