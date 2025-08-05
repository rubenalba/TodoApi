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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetAll()
    {
        var tasks = await _context.Tasks.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<TaskItem>>(tasks));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskItem>> Get(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if(task == null) return NotFound();
        return task;
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> Create(TaskItem dto)
    {
        var task = _mapper.Map<TaskItem>(dto);
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        var readDto = _mapper.Map<TaskReadDto>(task);
        return CreatedAtAction(nameof(Get), new { id = task.Id }, readDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskItem>> Update(int id, TaskItem task)
    {
        if(id != task.Id) return BadRequest();
        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<TaskItem>> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if(task == null) return NotFound();
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}