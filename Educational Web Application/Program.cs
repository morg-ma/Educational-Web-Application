using EducationalWebApplication.Data;
using EducationalWebApplication.Repository;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();

builder.Services.AddSession(cofg =>
{
    cofg.IdleTimeout = TimeSpan.FromMinutes(20);
});

// Types of Services: 
//      1- Framework Services: Already Declared, Already Registered
//      2- Built-in Services: Already Declared, Needs to Register
//      3- Custom Services: Needs to Declare, Needs to Register
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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ShowWebsite}/{id?}");

app.Run();
