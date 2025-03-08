using Microsoft.EntityFrameworkCore;
using WEB_MQDD_SHOESHOP_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc Connection String từ `appsettings.json`
var connectionString = builder.Configuration.GetConnectionString("ShoeShopDb");

// Đăng ký DbContext vào Dependency Injection
builder.Services.AddDbContext<ShoeShopDbContext>(options =>
    options.UseSqlServer(connectionString));

// Thêm dịch vụ api
builder.Services.AddControllers();
var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//app.MapGet("/", () => "API is running!");
app.Run();
