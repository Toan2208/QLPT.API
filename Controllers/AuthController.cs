using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly Klcn052QuanLyPhongTroContext _context;

    public AuthController(Klcn052QuanLyPhongTroContext context)
    {
        _context = context;
    }

    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // Chỉ kiểm tra tên đăng nhập tồn tại là cho qua (phục vụ test giao diện)
        var user = await _context.TaiKhoans
            .FirstOrDefaultAsync(u => u.TenDangNhap == model.Username);

        if (user == null)
        {
            return Unauthorized(new { message = "Không tìm thấy tài khoản!" });
        }

        return Ok(new
        {
            message = "Đăng nhập thành công",
            vaiTro = user.VaiTro ?? "Customer",
            maKhach = user.MaKhach
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            // 1. Kiểm tra xem tên đăng nhập đã tồn tại chưa
            var existingUser = await _context.TaiKhoans
                .FirstOrDefaultAsync(u => u.TenDangNhap == model.TenDangNhap);

            if (existingUser != null)
            {
                return BadRequest(new { message = "Tên đăng nhập đã tồn tại!" });
            }

            // 2. Tạo thông tin khách thuê mới trước
            var khachThue = new KhachThue
            {
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                Cccd = model.Cccd,
                DiaChi = model.DiaChi
            };

            _context.KhachThues.Add(khachThue);
            await _context.SaveChangesAsync();

            // 3. Tạo tài khoản đăng nhập liên kết
            var taiKhoan = new TaiKhoan
            {
                TenDangNhap = model.TenDangNhap,
                MatKhauHash = model.MatKhau,
                VaiTro = "Customer",
                MaKhach = khachThue.MaKhach // Hoặc MaKhachThue tùy theo bảng của bạn
            };

            _context.TaiKhoans.Add(taiKhoan);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đăng ký thành công" });
        }
        catch (Exception ex)
        {
            // Trả thẳng lỗi chi tiết về cho App Flutter hiển thị lên màn hình
            return StatusCode(500, new { message = "Lỗi Server: " + (ex.InnerException?.Message ?? ex.Message) });
        }
    }

    // --- Các Model dùng để nhận dữ liệu từ Client ---
    public class RegisterModel
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
        public string Cccd { get; set; }
        public string DiaChi { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}