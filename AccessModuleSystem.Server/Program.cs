using AccessModuleSystem.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.WebAssembly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure static files for Blazor WebAssembly
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Статические файлы для Blazor WebAssembly
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
  app.UseSwagger();
  app.UseSwaggerUI(options =>
  {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
    options.RoutePrefix = "swagger"; // Swagger доступен по /swagger
  });
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Для обслуживания статики

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();