using System.Globalization;
using BookStore.Application.Services;
using BookStore.Infrastructure;
using BookStore.Infrastructure.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Ghi log ra console và file logs/web-<ngày>.log (mục 4.5 – Thiết kế thích ứng)
builder.Host.UseSerilog((ctx, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .WriteTo.Console()
    .WriteTo.File("logs/web-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14));

// Tầng hạ tầng (EF Core + SQL Server) và tầng nghiệp vụ
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("BookStoreDB")
    ?? throw new InvalidOperationException("Thiếu chuỗi kết nối 'BookStoreDB' trong appsettings.json"));
builder.Services.AddScoped<ISachService, SachService>();
builder.Services.AddScoped<IDanhMucService, DanhMucService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Tạo/cập nhật CSDL theo migration và nạp dữ liệu mẫu khi chạy lần đầu
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
    await db.Database.MigrateAsync();
    await DbSeeder.SeedAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Loi");
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/Loi/{0}");

// Định dạng số, tiền tệ, ngày theo tiếng Việt (45.000 đ, 05/11/2026)
var vi = new CultureInfo("vi-VN");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(vi),
    SupportedCultures = new[] { vi },
    SupportedUICultures = new[] { vi }
});

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
