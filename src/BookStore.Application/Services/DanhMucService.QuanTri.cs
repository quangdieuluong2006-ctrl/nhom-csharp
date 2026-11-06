using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

/// <summary>
/// Quản lý danh mục, nhà xuất bản, tác giả (D04 – chỉ Quản trị viên).
/// "Xóa" danh mục/tác giả đã có sách được thay bằng ngừng dùng để bảo toàn dữ liệu lịch sử (BR09).
/// </summary>
public partial interface IDanhMucService
{
    Task<List<DanhMucQtDto>> LayDanhSachDanhMucAsync(string? tuKhoa);
    /// <summary>Danh mục gốc (cấp 1) để chọn làm danh mục cha – hệ thống dùng cây 2 cấp.</summary>
    Task<List<MucChonDto>> LayDanhMucGocAsync(int boQuaMa = 0);
    /// <summary>Mọi danh mục đang dùng, dạng "Cha › Con", để chọn khi thêm sách.</summary>
    Task<List<MucChonDto>> LayDanhMucChonAsync();
    Task<int> LuuDanhMucAsync(int maNguoiThucHien, DanhMucLuuDto dto);
    /// <summary>Trả về true nếu đã xóa hẳn; false nếu danh mục đang được dùng nên chỉ chuyển sang "Ngừng dùng".</summary>
    Task<bool> XoaDanhMucAsync(int maNguoiThucHien, int maDanhMuc);

    Task<List<NhaXuatBanQtDto>> LayDanhSachNhaXuatBanAsync(string? tuKhoa);
    Task<List<MucChonDto>> LayNhaXuatBanChonAsync();
    Task<int> LuuNhaXuatBanAsync(int maNguoiThucHien, NhaXuatBanLuuDto dto);
    /// <summary>NXB không có trạng thái nên chỉ xóa được khi chưa có sách nào thuộc NXB.</summary>
    Task XoaNhaXuatBanAsync(int maNguoiThucHien, int maNhaXuatBan);

    Task<List<TacGiaQtDto>> LayDanhSachTacGiaAsync(string? tuKhoa);
    Task<List<MucChonDto>> LayTacGiaChonAsync();
    Task<int> LuuTacGiaAsync(int maNguoiThucHien, TacGiaLuuDto dto);
    Task<bool> XoaTacGiaAsync(int maNguoiThucHien, int maTacGia);
}

public partial class DanhMucService
{
    // ------------------------------- Danh mục -------------------------------

    public async Task<List<DanhMucQtDto>> LayDanhSachDanhMucAsync(string? tuKhoa)
    {
        var q = _db.DanhMucs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa)) q = q.Where(d => d.TenDanhMuc.Contains(tuKhoa.Trim()));
        var ds = await q.Select(d => new DanhMucQtDto
        {
            MaDanhMuc = d.MaDanhMuc, TenDanhMuc = d.TenDanhMuc, MaDanhMucCha = d.MaDanhMucCha,
            TenDanhMucCha = d.DanhMucCha != null ? d.DanhMucCha.TenDanhMuc : null,
            MoTa = d.MoTa, SoSach = d.Sachs.Count, TrangThai = d.TrangThai
        }).ToListAsync();
        // Sắp xếp kiểu cây: danh mục cha đứng trước, các con ngay bên dưới
        return ds.OrderBy(d => d.TenDanhMucCha ?? d.TenDanhMuc).ThenBy(d => d.MaDanhMucCha.HasValue).ThenBy(d => d.TenDanhMuc).ToList();
    }

    public Task<List<MucChonDto>> LayDanhMucGocAsync(int boQuaMa = 0) =>
        _db.DanhMucs.AsNoTracking().Where(d => d.MaDanhMucCha == null && d.MaDanhMuc != boQuaMa)
            .OrderBy(d => d.TenDanhMuc).Select(d => new MucChonDto(d.MaDanhMuc, d.TenDanhMuc)).ToListAsync();

    public async Task<List<MucChonDto>> LayDanhMucChonAsync()
    {
        var ds = await _db.DanhMucs.AsNoTracking().Where(d => d.TrangThai)
            .Select(d => new { d.MaDanhMuc, d.TenDanhMuc, Cha = d.DanhMucCha != null ? d.DanhMucCha.TenDanhMuc : null })
            .ToListAsync();
        return ds.Select(d => new MucChonDto(d.MaDanhMuc, d.Cha is null ? d.TenDanhMuc : $"{d.Cha} › {d.TenDanhMuc}"))
            .OrderBy(d => d.Ten).ToList();
    }

    public async Task<int> LuuDanhMucAsync(int maNguoiThucHien, DanhMucLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var ten = KiemTra.BatBuoc(dto.TenDanhMuc, "Tên danh mục", 100);
        if (await _db.DanhMucs.AnyAsync(d => d.TenDanhMuc == ten && d.MaDanhMuc != dto.MaDanhMuc))
            throw new BusinessException("Tên danh mục đã tồn tại.");

        if (dto.MaDanhMucCha is int cha)
        {
            if (cha == dto.MaDanhMuc) throw new BusinessException("Danh mục không thể là cha của chính nó.");
            var dmCha = await _db.DanhMucs.AsNoTracking().FirstOrDefaultAsync(d => d.MaDanhMuc == cha)
                        ?? throw new BusinessException("Danh mục cha không tồn tại.");
            if (dmCha.MaDanhMucCha != null) throw new BusinessException("Hệ thống chỉ hỗ trợ danh mục 2 cấp: danh mục cha phải là danh mục gốc.");
            if (dto.MaDanhMuc != 0 && await _db.DanhMucs.AnyAsync(d => d.MaDanhMucCha == dto.MaDanhMuc))
                throw new BusinessException("Danh mục đang có danh mục con nên không thể trở thành danh mục con.");
        }

        DanhMuc dm;
        if (dto.MaDanhMuc == 0) _db.DanhMucs.Add(dm = new DanhMuc());
        else dm = await _db.DanhMucs.FirstOrDefaultAsync(d => d.MaDanhMuc == dto.MaDanhMuc)
                  ?? throw new BusinessException("Danh mục không tồn tại.");
        dm.TenDanhMuc = ten;
        dm.MaDanhMucCha = dto.MaDanhMucCha;
        dm.MoTa = KiemTra.TuyChon(dto.MoTa, "Mô tả", 500);
        dm.TrangThai = dto.TrangThai;
        await _db.SaveChangesAsync();
        return dm.MaDanhMuc;
    }

    public async Task<bool> XoaDanhMucAsync(int maNguoiThucHien, int maDanhMuc)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var dm = await _db.DanhMucs.FirstOrDefaultAsync(d => d.MaDanhMuc == maDanhMuc)
                 ?? throw new BusinessException("Danh mục không tồn tại.");
        bool dangDung = await _db.Sachs.AnyAsync(s => s.MaDanhMuc == maDanhMuc)
                        || await _db.DanhMucs.AnyAsync(d => d.MaDanhMucCha == maDanhMuc);
        if (dangDung) dm.TrangThai = false; else _db.DanhMucs.Remove(dm);
        await _db.SaveChangesAsync();
        return !dangDung;
    }

    // ----------------------------- Nhà xuất bản -----------------------------

    public async Task<List<NhaXuatBanQtDto>> LayDanhSachNhaXuatBanAsync(string? tuKhoa)
    {
        var q = _db.NhaXuatBans.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa)) q = q.Where(n => n.TenNhaXuatBan.Contains(tuKhoa.Trim()));
        return await q.OrderBy(n => n.TenNhaXuatBan).Select(n => new NhaXuatBanQtDto
        {
            MaNhaXuatBan = n.MaNhaXuatBan, TenNhaXuatBan = n.TenNhaXuatBan, DiaChi = n.DiaChi,
            SoDienThoai = n.SoDienThoai, Email = n.Email, SoSach = n.Sachs.Count
        }).ToListAsync();
    }

    public Task<List<MucChonDto>> LayNhaXuatBanChonAsync() =>
        _db.NhaXuatBans.AsNoTracking().OrderBy(n => n.TenNhaXuatBan)
            .Select(n => new MucChonDto(n.MaNhaXuatBan, n.TenNhaXuatBan)).ToListAsync();

    public async Task<int> LuuNhaXuatBanAsync(int maNguoiThucHien, NhaXuatBanLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var ten = KiemTra.BatBuoc(dto.TenNhaXuatBan, "Tên nhà xuất bản", 150);
        if (await _db.NhaXuatBans.AnyAsync(n => n.TenNhaXuatBan == ten && n.MaNhaXuatBan != dto.MaNhaXuatBan))
            throw new BusinessException("Tên nhà xuất bản đã tồn tại.");

        NhaXuatBan nxb;
        if (dto.MaNhaXuatBan == 0) _db.NhaXuatBans.Add(nxb = new NhaXuatBan());
        else nxb = await _db.NhaXuatBans.FirstOrDefaultAsync(n => n.MaNhaXuatBan == dto.MaNhaXuatBan)
                   ?? throw new BusinessException("Nhà xuất bản không tồn tại.");
        nxb.TenNhaXuatBan = ten;
        nxb.DiaChi = KiemTra.TuyChon(dto.DiaChi, "Địa chỉ", 255);
        nxb.SoDienThoai = KiemTra.SoDienThoaiTuyChon(dto.SoDienThoai);
        nxb.Email = KiemTra.Email(dto.Email);
        await _db.SaveChangesAsync();
        return nxb.MaNhaXuatBan;
    }

    public async Task XoaNhaXuatBanAsync(int maNguoiThucHien, int maNhaXuatBan)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var nxb = await _db.NhaXuatBans.FirstOrDefaultAsync(n => n.MaNhaXuatBan == maNhaXuatBan)
                  ?? throw new BusinessException("Nhà xuất bản không tồn tại.");
        if (await _db.Sachs.AnyAsync(s => s.MaNhaXuatBan == maNhaXuatBan))
            throw new BusinessException("Nhà xuất bản đang có sách nên không thể xóa.");
        _db.NhaXuatBans.Remove(nxb);
        await _db.SaveChangesAsync();
    }

    // -------------------------------- Tác giả --------------------------------

    public async Task<List<TacGiaQtDto>> LayDanhSachTacGiaAsync(string? tuKhoa)
    {
        var q = _db.TacGias.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa)) q = q.Where(t => t.TenTacGia.Contains(tuKhoa.Trim()));
        return await q.OrderBy(t => t.TenTacGia).Select(t => new TacGiaQtDto
        {
            MaTacGia = t.MaTacGia, TenTacGia = t.TenTacGia, TieuSu = t.TieuSu,
            SoSach = t.SachTacGias.Count, TrangThai = t.TrangThai
        }).ToListAsync();
    }

    public Task<List<MucChonDto>> LayTacGiaChonAsync() =>
        _db.TacGias.AsNoTracking().Where(t => t.TrangThai).OrderBy(t => t.TenTacGia)
            .Select(t => new MucChonDto(t.MaTacGia, t.TenTacGia)).ToListAsync();

    public async Task<int> LuuTacGiaAsync(int maNguoiThucHien, TacGiaLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        TacGia tg;
        if (dto.MaTacGia == 0) _db.TacGias.Add(tg = new TacGia());
        else tg = await _db.TacGias.FirstOrDefaultAsync(t => t.MaTacGia == dto.MaTacGia)
                  ?? throw new BusinessException("Tác giả không tồn tại.");
        tg.TenTacGia = KiemTra.BatBuoc(dto.TenTacGia, "Tên tác giả", 150);
        tg.TieuSu = KiemTra.TuyChon(dto.TieuSu, "Tiểu sử", 1000);
        tg.TrangThai = dto.TrangThai;
        await _db.SaveChangesAsync();
        return tg.MaTacGia;
    }

    public async Task<bool> XoaTacGiaAsync(int maNguoiThucHien, int maTacGia)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var tg = await _db.TacGias.FirstOrDefaultAsync(t => t.MaTacGia == maTacGia)
                 ?? throw new BusinessException("Tác giả không tồn tại.");
        bool dangDung = await _db.SachTacGias.AnyAsync(s => s.MaTacGia == maTacGia);
        if (dangDung) tg.TrangThai = false; else _db.TacGias.Remove(tg);
        await _db.SaveChangesAsync();
        return !dangDung;
    }
}
