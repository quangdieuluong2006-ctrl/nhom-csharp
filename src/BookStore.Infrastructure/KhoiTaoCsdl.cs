using BookStore.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure;

public static class KhoiTaoCsdl
{
    /// <summary>
    /// Tạo/cập nhật CSDL theo migration và nạp dữ liệu mẫu nếu CSDL còn trống.
    /// Web và Desktop đều gọi khi khởi động, nên chạy ứng dụng nào trước cũng được.
    ///
    /// Khi Web và Desktop khởi động CÙNG LÚC trên CSDL chưa có, cả hai cùng ra lệnh tạo CSDL/bảng:
    /// ứng dụng chậm hơn sẽ gặp lỗi "đã tồn tại". Vì vậy:
    ///  - Migration: nếu lỗi do ứng dụng kia đang tạo, chờ một chút rồi thử lại (lần sau không còn gì để làm).
    ///  - Dữ liệu mẫu: nạp trong giao tác có khóa ứng dụng (sp_getapplock) để chỉ MỘT ứng dụng nạp.
    /// </summary>
    public static async Task KhoiTaoCsdlAsync(this IServiceProvider services)
    {
        for (int lan = 1; ; lan++)
        {
            try
            {
                using var scope = services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<BookStoreDbContext>();
                await db.Database.MigrateAsync();
                await NapDuLieuMauAsync(db);
                return;
            }
            catch (SqlException ex) when (lan < 5 && LaLoiDoUngDungKhacDangKhoiTao(ex))
            {
                await Task.Delay(TimeSpan.FromSeconds(2 * lan));
            }
        }
    }

    /// <summary>
    /// 1801: CSDL đã tồn tại; 2714: bảng/đối tượng đã tồn tại; 1913: chỉ mục đã tồn tại;
    /// 2627/2601: trùng khóa (ví dụ cùng ghi lịch sử migration); 1205: deadlock.
    /// </summary>
    private static bool LaLoiDoUngDungKhacDangKhoiTao(SqlException ex) =>
        ex.Errors.Cast<SqlError>().Any(e => e.Number is 1801 or 2714 or 1913 or 2627 or 2601 or 1205);

    private static Task NapDuLieuMauAsync(BookStoreDbContext db) =>
        db.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            db.ChangeTracker.Clear();
            await using var tx = await db.Database.BeginTransactionAsync();
            // Khóa ứng dụng tự nhả khi giao tác kết thúc; ứng dụng thứ hai chờ ở đây rồi thấy CSDL đã có dữ liệu.
            await db.Database.ExecuteSqlRawAsync(
                "EXEC sp_getapplock @Resource = 'BookStore_DuLieuMau', @LockMode = 'Exclusive', " +
                "@LockOwner = 'Transaction', @LockTimeout = 60000");
            await DbSeeder.SeedAsync(db);
            await tx.CommitAsync();
        });
}
