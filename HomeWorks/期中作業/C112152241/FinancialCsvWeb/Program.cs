using FinancialCsvWeb.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 加入 MVC
builder.Services.AddControllersWithViews();

// 加入資料庫
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 錯誤處理
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTPS
app.UseHttpsRedirection();

// 靜態檔案
app.UseStaticFiles();

// 路由
app.UseRouting();

app.UseAuthorization();

// 預設進入財報列表
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=FinancialReports}/{action=Index}/{id?}");

app.Run();