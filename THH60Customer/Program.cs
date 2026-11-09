using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using THH60Customer.Identity;
using THH60Customer.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("Assignment3DB");

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "THH60Customer.Identity";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

var serviceBaseUrl = builder.Configuration["ServiceSettings:BaseUrl"]
    ?? "http://localhost:7777/";

if (!serviceBaseUrl.EndsWith('/'))
{
    serviceBaseUrl += "/";
}

builder.Services.AddHttpClient<CatalogService>(client =>
{
    client.BaseAddress = new Uri(serviceBaseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
