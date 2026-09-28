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

        // 1. Lấy danh sách hóa đơn theo mã khách
        [HttpGet("customer/{maKhach}")]
        public async Task<IActionResult> GetInvoicesByCustomer(int maKhach)
        {
            try
            {
                var maPhongs = await _context.HopDongs
                    .Where(h => h.MaKhachDaiDien == maKhach && h.TrangThai == "Đang hiệu lực")
                    .Select(h => h.MaPhong)
                    .ToListAsync();

                var invoices = await _context.HoaDons
                    .Include(h => h.MaHopDongNavigation)
                    .Where(h => h.MaHopDongNavigation != null && maPhongs.Contains(h.MaHopDongNavigation.MaPhong))
                    .Select(h => new {
                        maHoaDon = h.MaHoaDon,
                        maHopDong = h.MaHopDong,
                        thang = h.Thang,
                        nam = h.Nam,
                        tongTien = h.TongTien,
                        trangThai = h.TrangThai,
                        hanThanhToan = h.HanThanhToan
                    })
                    .ToListAsync();

                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 2. Nhận kết quả từ Python OCR để tính tiền và tạo hóa đơn
        [HttpPost("tinh-tien-ocr")]
        public async Task<IActionResult> TinhTienOcr([FromBody] OcrDto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                // Tìm hợp đồng đang có hiệu lực của phòng (ví dụ phòng 1013)
                var hopDong = await _context.HopDongs
                    .FirstOrDefaultAsync(h => h.MaPhong == model.MaPhong && h.TrangThai == "Đang hiệu lực");

                if (hopDong == null)
                {
                    return BadRequest(new { success = false, message = $"Chưa tìm thấy hợp đồng đang hiệu lực cho phòng {model.MaPhong}." });
                }

                double tieuThu = model.ChiSoMoi - model.ChiSoCu;
                if (tieuThu < 0) tieuThu = 0;
                double tienDien = tieuThu * model.DonGiaDien;
                double tienPhong = (double)(hopDong.GiaThueThang ?? 0);
                double tongTien = tienPhong + tienDien;

                var hoaDon = new HoaDon
                {
                    MaHopDong = hopDong.MaHopDong,
                    Thang = model.Thang,
                    Nam = model.Nam,
                    NgayLap = DateOnly.FromDateTime(DateTime.Now),
                    HanThanhToan = DateOnly.FromDateTime(DateTime.Now.AddDays(7)),
                    TongTien = (decimal)tongTien,
                    DaThanhToan = 0, // Đã sửa lỗi chính tả ở đây
                    ConLai = (decimal)tongTien,
                    TrangThai = "Chưa thanh toán"
                };

                _context.HoaDons.Add(hoaDon);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    success = true,
                    message = "Tạo hóa đơn thành công!",
                    chiSoMoi = model.ChiSoMoi,
                    tieuThu = tieuThu,
                    tongTienHoaDon = tongTien
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }

    public class OcrDto
    {
        public int MaPhong { get; set; }
        public double ChiSoCu { get; set; }
        public double ChiSoMoi { get; set; }
        public double DonGiaDien { get; set; }
        public int Thang { get; set; }
        public int Nam { get; set; }
    }
}