using BursaFuarMerkezi.DataAccess.Data;
using BursaFuarMerkezi.DataAccess.DbInitializer;
using BursaFuarMerkezi.DataAccess.Repository;
using BursaFuarMerkezi.DataAccess.Repository.IRepository;
using BursaFuarMerkezi.Models;
using BursaFuarMerkezi.Utility;
// its for email templates don't delete
using BursaFuarMerkezi.web.Models.Configuration;
using BursaFuarMerkezi.web.Services;
using BursaFuarMerkezi.web.ViewEngines;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
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


// Add configuration
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));


// Add repository/services
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IFileHelper, FileHelper>();
builder.Services.AddScoped<IEmailSender, EmailSenderSmtp>();
builder.Services.AddHostedService<TempFileCleanupService>();

builder.Services.Configure<EmailTemplatesConfig>(
    builder.Configuration.GetSection(EmailTemplatesConfig.SectionName));


builder.Services.AddRazorPages()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/{2}/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/{2}/Shared/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
        options.ViewLocationExpanders.Add(new CustomViewLocationExpander());
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure 404 handling with language detection
app.UseStatusCodePages(async context =>
{
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;
    
    if (response.StatusCode == 404)
    {
        // Try to detect language from URL
        var path = request.Path.Value;
        var lang = "tr"; // default
        
        if (!string.IsNullOrEmpty(path))
        {
            var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
            if (segments.Length > 0 && (segments[0] == "en" || segments[0] == "tr"))
            {
                lang = segments[0];
            }
        }
        
        var notFoundPath = lang == "en" ? "/en/error-404" : "/tr/hata-404";
        response.Redirect(notFoundPath);
    }
});

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
    name: "localized",
    pattern: "{lang}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { lang = "en|tr" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();


void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            dbInitializer.Initialize();
        }
        catch (Exception ex)
        {
            app.Logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
