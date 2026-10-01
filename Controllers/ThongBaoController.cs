using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongBaoController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public ThongBaoController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // 1. Lấy danh sách thông báo của một tài khoản (Dành cho App Mobile của khách)
        [HttpGet("customer/{maTaiKhoan}")]
        public async Task<IActionResult> GetNotificationsByAccount(int maTaiKhoan)
        {
            try
            {
                var list = await _context.ThongBaoNguoiNhans
                    .Where(n => n.MaTaiKhoan == maTaiKhoan)
                    .Include(n => n.MaThongBaoNavigation)
                    .OrderByDescending(n => n.MaThongBaoNavigation != null ? n.MaThongBaoNavigation.NgayTao : null)
                    .Select(n => new {
                        maThongBao = n.MaThongBao,
                        tieuDe = n.MaThongBaoNavigation != null ? n.MaThongBaoNavigation.TieuDe : "",
                        noiDung = n.MaThongBaoNavigation != null ? n.MaThongBaoNavigation.NoiDung : "",
                        loaiThongBao = n.MaThongBaoNavigation != null ? n.MaThongBaoNavigation.LoaiThongBao : "",
                        ngayTao = n.MaThongBaoNavigation != null ? n.MaThongBaoNavigation.NgayTao : null,
                        daDoc = n.DaDoc,
                        ngayDoc = n.NgayDoc
                    })
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 2. Đánh dấu thông báo là đã đọc (Dành cho App Mobile)
        [HttpPost("{maThongBao}/doc/{maTaiKhoan}")]
        public async Task<IActionResult> MarkAsRead(int maThongBao, int maTaiKhoan)
        {
            try
            {
                var item = await _context.ThongBaoNguoiNhans
                    .FirstOrDefaultAsync(n => n.MaThongBao == maThongBao && n.MaTaiKhoan == maTaiKhoan);

                if (item == null) return NotFound(new { success = false, message = "Không tìm thấy thông báo" });

                item.DaDoc = true;
                item.NgayDoc = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Đã đánh dấu đã đọc" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 3. Admin gửi thông báo mới (Dành cho Ứng dụng Desktop của Chủ trọ)
        [HttpPost("gui-thong-bao")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto model)
        {
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

                // Tạo thông báo chung
                var thongBao = new ThongBao
                {
                    TieuDe = model.TieuDe,
                    NoiDung = model.NoiDung,
                    LoaiThongBao = model.LoaiThongBao ?? "Chung",
                    NgayTao = DateTime.Now,
                    TrangThai = "Đã gửi"
                };

                _context.ThongBaos.Add(thongBao);
                await _context.SaveChangesAsync(); // Lưu để lấy MaThongBao tự tăng

                // Lấy danh sách tài khoản cần nhận thông báo
                List<int> danhSachTaiKhoanNhan = new List<int>();

                if (model.GuiChoTatCa)
                {
                    danhSachTaiKhoanNhan = await _context.TaiKhoans
                        .Where(t => t.TrangThai == "Active")
                        .Select(t => t.MaTaiKhoan)
                        .ToListAsync();
                }
                else if (model.DanhSachMaTaiKhoan != null && model.DanhSachMaTaiKhoan.Any())
                {
                    danhSachTaiKhoanNhan = model.DanhSachMaTaiKhoan;
                }

                // Thêm bản ghi vào bảng trung gian ThongBaoNguoiNhan
                foreach (var tkId in danhSachTaiKhoanNhan)
                {
                    _context.ThongBaoNguoiNhans.Add(new ThongBaoNguoiNhan
                    {
                        MaThongBao = thongBao.MaThongBao,
                        MaTaiKhoan = tkId,
                        DaDoc = false
                    });
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Gửi thông báo thành công", maThongBao = thongBao.MaThongBao });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }

    public class SendNotificationDto
    {
        public string TieuDe { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public string? LoaiThongBao { get; set; }
        public bool GuiChoTatCa { get; set; }
        public List<int>? DanhSachMaTaiKhoan { get; set; }
    }
}