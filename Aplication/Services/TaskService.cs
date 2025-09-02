using Aplication.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Aplication.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    
    public async Task<IEnumerable<TaskReadDto>> GetAllAsync(int id)
    {
        var tasks = await _repository.GetAllAsync(id);
        return _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
    }

    public async Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task == null || task.UserId != userId) return null;
        var readDto = _mapper.Map<TaskReadDto>(task);
        return readDto;
    }

    public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto, int userId)
    {
        var task = _mapper.Map<TaskItem>(taskDto);
        task.UserId = userId;
        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();
        
        return _mapper.Map<TaskReadDto>(task);
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task == null || task.UserId != userId)
        {
            return await Task.FromResult(false);
        }
        _mapper.Map(taskDto, task);
        await _repository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteTaskAsync(int id, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task == null)
        {
            return await Task.FromResult(false);
        }
        await _repository.DeleteAsync(task);
        await _repository.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}