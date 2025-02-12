
using Project.Controllers;
using Microsoft.EntityFrameworkCore;
using Project.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the DI container.
builder.Services.AddDbContext<DbuniPayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Add services to the DI container.
builder.Services.AddDbContext<DbuniPayContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbuniPay"));
});
// WS 服務啟動
//builder.Services.AddSingleton<ChatController>();
//加入 SignalR
builder.Services.AddSignalR();
//允許跨域請求
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5500") // 指定允許的前端來源
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // 允許憑證
    });
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
//跨域啟動
app.UseCors();
//加入 Hub

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();