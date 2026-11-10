using BookStore.Application;
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

        // Định dạng số, tiền, ngày theo tiếng Việt (320.000 đ, 12/10/2026) dù Windows cài tiếng Anh
        var vi = new System.Globalization.CultureInfo("vi-VN");
        System.Globalization.CultureInfo.DefaultThreadCurrentCulture = vi;
        System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = vi;
        System.Threading.Thread.CurrentThread.CurrentCulture = vi;
        System.Threading.Thread.CurrentThread.CurrentUICulture = vi;

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
        builder.Services.AddApplication();

        builder.Services.AddSingleton<PhienDangNhap>();
        builder.Services.AddSingleton<IFormFactory, FormFactory>();
        builder.Services.AddSingleton<AnhSach>();

        // Form: đăng ký Transient – mỗi lần mở là một đối tượng mới, nhận service qua constructor.
        // Thêm form mới: đăng ký ở đây rồi gắn vào menu trong FrmChinh (xem docs/HUONG_DAN_DESKTOP.md).
        builder.Services.AddTransient<FrmDangNhap>();
        builder.Services.AddTransient<FrmChinh>();
        builder.Services.AddTransient<FrmTongQuan>();
        builder.Services.AddTransient<FrmDoiMatKhau>();
        builder.Services.AddTransient<FrmSach>();
        builder.Services.AddTransient<FrmDanhMuc>();
        builder.Services.AddTransient<FrmNhaXuatBan>();
        builder.Services.AddTransient<FrmTacGia>();
        builder.Services.AddTransient<FrmDonHang>();
        builder.Services.AddTransient<FrmKhachHang>();
        builder.Services.AddTransient<FrmPhieuKho>();
        builder.Services.AddTransient<FrmKho>();
        builder.Services.AddTransient<FrmBaoCao>();
        builder.Services.AddTransient<FrmTaiKhoan>();

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
