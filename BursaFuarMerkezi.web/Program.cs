using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.DbInitializer;
using BursaFuarMerkezi.DataAccess.Repository;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add configr

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Admin/Account/Login";
    //options.AccessDeniedPath = $"/Admin/Account/AccessDenied";
});


// Add repository services
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileHelper, FileHelper>();

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
//app.MapGet("/", () => Results.Redirect("/Admin/Home/Index"));
SeedDatabase();

app.MapControllerRoute(
    name: "AreaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.MapControllerRoute(
    name: "fuarTestSlug",
    pattern: "fuartest/{slug}",
    defaults: new { controller = "FuarTest", action = "Details" });

app.MapControllerRoute(
    name: "blogSlug",
    pattern: "blogs/{slug}",
    defaults: new { controller = "Blogs", action = "Details" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}