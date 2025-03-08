using Microsoft.EntityFrameworkCore;
using WEB_MQDD_SHOESHOP.Models;

var builder = WebApplication.CreateBuilder(args);

// Đọc Connection String từ `appsettings.json`
var connectionString = builder.Configuration.GetConnectionString("ShoeShopDb");

// Đăng ký DbContext vào Dependency Injection
builder.Services.AddDbContext<ShoeShopDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:HostAddress"]); // http://localhost:5001
});
// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
