using System.Security.Claims;
using Aplication.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

/// <summary>
///     Controller responsible for managing <see cref="TaskReadDto"/> entities.
///     Provides endpoints to retrieve, create, update, and delete tasks for authenticated users.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(ITaskService service) : ControllerBase
{
    /// <summary>
    ///     Retrieves the current authenticated user's ID from the JWT token.
    /// </summary>
    /// <returns>The user ID as <see cref="int"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the token does not contain a valid UserId.</exception>
    private int GetUserId()
    {
        var strId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(strId) || !int.TryParse(strId, out var id))
        {
            throw new InvalidOperationException("The token has not contain a valid UserId.");
        }
        return id;
    }

    /// <summary>
    ///     Retrieves all tasks for the authenticated user.
    /// </summary>
    /// <returns>An <see cref="OkObjectResult"/> containing a list of <see cref="TaskReadDto"/>.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskReadDto>>> GetAll()
    { 
        var userId = GetUserId();
        var task = await service.GetAllAsync(userId);
        return Ok(task);
    }

    /// <summary>
    ///     Retrieves a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task to retrieve.</param>
    /// <returns>
    ///     An <see cref="OkObjectResult"/> containing the <see cref="TaskReadDto"/> if found, 
    ///     or <see cref="NotFoundResult"/> if the task does not exist.
    /// </returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskReadDto>> Get(int id)
    {
        var userId = GetUserId();
        var task = await service.GetTaskByIdAsync(id, userId);
        if (task == null) return NotFound();
        return Ok(task);
    }

    /// <summary>
    ///     Creates a new task for the authenticated user.
    /// </summary>
    /// <param name="dto">The <see cref="TaskCreateDto"/> containing the task information.</param>
    /// <returns>
    ///     A <see cref="CreatedAtActionResult"/> pointing to the newly created task.
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create(TaskCreateDto dto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = GetUserId();
        var task = await service.CreateTaskAsync(dto, userId);

        return CreatedAtAction(nameof(Get), new { id = task.Id }, task);
    }

    /// <summary>
    ///     Updates an existing task by ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="dto">The <see cref="TaskUpdateDto"/> containing updated task data.</param>
    /// <returns>
    ///     <see cref="NoContentResult"/> if the update was successful, or <see cref="NotFoundResult"/> if the task was not found.
    /// </returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskUpdateDto dto)
    {
        var userId = GetUserId();
        var task = await service.UpdateTaskAsync(id, dto, userId);
        
        if(task == false) return NotFound();
        
        return NoContent();
    }

    /// <summary>
    ///     Deletes a task by ID for the authenticated user.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <returns>
    ///     <see cref="NoContentResult"/> if deletion was successful, or <see cref="NotFoundResult"/> if the task was not found.
    /// </returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var task = await service.DeleteTaskAsync(id, userId);
        if(task == false) return NotFound();

        return NoContent();
    }
}