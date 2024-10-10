using EducationalWebApplication.Data;
using EducationalWebApplication.Models;
using EducationalWebApplication.Repository;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Types of Services: 
//      1- Framework Services: Already Declared, Already Registered
//      2- Built-in Services: Already Declared, Needs to Register
//      3- Custom Services: Needs to Declare, Needs to Register

// Add services to the container.
builder.Services.AddControllersWithViews();

// Lifetime Service: Scoped, Singleton, Transient
// 1- Singleton: Create one Object for all application
// 2- Transient: Create Object in every time I use the Repository
// 3- Scoped (Most Used): Create one Object per Request

builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

// Register Identity Services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Custom the Password Validations
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

}).AddEntityFrameworkStores<AppDBContext>(); // Make the program use my Context not the identity context

builder.Services.AddSession(cofg =>
{
    cofg.IdleTimeout = TimeSpan.FromMinutes(15);
});

builder.Services.AddSqlServer<AppDBContext>(
    builder.Configuration.GetConnectionString("DefaultConnection")
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowWebsite}/{id?}");

app.Run();
