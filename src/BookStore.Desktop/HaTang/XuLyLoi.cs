using BookStore.Application.Common;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Bắt mọi ngoại lệ chưa được xử lý (mục 4.5 – Thiết kế thích ứng):
/// - BusinessException (lỗi nghiệp vụ): hiện thông báo cho người dùng, không ghi log lỗi.
/// - Lỗi khác: ghi log đầy đủ ra logs/desktop-*.log, chỉ hiện thông báo thân thiện.
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
        logger.LogError(ex, "Lỗi không xử lý");
        ThongBao.Loi("Đã xảy ra lỗi ngoài ý muốn. Chi tiết đã được ghi vào file log.\n\n" + ex.Message);
    }
}
