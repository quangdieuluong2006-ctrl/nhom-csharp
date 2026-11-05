namespace BookStore.Domain.Enums;

/// <summary>Vai trò của tài khoản; lưu xuống CSDL dưới dạng chuỗi.</summary>
public enum VaiTro
{
    KhachHang,
    QuanTriVien,
    NhanVienBanHang,
    NhanVienKho
}

/// <summary>Các trạng thái của đơn hàng (xem sơ đồ trạng thái ở Chương 3).</summary>
public enum TrangThaiDonHang
{
    ChoXacNhan,
    DaXacNhan,
    DangGiao,
    HoanThanh,
    DaHuy
}

public enum TrangThaiThanhToan
{
    ChuaThanhToan,
    DaThanhToan
}

public enum LoaiPhieuKho
{
    Nhap,
    Xuat,
    DieuChinhTang,
    DieuChinhGiam
}
