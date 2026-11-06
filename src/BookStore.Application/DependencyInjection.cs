using BookStore.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Application;

public static class DependencyInjection
{
    /// <summary>Đăng ký toàn bộ service nghiệp vụ – Web và Desktop gọi cùng một hàm nên luôn dùng chung quy tắc.</summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ITaiKhoanService, TaiKhoanService>();
        services.AddScoped<ISachService, SachService>();
        services.AddScoped<IDanhMucService, DanhMucService>();
        services.AddScoped<IDonHangService, DonHangService>();
        services.AddScoped<IKhachHangService, KhachHangService>();
        services.AddScoped<IKhoService, KhoService>();
        services.AddScoped<IBaoCaoService, BaoCaoService>();
        return services;
    }
}
