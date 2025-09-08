using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Add custom services
builder.Services.AddScoped<ICaseService, CaseService>();

// Configure Identity options
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
});

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        
        // Ensure database is created
        context.Database.EnsureCreated();
        
        // Seed roles
        if (!await roleManager.RoleExistsAsync("RegulationsAdmin"))
        {
            await roleManager.CreateAsync(new IdentityRole("RegulationsAdmin"));
        }
        if (!await roleManager.RoleExistsAsync("Planner"))
        {
            await roleManager.CreateAsync(new IdentityRole("Planner"));
        }
        if (!await roleManager.RoleExistsAsync("FinalUser"))
        {
            await roleManager.CreateAsync(new IdentityRole("FinalUser"));
        }
        
        // Seed admin user
        var adminUser = await userManager.FindByEmailAsync("admin@tracker.com");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin@tracker.com",
                Email = "admin@tracker.com",
                FirstName = "System",
                LastName = "Administrator",
                Department = "IT",
                Position = "System Administrator",
                EmailConfirmed = true
            };
            
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "RegulationsAdmin");
            }
        }
        
        // Seed sample data if none exists
        if (!context.Organizations.Any())
        {
            context.Organizations.AddRange(
                new Organization { Name = "Sample Organization 1", Code = "ORG001", Address = "123 Main St", ContactPerson = "John Doe", ContactEmail = "john@org1.com", ContactPhone = "+1234567890" },
                new Organization { Name = "Sample Organization 2", Code = "ORG002", Address = "456 Oak Ave", ContactPerson = "Jane Smith", ContactEmail = "jane@org2.com", ContactPhone = "+1234567891" }
            );
        }
        
        if (!context.Legislations.Any())
        {
            context.Legislations.AddRange(
                new Legislation { Name = "Regulation 2025-001", Code = "REG001", Description = "General Compliance Regulation", Year = 2025, EffectiveDate = new DateTime(2025, 1, 1) },
                new Legislation { Name = "Regulation 2025-002", Code = "REG002", Description = "Financial Compliance Regulation", Year = 2025, EffectiveDate = new DateTime(2025, 1, 1) }
            );
        }
        
        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
