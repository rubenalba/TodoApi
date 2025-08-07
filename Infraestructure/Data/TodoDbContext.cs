using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Data;

public class TodoDbContext : DbContext
{
    public TodoDbContext(DbContextOptions<TodoDbContext> options) :  base(options){}
    public DbSet<TaskItem> Tasks{ get; set; }
    public DbSet<User> Users{ get; set; }
}