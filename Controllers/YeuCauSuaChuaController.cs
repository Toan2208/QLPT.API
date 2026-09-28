using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    public class YeuCauSuaChuaDto
    {
        public int MaPhong { get; set; }
        public int MaKhach { get; set; }
        public string? NoiDung { get; set; }
        public string? TrangThai { get; set; }
        public DateTime? NgayBao { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class YeuCauSuaChuaController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public YeuCauSuaChuaController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // 1. API lấy danh sách yêu cầu sửa chữa theo mã khách
        [HttpGet("customer/{maKhach}")]
        public async Task<IActionResult> GetByCustomer(int maKhach)
        {
            try
            {
                var list = await _context.YeuCauSuaChuas
                    .Where(x => x.MaKhach == maKhach)
                    .OrderByDescending(x => x.MaYeuCau)
                    .ToListAsync();

                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // 2. API nhận yêu cầu sửa chữa từ Flutter qua DTO
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] YeuCauSuaChuaDto dto)
        {
            try
            {
                if (dto == null) return BadRequest("Dữ liệu không hợp lệ.");

                var entity = new YeuCauSuaChua
                {
                    MaPhong = dto.MaPhong,
                    MaKhach = dto.MaKhach,
                    NoiDung = dto.NoiDung,
                    TrangThai = "Chờ tiếp nhận",
                    NgayBao = DateTime.Now
                };

                _context.YeuCauSuaChuas.Add(entity);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Gửi yêu cầu thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}