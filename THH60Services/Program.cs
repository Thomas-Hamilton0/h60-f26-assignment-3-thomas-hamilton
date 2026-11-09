using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using THH60Services.Interfaces;
using THH60Services.Models;
using THH60Services.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<H60AssignmentDbThContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("Assignment2DB")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<H60AssignmentDbThContext>();

    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.HeadContent =
            "<link rel=\"icon\" type=\"image/png\" href=\"/favicon.png\" />";
    });
}

app.UseAuthorization();

app.MapGet("/", () => Results.Redirect("/swagger"));

app.MapControllers();

app.Run();
