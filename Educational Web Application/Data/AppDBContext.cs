using Microsoft.EntityFrameworkCore;
using EducationalWebApplication.Models;
using EducationalWebApplication.ViewModels;
namespace EducationalWebApplication.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Trainee> Trainees { get; set; }
        public DbSet<CourseResult> CourseResults { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDBContext(DbContextOptions options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "AI", ManagerName = "Mohamed"},
                new Department { Id = 2, Name = "Data Science", ManagerName = "Farida"}
            );
            modelBuilder.Entity<Course>().HasData(
                new Course
                {
                    Id = 1,
                    Name = "Machine Learning",
                    Degree = 100,
                    MinDegree = 40,
                    Credits = 4,
                    DepartmentID = 1
                },
                new Course
                {
                    Id = 2,
                    Name = "Deep Learning",
                    Degree = 100,
                    MinDegree = 50,
                    Credits = 3,
                    DepartmentID = 1
                },
                new Course
                {
                    Id = 3,
                    Name = "Linear Algebra",
                    Degree = 100,
                    MinDegree = 40,
                    Credits = 3,
                    DepartmentID = 2
                },
                new Course
                {
                    Id = 4,
                    Name = "Data Science Tools",
                    Degree = 100,
                    MinDegree = 50,
                    Credits = 4,
                    DepartmentID = 2
                }
            );
            modelBuilder.Entity<Instructor>().HasData(
                new Instructor { Id = 1, Name = "Ali", Address = "Alex", ImageURL = "1.jpg", Salary = 20000, DepartmentID = 1, CourseID = 1},    
                new Instructor { Id = 2, Name = "Ahmed", Address = "Cairo", ImageURL = "2.jpg", Salary = 25000, DepartmentID = 1, CourseID = 2},    
                new Instructor { Id = 3, Name = "Mahmoud", Address = "Alex", ImageURL = "3.jpg", Salary = 21000, DepartmentID = 2, CourseID = 3},    
                new Instructor { Id = 4, Name = "Aya", Address = "Alex", ImageURL = "4.jpg", Salary = 19000, DepartmentID = 2, CourseID = 4}
            );
            modelBuilder.Entity<Trainee>().HasData(
                new Trainee
                {
                    Id = 1,
                    Name = "Marina",
                    ImageURL = "11.jpg",
                    Address = "Alex",
                    Grade = 3,
                    DepartmentID = 1
                },
                new Trainee
                {
                    Id = 2,
                    Name = "Karim",
                    ImageURL = "22.jpg",
                    Address = "Alex",
                    Grade = 2,
                    DepartmentID = 1
                },
                new Trainee
                {
                    Id = 3,
                    Name = "Salma",
                    ImageURL = "33.jpg",
                    Address = "Alex",
                    Grade = 3,
                    DepartmentID = 1
                },
                new Trainee
                {
                    Id = 4,
                    Name = "Aysel",
                    ImageURL = "44.jpg",
                    Address = "Alex",
                    Grade = 4,
                    DepartmentID = 1
                },
                new Trainee
                {
                    Id = 5,
                    Name = "Seif",
                    ImageURL = "55.jpg",
                    Address = "Alex",
                    Grade = 3,
                    DepartmentID = 2
                },
                new Trainee
                {
                    Id = 6,
                    Name = "Amir",
                    ImageURL = "66.jpg",
                    Address = "Alex",
                    Grade = 1,
                    DepartmentID = 2
                },
                new Trainee
                {
                    Id = 7,
                    Name = "Mostafa",
                    ImageURL = "77.jpg",
                    Address = "Alex",
                    Grade = 5,
                    DepartmentID = 2
                },
                new Trainee
                {
                    Id = 8,
                    Name = "Fady",
                    ImageURL = "88.jpg",
                    Address = "Alex",
                    Grade = 2,
                    DepartmentID = 2
                }
            );
            modelBuilder.Entity<CourseResult>().HasData(
                new CourseResult { Id = 1, Score = 90, CourseID = 1, TraineeID = 1},    
                new CourseResult { Id = 2, Score = 95, CourseID = 1, TraineeID = 2},    
                new CourseResult { Id = 3, Score = 92, CourseID = 2, TraineeID = 3},    
                new CourseResult { Id = 4, Score = 100, CourseID = 2, TraineeID = 4},    
                new CourseResult { Id = 5, Score = 80, CourseID = 3, TraineeID = 5},    
                new CourseResult { Id = 6, Score = 86, CourseID = 3, TraineeID = 6},    
                new CourseResult { Id = 7, Score = 99, CourseID = 4, TraineeID = 7},    
                new CourseResult { Id = 8, Score = 100, CourseID = 4, TraineeID = 8}
            );
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "admin" },
                new User { Id = 2, Username = "Mahmoud", Password = "1234"}
            );
        }
        public DbSet<EducationalWebApplication.ViewModels.EnrollmentViewModel> EnrollmentViewModel { get; set; } = default!;
    }
}
