using System.Security.Claims;
using Aplication.Interfaces;
using Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApi.Controllers;


namespace Test.Controllers;

public class TaskControllerTest
{
    private readonly Mock<ITaskService> _service;
    private readonly TasksController _controller;
    
    public TaskControllerTest()
    {
        _service = new Mock<ITaskService>();
        _controller = new TasksController(_service.Object);
        // Simula un usuario con ID 1
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, "1")
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfTaskReadDto()
    {
        // Arrange
        var tasks = new List<TaskReadDto> {
            new TaskReadDto { Id = 1, Title = "Tarea 1" },
            new TaskReadDto { Id = 2, Title = "Tarea 2" }
        };
        _service.Setup(s => s.GetAllAsync(1)).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTasks = Assert.IsAssignableFrom<IEnumerable<TaskReadDto>>(okResult.Value);
        Assert.Equal(2, returnedTasks.Count());
    }
    
    [Fact]
    public async Task Get_ReturnsNotFound_WhenTaskDoesNotExist()
    {
        // Arrange
        _service.Setup(s => s.GetTaskByIdAsync(42, 1)).ReturnsAsync((TaskReadDto)null);

        // Act
        var result = await _controller.Get(42);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Get_ReturnsOk_WithTaskReadDto()
    {
        // Arrange
        var dto = new TaskReadDto { Id = 5, Title = "Test", IsCompleted = false, CreatedAt = System.DateTime.UtcNow };
        _service.Setup(s => s.GetTaskByIdAsync(5, 1)).ReturnsAsync(dto);

        // Act
        var actionResult = await _controller.Get(5);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returned = Assert.IsType<TaskReadDto>(okResult.Value);
        Assert.Equal(5, returned.Id);
        Assert.Equal("Test", returned.Title);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
    {
        //Arrange
        var dto = new TaskCreateDto{Title = "A"};
        _controller.ModelState.AddModelError("Title", "Title too short");
        
        //Act
        var result = await _controller.Create(dto);
        
        //Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        var modelState = Assert.IsType<SerializableError>(badRequest.Value);
        Assert.True(modelState.ContainsKey("Title"));
    }

    [Fact]
    public async Task Create_ShallCreateTask()
    {
        // Arrange
        var inDto = new TaskCreateDto { Title = "Nueva Tarea" };
        var outDto = new TaskReadDto { Id = 10, Title = "Nueva Tarea", IsCompleted = false, CreatedAt = System.DateTime.UtcNow };

        _service.Setup(s => s.CreateTaskAsync(inDto, 1)).ReturnsAsync(outDto);

        // Act
        var actionResult = await _controller.Create(inDto);

        // Assert
        var created = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
        Assert.Equal(nameof(_controller.Get), created.ActionName);

        var returned = Assert.IsType<TaskReadDto>(created.Value);
        Assert.Equal(10, returned.Id);
        Assert.Equal("Nueva Tarea", returned.Title);
    }

    [Fact]
    public async Task Put_ReturnNotFound_WhenTaskDoesNotExist()
    {
        //Arrange
        var task = new TaskUpdateDto{Title = "Tarea 1"};
        _service.Setup(s => s.UpdateTaskAsync(1, task, 1)).ReturnsAsync(false);
        
        //Act
        var result = await _controller.Update(1, task);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Put_UpdateTask_OnCall()
    {
        //Arrange
        var task = new TaskUpdateDto{Title = "Tarea 1"};
        _service.Setup(s => s.UpdateTaskAsync(1, task, 1)).ReturnsAsync(true);
        
        //Act
        var result = await _controller.Update(1, task);
        
        //Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnNotFound_WhenTaskDoesNotExist()
    {
        //Arrange
        var task = new TaskReadDto { Id = 1, Title = "Test", IsCompleted = false, CreatedAt = System.DateTime.UtcNow };
        _service.Setup(s => s.DeleteTaskAsync(1, 1)).ReturnsAsync(false);
        
        //Act
        var result = await _controller.Delete(1);
        
        //Assert
        Assert.IsType<NotFoundResult>(result);
    }
    
    [Fact]
    public async Task Delete_ReturnNoContent_WhenDeleteTask()
    {
        //Arrange
        var task = new TaskReadDto { Id = 1, Title = "Test", IsCompleted = false, CreatedAt = System.DateTime.UtcNow };
        _service.Setup(s => s.DeleteTaskAsync(1, 1)).ReturnsAsync(true);
        
        //Act
        var result = await _controller.Delete(1);
        
        //Assert
        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task GetAll_ThrowsInvalidOperationException_WhenUserIdClaimMissing()
    {
        //Arrange
        var userWithoutClaim = new ClaimsPrincipal(new ClaimsIdentity()); // sin claims
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = userWithoutClaim }
        };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _controller.GetAll());
    }

}