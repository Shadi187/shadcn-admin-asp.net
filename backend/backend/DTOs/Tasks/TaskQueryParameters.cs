namespace backend.DTOs.Tasks;

public class TaskQueryParameters
{
    public string? Status { get; set; } = string.Empty;
    public string? Priority { get; set; } = string.Empty;
    public string? Search { get; set; } = string.Empty;
    public string? SortBy { get; set; } = string.Empty;
    public string SortDirection { get; set; } = "desc";
    public string? Label { get; set; } = string.Empty;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

/*
 
 
   
   SortDirection	string	"desc"
   PageNumber	int	1
   PageSize	int	10
  
 */