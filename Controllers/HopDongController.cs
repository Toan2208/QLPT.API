using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models; // Namespace chứa các Model và DbContext của bạn

namespace QLPT.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HopDongController : ControllerBase
    {
        private readonly Klcn052QuanLyPhongTroContext _context;

        public HopDongController(Klcn052QuanLyPhongTroContext context)
        {
            _context = context;
        }

        // API lấy hợp đồng theo mã khách thuê cho Mobile (Flutter)
        [HttpGet("customer/{maKhach}")]
        public async Task<IActionResult> GetContractsByCustomer(int maKhach)
        {
            try
            {
                var contracts = await _context.HopDongs
                    .Where(h => h.MaKhachDaiDien == maKhach)
                    .Select(h => new {
                        maHopDong = h.MaHopDong,
                        maPhong = h.MaPhong,
                        tenPhong = "Phòng " + h.MaPhong,
                        giaThue = h.GiaThueThang,
                        tienCoc = h.TienCoc,
                        ngayBatDau = h.NgayBatDau.ToString("yyyy-MM-dd"),
                        ngayKetThuc = h.NgayKetThuc.ToString("yyyy-MM-dd"),
                        trangThai = h.TrangThai
                    })
                    .ToListAsync();

                return Ok(contracts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Lỗi server: {ex.Message}" });
            }
        }
    }
}