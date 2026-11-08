using backend.Data;
using backend.DTOs.Tasks;
using backend.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Tasks;

public class TaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<PaginatedTasksResponseDto> GetAllTasksAsync(
        TaskQueryParameters taskQuery
        )
    {
        
        PaginatedTasksResponseDto returnedQuery = new PaginatedTasksResponseDto();

        returnedQuery.PageNumber = taskQuery.PageNumber;
        returnedQuery.PageSize = taskQuery.PageSize;

        var escapedSearch = taskQuery.Search!
            .Replace("!", "!!")
            .Replace("%", "!%")
            .Replace("_", "!_");

        var pattern = $"%{escapedSearch}%";
        
        

        var skip = ((long)taskQuery.PageNumber - 1) * taskQuery.PageSize;
        
        var propertyName = taskQuery.SortBy?.ToLowerInvariant() switch
        {
            "title" => nameof(TaskItem.Title),
            "createdat" => nameof(TaskItem.CreatedAt),
            _ => nameof(TaskItem.CreatedAt)
        };

        IQueryable<TaskItem> tasks = _context.Tasks;
        
        //  FILTERING
        if (!string.IsNullOrWhiteSpace(taskQuery.Status))
        {
            tasks = tasks.Where(e => e.Status == taskQuery.Status);
        }

        if (!string.IsNullOrWhiteSpace(taskQuery.Priority))
        {
            tasks = tasks.Where(e => e.Priority == taskQuery.Priority);
        }
        
        if (!string.IsNullOrWhiteSpace(taskQuery.Label))
        {
            tasks = tasks.Where(e =>EF.Functions.ILike(e.Label, $"%{taskQuery.Label}%"));
        }

        if (!string.IsNullOrWhiteSpace(taskQuery.Search))
        {
            tasks = tasks.Where(e => EF.Functions.ILike(e.Title, pattern, "!") ||
                                     EF.Functions.ILike(e.Label, pattern, escapeCharacter:"!"));
        }

        

        
        //COUNTING
        returnedQuery.TotalCount = await tasks
            .CountAsync();
        
        returnedQuery.TotalPages = (int)Math.Ceiling((double)returnedQuery.TotalCount/ returnedQuery.PageSize);
        
        //SORTING
        if (taskQuery.SortDirection == "asc")
        {
            tasks = tasks.OrderBy(e =>EF.Property<object>(e,propertyName)).ThenBy(e => e.Id);
        }

        else
        {
            tasks = tasks.OrderByDescending(e => EF.Property<object>(e,propertyName)).ThenBy(e => e.Id);

        }

        
        
        //SELECTING
        var query = tasks.Select(p => new TaskResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Status = p.Status,
                Label = p.Label,
                Priority = p.Priority,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                AssignedUserId = p.AssignedUserId,
                AssignedUserName = p.AssignedUser == null ? null : p.AssignedUser.FirstName + " " + p.AssignedUser.LastName 
                
                
            }
        );



        if (skip > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException(
                nameof(taskQuery.PageNumber),
                "Pagination offset is too large"
            );
        }
        
         int offset = (int)skip;
        
         
         
        //SKIPPING
        var pageQuery =  query
            .Skip(offset)
            .Take(taskQuery.PageSize);
            
        Console.WriteLine("SQL TIMEEE  \n" + pageQuery.ToQueryString());

        returnedQuery.Items = await pageQuery.ToListAsync();
        
        return returnedQuery;

    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(Guid id)
    {
        var task =  _context.Tasks.Where(p => p.Id == id);

        var selectedTasks = task.Select(p => new TaskResponseDto
        {
            Id = p.Id,
            Title = p.Title,
            Status = p.Status,
            Label = p.Label,
            Priority = p.Priority,
            CreatedAt = p.CreatedAt,
            UpdatedAt = p.UpdatedAt,
            AssignedUserId = p.AssignedUserId   
        });

        var querySql = selectedTasks.ToQueryString();
        Console.WriteLine(querySql);

        return await selectedTasks.FirstOrDefaultAsync();
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto dto)
    {
        var newTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Status = dto.Status,
            Label = dto.Label,
            Priority = dto.Priority,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,


        };

        _context.Tasks.Add(newTask);
        await _context.SaveChangesAsync();

        return ToResponse(newTask);

    }

    public async Task<TaskResponseDto?> UpdateTaskAsync(Guid id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(p => p.Id == id);

        if (task == null)
            return null;


        task.Title = dto.Title;
        task.Status = dto.Status;
        task.Label = dto.Label;
        task.Priority = dto.Priority;
        task.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ToResponse(task);

    }

    public async Task<bool> DeleteTaskAsync(Guid id)
    {
        var task =  await _context.Tasks.FirstOrDefaultAsync(p => p.Id == id);

        if (task == null)
            return false;

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        
        
        
        
        return true;
    }
    
    private static TaskResponseDto ToResponse(TaskItem newTask)
    {
        return new TaskResponseDto
        {
            Id = newTask.Id,
            Title = newTask.Title,
            Status = newTask.Status,
            Label = newTask.Label,
            Priority = newTask.Priority,
            CreatedAt = newTask.CreatedAt,
            UpdatedAt = newTask.UpdatedAt,
        };
        
    
    }


    
    

}