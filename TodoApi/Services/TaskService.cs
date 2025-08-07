using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Services;

public class TaskService : ITaskService
{
    private readonly TodoDbContext _context;
    private readonly IMapper _mapper;

    public TaskService(TodoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    
    public async Task<IEnumerable<TaskReadDto>> GetAllAsync(int id)
    {
        var tasks = await _context.Tasks.Where(t => t.UserId == id).ToListAsync();
        return _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
    }

    public async Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.UserId != userId) return null;
        var readDto = _mapper.Map<TaskReadDto>(task);
        return readDto;
    }

    public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto, int userId)
    {
        var task = _mapper.Map<TaskItem>(taskDto);
        task.UserId = userId;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<TaskReadDto>(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto, int userId)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null || task.UserId != userId) return await Task.FromResult(false);
        _mapper.Map(taskDto, task);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return await Task.FromResult(false);
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}