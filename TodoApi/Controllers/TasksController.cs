using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApi.DTO;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(ITaskService service) : ControllerBase
{

    private int GetUserId()
    {
        var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(strId) || !int.TryParse(strId, out var id))
        {
            throw new InvalidOperationException("The token has not contain a valid UserId.");
        }
        return id;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll()
    { 
        var userId = GetUserId();
        var task =  await service.GetAllAsync(userId);
        return Ok(task);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskReadDto>> Get(int id)
    {
        var userId = GetUserId();
        var task = await service.GetTaskByIdAsync(id, userId);
        if (task == null) return NotFound();
        return Ok(task);
    }
    
    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create(TaskCreateDto dto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = GetUserId();
        var task = await service.CreateTaskAsync(dto, userId);

        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var userId = GetUserId();
        var task = await service.UpdateTaskAsync(id, dto, userId);
        
        if(task == false) return NotFound();
        
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await service.DeleteTaskAsync(id);
        if(task == false) return NotFound();

        return NoContent();
    }

}