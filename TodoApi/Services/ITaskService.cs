using TodoApi.DTO;
using TodoApi.Models;

namespace TodoApi.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskReadDto>> GetAllAsync(int id);
    Task<TaskReadDto?> GetTaskByIdAsync(int id, int userId);
    Task<TaskReadDto> CreateTaskAsync(TaskCreateDto task, int userId);
    Task<bool> UpdateTaskAsync(int id, TaskUpdateDto task,  int userId);
    Task<bool> DeleteTaskAsync(int id);
}