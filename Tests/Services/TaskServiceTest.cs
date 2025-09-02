using Aplication.Interfaces;
using Aplication.Services;
using AutoMapper;
using Domain.DTOs;
using Domain.Models;
using Moq;

namespace Test.Services;

public class TaskServiceTest
{
    private readonly Mock<ITaskRepository> _repository;
    private readonly Mock<IMapper> _mapper;
    private readonly TaskService _service;

    public TaskServiceTest()
    {
        _repository = new Mock<ITaskRepository>();
        _mapper = new Mock<IMapper>();
        _service = new TaskService(_repository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetAll_ShallReturnAllTasks()
    {
        // Arrange
        var taskItems = new List<TaskItem>
        {
            new TaskItem { Id = 1, Title = "Task 1" },
            new TaskItem { Id = 2, Title = "Task 2" }
        };

        var taskDtos = new List<TaskReadDto>
        {
            new TaskReadDto { Id = 1, Title = "Task 1" },
            new TaskReadDto { Id = 2, Title = "Task 2" }
        };

        _repository.Setup(r => r.GetAllAsync(1)).ReturnsAsync(taskItems);

        _mapper.Setup(m => m.Map<IEnumerable<TaskReadDto>>(taskItems))
            .Returns(taskDtos);

        // Act
        var result = await _service.GetAllAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal("Task 1", result.First().Title);

        _repository.Verify(r => r.GetAllAsync(1), Times.Once);
        _mapper.Verify(m => m.Map<IEnumerable<TaskReadDto>>(taskItems), Times.Once);
    }

    [Fact]
    public async Task GetTaskById_ShallReturnTaskById()
    {
        //Arrange
        var taskItem = new TaskItem { Id = 1, Title = "Task 1",IsCompleted = false, CreatedAt = DateTime.UtcNow, UserId = 1};
        var taskDto = new TaskReadDto { Id = 1, Title = "Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow};
        const int userId = 1;
        _repository.Setup(r => r.GetByIdAsync(taskItem.Id, userId)).ReturnsAsync(taskItem);
        _mapper.Setup(m => m.Map<TaskReadDto>(taskItem)).Returns(taskDto);
        
        //Act
        var result = await _service.GetTaskByIdAsync(taskItem.Id, userId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(taskDto.Id, result.Id);
        Assert.Equal(taskDto.Title, result.Title);
        
        _repository.Verify(r => r.GetByIdAsync(taskItem.Id, userId), Times.Once);
        _mapper.Verify(m => m.Map<TaskReadDto>(taskItem), Times.Once);
    }

    [Fact]
    public async Task GetTaskById_ShallReturnNull_WhenTaskIsNull()
    {
        //Arrange
        var taskItem = new TaskItem { Id = 1, Title = "Task 1",IsCompleted = false, CreatedAt = DateTime.UtcNow};
        _repository.Setup(r => r.GetByIdAsync(taskItem.Id, 4)).ReturnsAsync(taskItem);
        
        //Act
        var result = await _service.GetTaskByIdAsync(taskItem.Id, 1);
        
        //Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetTaskById_ShallReturnNull_WhenUserIdIsDistinct()
    {
        //Arrange
        var taskItem = new TaskItem { Id = 1, Title = "Task 1",IsCompleted = false, CreatedAt = DateTime.UtcNow, UserId = 1};
        var taskDto = new TaskReadDto { Id = 1, Title = "Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow};
        const int userId = 1;
        _repository.Setup(r => r.GetByIdAsync(taskItem.Id, 2)).ReturnsAsync(taskItem);
        _mapper.Setup(m => m.Map<TaskReadDto>(taskItem)).Returns(taskDto);
        
        //Act
        var result = await _service.GetTaskByIdAsync(taskItem.Id, 2);
        
        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateTaskAsync_ShallCreateTask()
    {
        //Arrange
        var taskDto = new TaskCreateDto { Title = "Task 1"};
        var taskItem = new TaskItem { Id = 1, Title = "Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow,  UserId = 1};
        var taskReadDto = new TaskReadDto { Id = 1, Title = "Task 1", IsCompleted = false, CreatedAt = taskItem.CreatedAt};
        const int userId = 1;
        
        _mapper.Setup(m=> m.Map<TaskItem>(taskDto)).Returns(taskItem);
        _repository.Setup(r => r.AddAsync(taskItem));
        _mapper.Setup(m => m.Map<TaskReadDto>(taskItem)).Returns(taskReadDto);
        
        //Act
        var result = await _service.CreateTaskAsync(taskDto, userId);
        
        //Assert
        Assert.NotNull(result);
        Assert.Equal(taskReadDto.Id, result.Id);
        Assert.Equal(taskDto.Title, result.Title);
        Assert.Equal(taskReadDto.IsCompleted, result.IsCompleted);
        
        _mapper.Verify(m=> m.Map<TaskItem>(taskDto), Times.Once);
        _repository.Verify(r => r.AddAsync(taskItem), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
        _mapper.Verify(m => m.Map<TaskReadDto>(taskItem), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShallUpdateTask()
    {
        //Arrange
        const int id = 1;
        const int userId = 1;
        var taskItem = new  TaskItem { Id = id, Title = "Task 1", IsCompleted = false, CreatedAt = DateTime.UtcNow, UserId = userId};
        var taskUpdateDto = new TaskUpdateDto { Title = "Task updated" };
        _repository.Setup(r => r.GetByIdAsync(id, userId)).ReturnsAsync(taskItem);
        
        //Act
        var result = await _service.UpdateTaskAsync(id, taskUpdateDto, userId);
       
        //Assert
        Assert.True(result);
        _repository.Verify(r => r.GetByIdAsync(id, userId), Times.Once);
        _mapper.Verify(m => m.Map(taskUpdateDto, taskItem), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShallReturnFalseIfTaskNull()
    { 
        //Arrange
        const int taskId = 1;
        const int userId = 1;
        var updateDto = new TaskUpdateDto { Title = "Updated title" };

        _repository
            .Setup(r => r.GetByIdAsync(taskId, userId))
            .ReturnsAsync((TaskItem?)null);
        
        //Act
        var result = await _service.UpdateTaskAsync(taskId, updateDto, userId);
        
        //Assert
        Assert.False(result);
        _repository.Verify(r => r.GetByIdAsync(taskId, userId), Times.Once);
        _mapper.Verify(m => m.Map(updateDto, It.IsAny<TaskItem>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
    
    [Fact]
    public async Task UpdateTaskAsync_WhenUserIdDoesNotMatch_ReturnsFalse()
    {
        // Arrange
        const int taskId = 1;
        const int userId = 1;
        var updateDto = new TaskUpdateDto { Title = "Updated title" };
    
        var taskItem = new TaskItem { Id = taskId, UserId = 999 };

        _repository
            .Setup(r => r.GetByIdAsync(taskId, userId))
            .ReturnsAsync(taskItem);

        // Act
        var result = await _service.UpdateTaskAsync(taskId, updateDto, userId);

        // Assert
        Assert.False(result);
        _mapper.Verify(m => m.Map(It.IsAny<TaskUpdateDto>(), It.IsAny<TaskItem>()), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteTaskAsync_ShallDeleteTask()
    {
        //Arrange
        const int id = 1;
        const int userId = 1;
        var  taskItem = new TaskItem { Id = id, Title = "Task 1" };
        _repository.Setup(r => r.GetByIdAsync(id, userId)).ReturnsAsync(taskItem);
        
        //Act
        var result = await _service.DeleteTaskAsync(id, userId);
        
        //Assert
        Assert.True(result);
        _repository.Verify(r => r.GetByIdAsync(id, userId), Times.Once);
        _repository.Verify(r => r.DeleteAsync(taskItem), Times.Once);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Once);
    }
    
    [Fact]
    public async Task DeleteTaskAsync_ShallReturnFalseIfTaskNull()
    {
        //Arrange
        const int id = 1;
        const int userId = 1;
        var  taskItem = new TaskItem { Id = id, Title = "Task 1" };
        _repository.Setup(r => r.GetByIdAsync(id, userId)).ReturnsAsync((TaskItem?)null);
        
        //Act
        var result = await _service.DeleteTaskAsync(id, userId);
        
        //Assert
        Assert.False(result);
        _repository.Verify(r => r.GetByIdAsync(id, userId), Times.Once);
        _repository.Verify(r => r.DeleteAsync(taskItem), Times.Never);
        _repository.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    
}