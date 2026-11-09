using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

/// <summary>
/// Quản lý sách (D03): xem – Quản trị viên, NV bán hàng, NV kho; thêm/sửa/ngừng kinh doanh – chỉ Quản trị viên (US06).
/// </summary>
public partial interface ISachService
{
    Task<List<SachQtDto>> TimKiemQuanTriAsync(string? tuKhoa, int? maDanhMuc, bool? dangBan);
    Task<SachLuuDto?> LayDeSuaAsync(int maSach);
    Task<int> LuuAsync(int maNguoiThucHien, SachLuuDto dto);
    /// <summary>
    /// Sách chưa từng có giao dịch (đơn hàng, phiếu kho) thì xóa hẳn và trả về true;
    /// đã có giao dịch thì chỉ chuyển sang "Ngừng kinh doanh" (BR09) và trả về false.
    /// </summary>
    Task<bool> XoaHoacNgungBanAsync(int maNguoiThucHien, int maSach);
    /// <summary>Danh sách sách để chọn khi lập phiếu kho: "Tên sách (tồn: n)".</summary>
    Task<List<MucChonDto>> LaySachChonAsync();
}

public partial class SachService
{
    public async Task<List<SachQtDto>> TimKiemQuanTriAsync(string? tuKhoa, int? maDanhMuc, bool? dangBan)
    {
        var q = _db.Sachs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var k = tuKhoa.Trim();
            q = q.Where(s => s.TenSach.Contains(k) || s.ISBN == k || s.SachTacGias.Any(t => t.TacGia.TenTacGia.Contains(k)));
        }
        if (maDanhMuc is int dm) q = q.Where(s => s.MaDanhMuc == dm || s.DanhMuc.MaDanhMucCha == dm);
        if (dangBan is bool b) q = q.Where(s => s.TrangThai == b);

        return await q.OrderBy(s => s.TenSach).Select(s => new SachQtDto
        {
            MaSach = s.MaSach, ISBN = s.ISBN, TenSach = s.TenSach,
            TacGia = string.Join(", ", s.SachTacGias.Select(t => t.TacGia.TenTacGia)),
            DanhMuc = s.DanhMuc.TenDanhMuc,
            NhaXuatBan = s.NhaXuatBan != null ? s.NhaXuatBan.TenNhaXuatBan : null,
            NamXuatBan = s.NamXuatBan, DonGia = s.DonGia, SoLuongTon = s.SoLuongTon, TrangThai = s.TrangThai
        }).ToListAsync();
    }

    public Task<SachLuuDto?> LayDeSuaAsync(int maSach) =>
        _db.Sachs.AsNoTracking().Where(s => s.MaSach == maSach).Select(s => new SachLuuDto
        {
            MaSach = s.MaSach, ISBN = s.ISBN, TenSach = s.TenSach, MaDanhMuc = s.MaDanhMuc,
            MaNhaXuatBan = s.MaNhaXuatBan, NamXuatBan = s.NamXuatBan, DonGia = s.DonGia, MoTa = s.MoTa,
            HinhAnh = s.HinhAnh, TrangThai = s.TrangThai, SoLuongTon = s.SoLuongTon,
            MaTacGias = s.SachTacGias.Select(t => t.MaTacGia).ToList()
        }).FirstOrDefaultAsync();

    public async Task<int> LuuAsync(int maNguoiThucHien, SachLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);

        // ----- Kiểm tra dữ liệu (US06, BR02) -----
        var ten = KiemTra.BatBuoc(dto.TenSach, "Tên sách", 200);
        var isbn = KiemTra.TuyChon(dto.ISBN, "ISBN", 20)?.Replace("-", "").Replace(" ", "");
        if (isbn != null)
        {
            if (!isbn.All(char.IsDigit) || (isbn.Length != 10 && isbn.Length != 13))
                throw new BusinessException("ISBN phải gồm 10 hoặc 13 chữ số.");
            if (await _db.Sachs.AnyAsync(s => s.ISBN == isbn && s.MaSach != dto.MaSach))
                throw new BusinessException("ISBN đã tồn tại ở sách khác.");
        }
        if (dto.DonGia < 0) throw new BusinessException("Đơn giá không được âm.");
        if (dto.DonGia > 100_000_000) throw new BusinessException("Đơn giá vượt quá giới hạn cho phép.");
        if (dto.NamXuatBan is short nam && (nam < 1900 || nam > DateTime.Today.Year + 1))
            throw new BusinessException($"Năm xuất bản phải từ 1900 đến {DateTime.Today.Year + 1}.");
        if (!await _db.DanhMucs.AnyAsync(d => d.MaDanhMuc == dto.MaDanhMuc))
            throw new BusinessException("Vui lòng chọn danh mục.");
        if (dto.MaNhaXuatBan is int nxb && !await _db.NhaXuatBans.AnyAsync(n => n.MaNhaXuatBan == nxb))
            throw new BusinessException("Nhà xuất bản không tồn tại.");
        var maTacGias = dto.MaTacGias.Distinct().ToList();
        if (maTacGias.Count == 0) throw new BusinessException("Vui lòng chọn ít nhất một tác giả.");
        if (await _db.TacGias.CountAsync(t => maTacGias.Contains(t.MaTacGia)) != maTacGias.Count)
            throw new BusinessException("Có tác giả không tồn tại.");

        Sach s;
        if (dto.MaSach == 0)
        {
            // Sách mới có tồn = 0; tồn chỉ tăng khi lập phiếu nhập kho (BR06, UC-D07)
            s = new Sach { SoLuongTon = 0, NgayCapNhat = DateTime.Now };
            _db.Sachs.Add(s);
        }
        else
        {
            s = await _db.Sachs.Include(x => x.SachTacGias).FirstOrDefaultAsync(x => x.MaSach == dto.MaSach)
                ?? throw new BusinessException("Sách không tồn tại.");
        }

        s.TenSach = ten;
        s.ISBN = isbn;
        s.MaDanhMuc = dto.MaDanhMuc;
        s.MaNhaXuatBan = dto.MaNhaXuatBan;
        s.NamXuatBan = dto.NamXuatBan;
        s.DonGia = dto.DonGia;
        s.MoTa = KiemTra.TuyChon(dto.MoTa, "Mô tả", 4000);
        s.HinhAnh = KiemTra.TuyChon(dto.HinhAnh, "Đường dẫn ảnh", 500);
        s.TrangThai = dto.TrangThai;

        // Đồng bộ bảng nối SACHTACGIA: bỏ tác giả không còn chọn, thêm tác giả mới chọn
        foreach (var cu in s.SachTacGias.Where(t => !maTacGias.Contains(t.MaTacGia)).ToList())
            s.SachTacGias.Remove(cu);
        foreach (var ma in maTacGias.Where(m => s.SachTacGias.All(t => t.MaTacGia != m)))
            s.SachTacGias.Add(new SachTacGia { MaTacGia = ma, VaiTroTacGia = "Tác giả" });

        await _db.SaveChangesAsync();
        return s.MaSach;
    }

    public async Task<bool> XoaHoacNgungBanAsync(int maNguoiThucHien, int maSach)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var s = await _db.Sachs.Include(x => x.SachTacGias).FirstOrDefaultAsync(x => x.MaSach == maSach)
                ?? throw new BusinessException("Sách không tồn tại.");
        bool coGiaoDich = await _db.ChiTietDonHangs.AnyAsync(c => c.MaSach == maSach)
                          || await _db.ChiTietPhieuKhos.AnyAsync(c => c.MaSach == maSach);
        if (coGiaoDich)
            s.TrangThai = false;
        else
            _db.Sachs.Remove(s); // SACHTACGIA của sách bị xóa theo (cascade)
        await _db.SaveChangesAsync();
        return !coGiaoDich;
    }

    public async Task<List<MucChonDto>> LaySachChonAsync()
    {
        var ds = await _db.Sachs.AsNoTracking().OrderBy(s => s.TenSach)
            .Select(s => new { s.MaSach, s.TenSach, s.SoLuongTon, s.TrangThai }).ToListAsync();
        // Ghép chuỗi hiển thị trong bộ nhớ (sau ToListAsync) cho đơn giản, không phụ thuộc cách EF dịch sang SQL
        return ds.Select(s => new MucChonDto(s.MaSach,
            $"{s.TenSach} (tồn: {s.SoLuongTon}){(s.TrangThai ? "" : " – ngừng bán")}")).ToList();
    }
}
