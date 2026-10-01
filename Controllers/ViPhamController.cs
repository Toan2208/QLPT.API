using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViPhamController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public ViPhamController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.ViPhams
                    .Include(v => v.MaPhongNavigation)
                    .Include(v => v.MaKhachNavigation)
                    .ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ViPham model)
        {
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });
                model.NgayViPham = DateTime.Now;
                model.TrangThai = "Chưa xử lý";

                _context.ViPhams.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Ghi nhận vi phạm thành công", data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}