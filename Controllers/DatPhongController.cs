using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatPhongController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public DatPhongController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // Lấy danh sách phiếu đặt phòng (kèm thông tin phòng và khách)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.DatPhongs
                    .Include(d => d.MaPhongNavigation)
                    .Include(d => d.MaKhachNavigation)
                    .Select(d => new {
                        maDatPhong = d.MaDatPhong,
                        maPhong = d.MaPhong,
                        tenPhong = d.MaPhongNavigation != null ? d.MaPhongNavigation.TenPhong : "",
                        maKhach = d.MaKhach,
                        tenKhach = d.MaKhachNavigation != null ? d.MaKhachNavigation.HoTen : "",
                        ngayDat = d.NgayDat,
                        ngayDuKienNhan = d.NgayDuKienNhan,
                        tienCoc = d.TienCoc,
                        trangThai = d.TrangThai,
                        ghiChu = d.GhiChu
                    })
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // Tạo mới phiếu đặt phòng (giữ chỗ) và tự động đổi trạng thái phòng sang 'Đã đặt cọc'
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DatPhong model)
        {
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

                model.TrangThai = "Đã đặt cọc";
                _context.DatPhongs.Add(model);

                // Cập nhật trạng thái của phòng tương ứng thành 'Đã đặt cọc'
                var phong = await _context.Phongs.FindAsync(model.MaPhong);
                if (phong != null)
                {
                    phong.TrangThai = "Đã đặt cọc";
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Đặt phòng/Giữ chỗ thành công", data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}