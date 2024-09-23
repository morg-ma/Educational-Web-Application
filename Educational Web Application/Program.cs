using EducationalWebApplication.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(cofg =>
{
    cofg.IdleTimeout = TimeSpan.FromMinutes(20);
});

builder.Services.AddSqlServer<AppDBContext>(
    builder.Configuration
    .GetConnectionString("DefaultConnection"));

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
