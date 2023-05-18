using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Bookstore.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bookstore.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BookstoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreContext") ?? throw new InvalidOperationException("Connection string 'BookstoreContext' not found.")));

/*builder.Services.AddDefaultIdentity<BookstoreUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BookstoreContext>()
    .AddDefaultTokenProviders();*/


builder.Services.AddIdentity<BookstoreUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<BookstoreContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.WriteIndented = true;
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
}).AddRazorPagesOptions(options =>
{
    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 100;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;

    options.ClaimsIdentity.RoleClaimType = "User";
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles")),
    RequestPath = "/UploadedFiles"
});


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();




