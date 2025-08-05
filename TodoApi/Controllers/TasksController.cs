using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly IMapper _mapper;
    public TasksController(TodoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var userId = GetUserId();
        var tasks = await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();

        return Ok(_mapper.Map<IEnumerable<TaskItem>>(tasks));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> Get(int id)
    {
        var userId = GetUserId();
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if(task == null) return NotFound();

        return task;
    }
    [HttpPost]
    public async Task<ActionResult<TaskReadDto>> Create(TaskCreateDto dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        task.UserId = GetUserId();
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        var readDto = _mapper.Map<TaskReadDto>(task);

        return CreatedAtAction(nameof(Get), new { id = task.Id }, readDto);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, TaskItem task)
    {
        if (id != task.Id) return BadRequest();

        var userId = GetUserId();
        var existingTask = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (existingTask == null) return NotFound();

        existingTask.Title = task.Title;
        existingTask.IsCompleted = task.IsCompleted;

        await _context.SaveChangesAsync();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
        if (task == null) return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }

}