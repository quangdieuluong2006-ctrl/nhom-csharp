using BookStore.Application.Common;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Bắt mọi ngoại lệ chưa được xử lý (mục 4.5 – Thiết kế thích ứng):
/// - BusinessException (lỗi nghiệp vụ): hiện thông báo cho người dùng, không ghi log lỗi.
/// - Lỗi khác: ghi log đầy đủ ra logs/desktop-*.log kèm mã lỗi, chỉ hiện thông báo thân thiện và mã lỗi.
/// </summary>
public static class XuLyLoi
{
    public static void DangKy(ILogger logger)
    {
        System.Windows.Forms.Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        System.Windows.Forms.Application.ThreadException += (_, e) => XuLy(logger, e.Exception);
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            if (e.ExceptionObject is Exception ex) logger.LogCritical(ex, "Lỗi nghiêm trọng, ứng dụng phải dừng");
        };
    }

    public static void XuLy(ILogger logger, Exception ex)
    {
        if (ex is BusinessException be)
        {
            ThongBao.CanhBao(be.Message);
            return;
        }
        // Mã lỗi ngắn giúp đối chiếu thông báo người dùng gửi lên với dòng log tương ứng
        var maLoi = DateTime.Now.ToString("HHmmss") + "-" + Guid.NewGuid().ToString("N")[..4].ToUpperInvariant();
        logger.LogError(ex, "Lỗi không xử lý, mã lỗi {MaLoi}", maLoi);
        // Không hiện ex.Message: nội dung kỹ thuật (tên bảng, câu SQL...) không có ích cho người dùng và không nên lộ ra
        ThongBao.Loi($"Đã xảy ra lỗi ngoài ý muốn (mã lỗi: {maLoi}).\n" +
                     "Chi tiết đã được ghi vào file log. Vui lòng thử lại hoặc báo quản trị viên kèm mã lỗi.");
    }
}
