using backend.DTOs.Tasks;
using backend.Services.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("api/tasks")]
public class TaskController: ControllerBase
{
    private readonly TaskService  _service;

    public TaskController(TaskService service)
    {
        _service = service;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(Guid id)
    {
        var task = await _service.GetTaskByIdAsync(id);
        
        if (task == null)
            return NotFound();
        
        
        return Ok(task);
    }

    [HttpGet]
    public async Task<ActionResult<List<TaskResponseDto>>> GetAll()
    {
        var tasks = await _service.GetAllTasksAsync();

        return Ok(tasks);
        
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(CreateTaskDto dto)
    {
       var createTask =  await _service.CreateTaskAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new {id = createTask.Id},
            createTask
            );
            
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TaskResponseDto>> Update(Guid id, UpdateTaskDto dto)
    {
        var updatedTask = await _service.UpdateTaskAsync(id, dto);

        if (updatedTask == null)
            return NotFound();

        return Ok(updatedTask);
    }

    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deletedTask = await _service.DeleteTaskAsync(id);

        if (deletedTask == false )
            return NotFound();

        return NoContent();
    }
}