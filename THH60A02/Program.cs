using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using TH_H60_A02.Identity;
using TH_H60_A02.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("Assignment2DB");

var serviceBaseUrl = builder.Configuration["ServiceSettings:BaseUrl"]
    ?? "http://localhost:7777/";

if (!serviceBaseUrl.EndsWith('/'))
{
    serviceBaseUrl += "/";
}

builder.Services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddRazorPages();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
});

builder.Services.AddHttpClient<ProductService>(client =>
{
    client.BaseAddress = new Uri(serviceBaseUrl);
});

builder.Services.AddHttpClient<CategoryService>(client =>
{
    client.BaseAddress = new Uri(serviceBaseUrl);
});

builder.Services.AddHttpClient<CustomerService>(client =>
{
    client.BaseAddress = new Uri(serviceBaseUrl);
});

builder.Services.AddScoped<CustomerAccountService>();
builder.Services.AddScoped<CustomerManagementService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var identityDb = scope.ServiceProvider
        .GetRequiredService<ApplicationIdentityDbContext>();

    await identityDb.Database.MigrateAsync();

    await SeedData.SeedRoles(scope.ServiceProvider);

    await SeedData.SeedTestAccounts(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
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
