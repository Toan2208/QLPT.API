using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public HoaDonController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách toàn bộ hóa đơn
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.HoaDons
                    .Include(h => h.MaHopDongNavigation)
                        .ThenInclude(hd => hd.MaPhongNavigation)
                    .Include(h => h.MaHopDongNavigation)
                        .ThenInclude(hd => hd.MaKhachDaiDienNavigation)
                    .Include(h => h.ChiTietHoaDons)
                    .OrderByDescending(h => h.Nam).ThenByDescending(h => h.Thang)
                    .ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 2. Lấy hóa đơn theo Mã Hợp Đồng (Cho App Mobile của khách thuê)
        [HttpGet("hop-dong/{maHopDong}")]
        public async Task<IActionResult> GetByHopDong(int maHopDong)
        {
            try
            {
                var list = await _context.HoaDons
                    .Where(h => h.MaHopDong == maHopDong)
                    .Include(h => h.ChiTietHoaDons)
                    .OrderByDescending(h => h.Nam).ThenByDescending(h => h.Thang)
                    .ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 3. Xử lý số liệu từ WinForm (Điện có OCR + Nước nhập thủ công, tính tiền, chi tiết và chặn trùng)
        [HttpPost("tinh-tien-ocr")]
        public async Task<IActionResult> TinhTienOcr([FromBody] OcrRequestDto model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });

                // Kiểm tra hợp đồng đang hiệu lực của phòng
                var hopDong = await _context.HopDongs
                    .Include(h => h.MaPhongNavigation)
                    .FirstOrDefaultAsync(h => h.MaPhong == model.MaPhong && h.TrangThai == "Đang hiệu lực");

                if (hopDong == null)
                {
                    return BadRequest(new { success = false, message = $"Phòng {model.MaPhong} hiện không có hợp đồng đang hiệu lực." });
                }

                // Chặn tạo trùng hóa đơn trong cùng tháng/năm cho một hợp đồng
                var existingHoaDon = await _context.HoaDons
                    .FirstOrDefaultAsync(h => h.MaHopDong == hopDong.MaHopDong && h.Thang == model.Thang && h.Nam == model.Nam);

                if (existingHoaDon != null)
                {
                    return BadRequest(new { success = false, message = $"Hóa đơn tháng {model.Thang}/{model.Nam} của phòng này đã tồn tại rồi!" });
                }

                // Sử dụng đơn giá do WinForm truyền lên (nếu không có thì lấy mặc định)
                decimal donGiaDien = model.DonGiaDien > 0 ? model.DonGiaDien : 3500;
                decimal donGiaNuoc = model.DonGiaNuoc > 0 ? model.DonGiaNuoc : 10000;

                // Tính toán tiêu thụ điện nước
                decimal tieuThuDien = model.ChiSoDienMoi - model.ChiSoDienCu;
                if (tieuThuDien < 0) tieuThuDien = 0;
                decimal tienDien = tieuThuDien * donGiaDien;

                decimal tieuThuNuoc = model.ChiSoNuocMoi - model.ChiSoNuocCu;
                if (tieuThuNuoc < 0) tieuThuNuoc = 0;
                decimal tienNuoc = tieuThuNuoc * donGiaNuoc;

                // Ghi nhận Chỉ số Điện vào bảng ChiSoDienNuoc (Có lưu kết quả OCR và đường dẫn ảnh)
                var chiSoDien = new ChiSoDienNuoc
                {
                    MaPhong = model.MaPhong,
                    LoaiChiSo = "Điện",
                    Thang = model.Thang,
                    Nam = model.Nam,
                    ChiSoCu = model.ChiSoDienCu,
                    ChiSoMoi = model.ChiSoDienMoi,
                    ChiSoOcr = model.ChiSoDienOCR,
                    ChiSoXacNhan = model.ChiSoDienMoi,
                    DuongDanAnh = model.DuongDanAnhDien,
                    NgayGhi = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.ChiSoDienNuocs.Add(chiSoDien);

                // Ghi nhận Chỉ số Nước vào bảng ChiSoDienNuoc (Nhập thủ công không qua OCR)
                var chiSoNuoc = new ChiSoDienNuoc
                {
                    MaPhong = model.MaPhong,
                    LoaiChiSo = "Nước",
                    Thang = model.Thang,
                    Nam = model.Nam,
                    ChiSoCu = model.ChiSoNuocCu,
                    ChiSoMoi = model.ChiSoNuocMoi,
                    ChiSoOcr = null,
                    ChiSoXacNhan = model.ChiSoNuocMoi,
                    DuongDanAnh = null,
                    NgayGhi = DateOnly.FromDateTime(DateTime.Now)
                };
                _context.ChiSoDienNuocs.Add(chiSoNuoc);

                decimal tienPhong = hopDong.GiaThueThang ?? 0;
                var chiTietList = new List<ChiTietHoaDon>();

                // Thêm tiền phòng
                chiTietList.Add(new ChiTietHoaDon
                {
                    NoiDung = "Tiền phòng tháng " + model.Thang,
                    SoLuong = 1,
                    DonGia = tienPhong,
                    ThanhTien = tienPhong
                });

                // Thêm tiền điện
                chiTietList.Add(new ChiTietHoaDon
                {
                    NoiDung = $"Tiền điện ({tieuThuDien} kWh)",
                    SoLuong = tieuThuDien,
                    DonGia = donGiaDien,
                    ThanhTien = tienDien
                });

                // Thêm tiền nước
                chiTietList.Add(new ChiTietHoaDon
                {
                    NoiDung = $"Tiền nước ({tieuThuNuoc} m3)",
                    SoLuong = tieuThuNuoc,
                    DonGia = donGiaNuoc,
                    ThanhTien = tienNuoc
                });

                // Lấy các dịch vụ đã đăng ký theo hợp đồng
                var danhSachDangKyDv = await _context.DangKyDichVus
                    .Include(d => d.MaDichVuNavigation)
                    .Where(d => d.MaHopDong == hopDong.MaHopDong && d.TrangThai == "Đang sử dụng")
                    .ToListAsync();

                decimal tongTienDichVuKhac = 0;
                foreach (var itemDv in danhSachDangKyDv)
                {
                    decimal giaDv = itemDv.MaDichVuNavigation?.DonGia ?? itemDv.MaDichVuNavigation?.DonGia ?? 0;
                    tongTienDichVuKhac += giaDv;
                    chiTietList.Add(new ChiTietHoaDon
                    {
                        NoiDung = itemDv.MaDichVuNavigation?.TenDichVu ?? "Dịch vụ phát sinh",
                        SoLuong = 1,
                        DonGia = giaDv,
                        ThanhTien = giaDv
                    });
                }

                decimal tongTienHoaDon = tienPhong + tienDien + tienNuoc + tongTienDichVuKhac;

                // Tạo hóa đơn chính
                var hoaDon = new HoaDon
                {
                    MaHopDong = hopDong.MaHopDong,
                    Thang = (short)model.Thang,
                    Nam = model.Nam,
                    NgayLap = DateOnly.FromDateTime(DateTime.Now),
                    HanThanhToan = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                    TongTien = tongTienHoaDon,
                    DaThanhToan = 0,
                    ConLai = tongTienHoaDon,
                    TrangThai = "Chưa thanh toán"
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync(); // Lưu để phát sinh MaHoaDon tự tăng

                // Gắn mã hóa đơn cho các chi tiết và thêm vào DB
                foreach (var chiTiet in chiTietList)
                {
                    chiTiet.MaHoaDon = hoaDon.MaHoaDon;
                    _context.ChiTietHoaDons.Add(chiTiet);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new
                {
                    success = true,
                    message = "Lập hóa đơn, lưu chỉ số điện nước thành công!",
                    maHoaDon = hoaDon.MaHoaDon,
                    tongTien = tongTienHoaDon,
                    chiTiet = chiTietList
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }

    // DTO nhận dữ liệu từ WinForm (Điện có OCR riêng, Nước nhập thủ công)
    public class OcrRequestDto
    {
        public int MaPhong { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }

        // Điện (Hỗ trợ OCR)
        public decimal ChiSoDienCu { get; set; }
        public decimal ChiSoDienMoi { get; set; }
        public decimal ChiSoDienOCR { get; set; }
        public string? DuongDanAnhDien { get; set; }
        public decimal DonGiaDien { get; set; }

        // Nước (Nhập thủ công)
        public decimal ChiSoNuocCu { get; set; }
        public decimal ChiSoNuocMoi { get; set; }
        public decimal DonGiaNuoc { get; set; }
    }
}