using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

public static class KhoiTaoCsdl
{
    /// <summary>
    /// Tạo/cập nhật CSDL theo migration và nạp dữ liệu mẫu nếu CSDL còn trống.
    /// Web và Desktop đều gọi khi khởi động, nên chạy ứng dụng nào trước cũng được.
    /// </summary>
    public static async Task KhoiTaoCsdlAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
        await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db);
    }
}
