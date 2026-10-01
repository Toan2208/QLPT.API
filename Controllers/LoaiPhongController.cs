using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiPhongController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public LoaiPhongController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // Lấy danh sách loại phòng
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.LoaiPhongs.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // Thêm loại phòng mới (Dành cho Desktop Admin)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LoaiPhong model)
        {
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

                _context.LoaiPhongs.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm loại phòng thành công", data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }

        // Xóa loại phòng
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await _context.LoaiPhongs.FindAsync(id);
                if (item == null) return NotFound(new { success = false, message = "Không tìm thấy loại phòng" });

                _context.LoaiPhongs.Remove(item);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Xóa loại phòng thành công" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}