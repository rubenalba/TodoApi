using Domain.DTOs;

namespace Aplication.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDto>> GetAllAsync(int id);
    Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId);
    Task<TaskReadDto> CreateTaskAsync(TaskCreateDto task, int userId);
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto task,  int userId);
    Task<bool> DeleteTaskAsync(int i, int id);
}