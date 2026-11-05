using BookStore.Application.Services;
using BookStore.Desktop.Forms;
using BookStore.Desktop.HaTang;
using BookStore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BookStore.Desktop;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // ---- Cấu hình và DI (giống Program.cs của Web) ----
        var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
        {
            ContentRootPath = AppContext.BaseDirectory
        });
        // Cấu hình riêng của từng máy (chuỗi kết nối...), KHÔNG đưa lên Git – xem README
        builder.Configuration.AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.Local.json"), optional: true);

        builder.Services.AddSerilog(cfg => cfg
            .ReadFrom.Configuration(builder.Configuration)
            .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "desktop-.log"),
                rollingInterval: RollingInterval.Day, retainedFileCountLimit: 14));

        builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("BookStoreDB")
            ?? throw new InvalidOperationException("Thiếu chuỗi kết nối 'BookStoreDB' trong appsettings.json"));

        // Service nghiệp vụ dùng chung với Web (BookStore.Application)
        builder.Services.AddScoped<ITaiKhoanService, TaiKhoanService>();
        builder.Services.AddScoped<ISachService, SachService>();
        builder.Services.AddScoped<IDanhMucService, DanhMucService>();
        builder.Services.AddScoped<IDonHangService, DonHangService>();

        builder.Services.AddSingleton<PhienDangNhap>();
        builder.Services.AddSingleton<IFormFactory, FormFactory>();

        // Form: đăng ký Transient. THÀNH VIÊN THÊM FORM MỚI Ở ĐÂY, ví dụ:
        //   builder.Services.AddTransient<FrmQuanLySach>();
        builder.Services.AddTransient<FrmDangNhap>();
        builder.Services.AddTransient<FrmChinh>();

        using var host = builder.Build();
        var log = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("BookStore.Desktop");
        XuLyLoi.DangKy(log);

        // ---- Kết nối và khởi tạo CSDL ----
        try
        {
            host.Services.KhoiTaoCsdlAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            log.LogCritical(ex, "Không kết nối được CSDL");
            ThongBao.Loi("Không kết nối được CSDL BookStoreDB.\nHãy kiểm tra SQL Server và chuỗi kết nối trong appsettings.json.\n\n" + ex.Message);
            return;
        }

        // ---- Vòng đăng nhập: đăng xuất thì quay lại màn hình đăng nhập ----
        var forms = host.Services.GetRequiredService<IFormFactory>();
        var phien = host.Services.GetRequiredService<PhienDangNhap>();
        while (true)
        {
            using (var dangNhap = forms.Tao<FrmDangNhap>())
            {
                if (dangNhap.ShowDialog() != DialogResult.OK) break;
            }

            using var chinh = forms.Tao<FrmChinh>();
            System.Windows.Forms.Application.Run(chinh);
            if (!chinh.DaDangXuat) break;
            phien.DangXuat();
        }
        Log.CloseAndFlush();
    }
}
