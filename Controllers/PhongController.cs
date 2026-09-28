using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLPT.API.Models; // Đảm bảo namespace này khớp với tên dự án API của bạn

[Route("api/[controller]")]
[ApiController]
public class PhongController : ControllerBase
{
    private readonly Klcn052QuanLyPhongTroContext _context;

    public PhongController(Klcn052QuanLyPhongTroContext context)
    {
        _context = context;
    }

    // GET: api/Phong
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Phong>>> GetPhongs()
    {
        return await _context.Phongs.ToListAsync();
    }

    // POST: api/Phong (Thêm phòng mới)
    [HttpPost]
    public async Task<ActionResult<Phong>> PostPhong(Phong phong)
    {
        _context.Phongs.Add(phong);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPhongs), new { id = phong.MaPhong }, phong);
    }
}