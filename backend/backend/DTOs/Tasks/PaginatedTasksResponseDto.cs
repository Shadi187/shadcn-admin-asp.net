namespace backend.DTOs.Tasks;

public class PaginatedTasksResponseDto
{
    public List<TaskResponseDto> Items { get; set; } = new();
    
    public int TotalCount { get; set; }
    
    public int PageNumber { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalPages { get; set; }
}