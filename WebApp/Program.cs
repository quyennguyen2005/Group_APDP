using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Repositories;
using WebApp.Services.Auth;
using WebApp.Services.Classification;
using WebApp.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IStudentClassificationStrategy, GpaClassificationStrategy>();
builder.Services.AddScoped<IStudentClassificationStrategy, CreditMomentumStrategy>();
builder.Services.AddScoped<StudentClassificationService>();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

// Ensure database is created (only in Development)
if (app.Environment.IsDevelopment())
{
    try
    {
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            // Drop and recreate database to ensure schema matches models
            if (db.Database.CanConnect())
            {
                logger.LogInformation("Dropping existing database...");
                db.Database.EnsureDeleted();
            }
            
            logger.LogInformation("Creating database with current schema...");
            db.Database.EnsureCreated();
            logger.LogInformation("Database created successfully.");
        }
    }
    catch (Exception ex)
    {
        // Log error but don't stop the app
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while creating the database: {Message}", ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Index}/{id?}");

app.Run();
