using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) :  base(options){}
    public DbSet<TaskItem> Tasks{ get; set; }
    public DbSet<User> Users{ get; set; }
}