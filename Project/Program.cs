
using Project.Controllers;
using Microsoft.EntityFrameworkCore;
using Project.Models;
using Project.Hubs;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson(); // ? �T�O�䴩 JObject

builder.Services.AddMemoryCache(); // ? ���U�O����֨�

// ���U IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// �K�[�򥻪���x�A�ȡA�u��X�챱��x�M��������
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});
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
// WS �A�ȱҰ�
//builder.Services.AddSingleton<ChatController>();
//�[�J SignalR
builder.Services.AddSignalR();
//���\���ШD
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://127.0.0.1:5500") // ���w���\���e�ݨӷ�
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // ���\����
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
//���Ұ�
app.UseCors();
//�[�J Hub
app.MapHub<ChatHub>("/chatroom");
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FrontHome}/{action=FrontIndex}/{id?}");

app.Run();