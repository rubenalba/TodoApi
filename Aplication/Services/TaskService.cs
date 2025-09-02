using Aplication.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;

namespace Aplication.Services;

/// <summary>
///     Implementation of <see cref="ITaskRepository"/> that handles task-related operations.
///     Uses <see cref="IMapper"/> for data access and <see cref="ITaskService"/> for DTO mapping.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes a new instance of the <see cref="TaskService"/> class.
    /// </summary>
    /// <param name="repository">The task repository <see cref="ITaskRepository"/>.</param>
    /// <param name="mapper">The AutoMapper instance <see cref="IMapper"/>.</param>
    public TaskService(ITaskRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    /// <summary>
    ///     Retrieves all tasks for a specific user.
    /// </summary>
    /// <param name="id">The ID of the user.</param>
    /// <returns>A collection of <see cref="TaskReadDto"/>.</returns>
    public async Task<IEnumerable<TaskReadDto>> GetAllAsync(int id)
    {
        var tasks = await _repository.GetAllAsync(id);
        return _mapper.Map<IEnumerable<TaskReadDto>>(tasks);
    }

    /// <summary>
    ///     Retrieves a specific task by its ID for a given user.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns>
    ///     The <see cref="TaskReadDto"/> if found and belongs to the user, or <c>null</c> otherwise.
    /// </returns>
    public async Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task == null || task.UserId != userId) return null;
        var readDto = _mapper.Map<TaskReadDto>(task);
        return readDto;
    }

    /// <summary>
    ///     Creates a new task for a specific user.
    /// </summary>
    /// <param name="taskDto">The <see cref="TaskCreateDto"/> containing the task information.</param>
    /// <param name="userId">The ID of the user creating the task.</param>
    /// <returns>The created <see cref="TaskReadDto"/>.</returns>
    public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto taskDto, int userId)
    {
        var task = _mapper.Map<TaskItem>(taskDto);
        task.UserId = userId;
        await _repository.AddAsync(task);
        await _repository.SaveChangesAsync();
        
        return _mapper.Map<TaskReadDto>(task);
    }

    /// <summary>
    ///     Updates an existing task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task to update.</param>
    /// <param name="taskDto">The <see cref="TaskUpdateDto"/> containing updated task data.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns><c>true</c> if the update was successful; otherwise, <c>false</c>.</returns>
    public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto taskDto, int userId)
    {
        var task = await _repository.GetByIdAsync(id, userId);
        if (task == null || task.UserId != userId)
        {
            return await Task.FromResult(false);
        }
        _mapper.Map(taskDto, task);
        await _repository.UpdateAsync(task);
        await _repository.SaveChangesAsync();
        return true;
    }

    /// <summary>
    ///     Deletes a task by its ID for a specific user.
    /// </summary>
    /// <param name="id">The ID of the task to delete.</param>
    /// <param name="userId">The ID of the user who owns the task.</param>
    /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
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