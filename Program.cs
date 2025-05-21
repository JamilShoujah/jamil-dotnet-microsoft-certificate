using System;
using System.IO;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.OpenApi.Models;
using JamilDotnetMicrosoftCertificate.Data;
using JamilDotnetMicrosoftCertificate.Middleware;
using JamilDotnetMicrosoftCertificate.Models;

var builder = WebApplication.CreateBuilder(args);

// ===== Services =====
builder.Services.AddControllersWithViews();

// EF Core In-Memory DB
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseInMemoryDatabase("UsersDb"));

// Authentication (demo stub)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        // configure real token validation here
    });

// Data Protection — persist to ./keys/
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(
        Path.Combine(builder.Environment.ContentRootPath, "keys")))
    .SetApplicationName("UserManagementAPI");

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });
});

var app = builder.Build();

// ===== Middleware Pipeline =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// 1) Global error handler
app.UseMiddleware<ErrorHandlingMiddleware>();
// 2) Authentication
// app.UseMiddleware<AuthenticationMiddleware>();
// 3) Logging
app.UseMiddleware<LoggingMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// THIS IS THE KEY ADDITION FOR API CONTROLLERS:
app.MapControllers();

// MVC routes (optional, if you serve MVC views)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Swagger UI (dev only)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User API V1");
    });
}

// ===== Seed Data =====
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Users.Any())
    {
        var faker = new Faker<User>()
            .RuleFor(u => u.Id, f => Guid.NewGuid())
            .RuleFor(u => u.Name, f => f.Name.FullName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Age, f => f.Random.Int(18, 100))
            .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Role, f => "User"); 

        var users = faker.Generate(10);
        context.Users.AddRange(users);
        context.SaveChanges();
    }
}

app.Run();
