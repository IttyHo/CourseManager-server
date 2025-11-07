using Microsoft.EntityFrameworkCore;
using CourseManager.Api.Models;

namespace CourseManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Course> Courses => Set<Course>();
}
