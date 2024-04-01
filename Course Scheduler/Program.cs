using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Course_Scheduler.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Course_SchedulerContext>(options =>
    //options.UseSqlServer(
    //    builder.Configuration.GetConnectionString("Course_SchedulerContext"))
    options.UseSqlite(
        builder.Configuration.GetConnectionString("localSqlLiteDb"))
    );

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
