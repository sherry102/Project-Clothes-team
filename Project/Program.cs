
using Project.Controllers;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(); // ? 確保支援 JObject

builder.Services.AddMemoryCache(); // ? 註冊記憶體快取


// Add services to the DI container.
builder.Services.AddDbContext<DbuniPayContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();
// 在這裡替換原來的 AddSession() 配置
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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
app.MapHub<ChatHub>("/chatroom");
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FrontHome}/{action=FrontIndex}/{id?}");

app.Run();