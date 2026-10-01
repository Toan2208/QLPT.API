using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachThueController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public KhachThueController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // Lấy danh sách toàn bộ khách thuê
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var list = await _context.KhachThues.ToListAsync();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // Lấy thông tin chi tiết 1 khách thuê theo mã
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var khach = await _context.KhachThues.FindAsync(id);
                if (khach == null) return NotFound(new { success = false, message = "Không tìm thấy khách thuê" });
                return Ok(khach);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }

        // Thêm mới khách thuê
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] KhachThue model)
        {
            try
            {
                if (model == null) return BadRequest(new { success = false, message = "Dữ liệu không hợp lệ" });

                _context.KhachThues.Add(model);
                await _context.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm khách thuê thành công", data = model });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}