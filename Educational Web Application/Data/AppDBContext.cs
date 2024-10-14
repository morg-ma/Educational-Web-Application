using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace EducationalWebApplication.Data
{
    public class AppDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<CourseResult> CourseResults { get; set; }
        public DbSet<Department> Departments { get; set; }

        public AppDBContext(DbContextOptions options) : base(options) {}
    }
}
