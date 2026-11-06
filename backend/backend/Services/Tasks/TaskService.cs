using backend.Data;
using backend.DTOs.Tasks;
using backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Tasks;

public class TaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TaskResponseDto>> GetAllTasksAsync()
    {

        var tasks = await _context.Tasks.Select(p => new TaskResponseDto
            {
                Id = p.Id,
                Title = p.Title,
                Status = p.Status,
                Label = p.Label,
                Priority = p.Priority,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                AssignedUserId = p.AssignedUserId
            }
        ).ToListAsync();

        return tasks;
    }

    public async Task<TaskResponseDto?> GetTaskByIdAsync(Guid id)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(p => p.Id == id);

        if (task != null)
        {
            var returnedTask = new TaskResponseDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status,
                Label = task.Label,
                Priority = task.Priority,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                AssignedUserId = task.AssignedUserId
            };



            return returnedTask;
        }

        else
        {
            return null;
        }
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