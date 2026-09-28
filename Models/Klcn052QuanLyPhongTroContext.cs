using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QLPT.API.Models;

public partial class Klcn052QuanLyPhongTroContext : DbContext
{
    public Klcn052QuanLyPhongTroContext()
    {
    }

    public Klcn052QuanLyPhongTroContext(DbContextOptions<Klcn052QuanLyPhongTroContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BanGiaoTaiSan> BanGiaoTaiSans { get; set; }

    public virtual DbSet<BaoTraPhong> BaoTraPhongs { get; set; }

    public virtual DbSet<ChiSoDienNuoc> ChiSoDienNuocs { get; set; }

    public virtual DbSet<ChiTietBanGiao> ChiTietBanGiaos { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<CoSo> CoSos { get; set; }

    public virtual DbSet<CuTru> CuTrus { get; set; }

    public virtual DbSet<DangKyDichVu> DangKyDichVus { get; set; }

    public virtual DbSet<DatPhong> DatPhongs { get; set; }

    public virtual DbSet<DichVu> DichVus { get; set; }

    public virtual DbSet<GiaHanHopDong> GiaHanHopDongs { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HopDong> HopDongs { get; set; }

    public virtual DbSet<KhachThue> KhachThues { get; set; }

    public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }

    public virtual DbSet<PhieuThuChi> PhieuThuChis { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<PhuongTien> PhuongTiens { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiSan> TaiSans { get; set; }

    public virtual DbSet<TaiSanPhong> TaiSanPhongs { get; set; }

    public virtual DbSet<ThanhLy> ThanhLies { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<ThongBaoNguoiNhan> ThongBaoNguoiNhans { get; set; }

    public virtual DbSet<ViPham> ViPhams { get; set; }

    public virtual DbSet<YeuCauSuaChua> YeuCauSuaChuas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=KLCN052_QuanLyPhongTro;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BanGiaoTaiSan>(entity =>
        {
            entity.HasKey(e => e.MaBanGiao).HasName("PK__BanGiaoT__6484252D966B34F3");

            entity.ToTable("BanGiaoTaiSan");

            entity.Property(e => e.ChiSoDien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChiSoNuoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.LoaiBanGiao).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.BanGiaoTaiSans)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BanGiaoTa__MaHop__31B762FC");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.BanGiaoTaiSans)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BanGiaoTa__MaPho__32AB8735");
        });

        modelBuilder.Entity<BaoTraPhong>(entity =>
        {
            entity.HasKey(e => e.MaBaoTra).HasName("PK__BaoTraPh__51A9CA49F609AE3F");

            entity.ToTable("BaoTraPhong");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.LyDo).HasMaxLength(300);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.BaoTraPhongs)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaoTraPho__MaHop__339FAB6E");

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.BaoTraPhongs)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BaoTraPho__MaKha__3493CFA7");
        });

        modelBuilder.Entity<ChiSoDienNuoc>(entity =>
        {
            entity.HasKey(e => e.MaChiSo).HasName("PK__ChiSoDie__EBA18E15B19A5D02");

            entity.ToTable("ChiSoDienNuoc");

            entity.Property(e => e.ChiSoCu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChiSoMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChiSoOcr)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("ChiSoOCR");
            entity.Property(e => e.ChiSoXacNhan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DuongDanAnh).HasMaxLength(500);
            entity.Property(e => e.LoaiChiSo).HasMaxLength(20);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.ChiSoDienNuocs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiSoDien__MaPho__3587F3E0");
        });

        modelBuilder.Entity<ChiTietBanGiao>(entity =>
        {
            entity.HasKey(e => e.MaChiTietBanGiao).HasName("PK__ChiTietB__576823D373B498E6");

            entity.ToTable("ChiTietBanGiao");

            entity.Property(e => e.ChiPhiPhat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.TinhTrang).HasMaxLength(100);

            entity.HasOne(d => d.MaBanGiaoNavigation).WithMany(p => p.ChiTietBanGiaos)
                .HasForeignKey(d => d.MaBanGiao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietBa__MaBan__367C1819");

            entity.HasOne(d => d.MaTaiSanNavigation).WithMany(p => p.ChiTietBanGiaos)
                .HasForeignKey(d => d.MaTaiSan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietBa__MaTai__37703C52");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaChiTiet).HasName("PK__ChiTietH__CDF0A11436F17DF2");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NoiDung).HasMaxLength(200);
            entity.Property(e => e.SoLuong).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__MaHoa__3864608B");
        });

        modelBuilder.Entity<CoSo>(entity =>
        {
            entity.HasKey(e => e.MaCoSo).HasName("PK__CoSo__152D0634E4BF1F50");

            entity.ToTable("CoSo");

            entity.Property(e => e.DiaChi).HasMaxLength(300);
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TenCoSo).HasMaxLength(100);
        });

        modelBuilder.Entity<CuTru>(entity =>
        {
            entity.HasKey(e => e.MaCuTru).HasName("PK__CuTru__7DEA613F69008D41");

            entity.ToTable("CuTru");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.TrangThaiCuTru).HasMaxLength(50);
            entity.Property(e => e.TrangThaiKhaiBao).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.CuTrus)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuTru__MaHopDong__395884C4");

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.CuTrus)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuTru__MaKhach__3A4CA8FD");
        });

        modelBuilder.Entity<DangKyDichVu>(entity =>
        {
            entity.HasKey(e => e.MaDangKy).HasName("PK__DangKyDi__BA90F02DF5CF98F2");

            entity.ToTable("DangKyDichVu");

            entity.Property(e => e.DonGiaApDung).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.DangKyDichVus)
                .HasForeignKey(d => d.MaDichVu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangKyDic__MaDic__3B40CD36");

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.DangKyDichVus)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DangKyDic__MaHop__3C34F16F");
        });

        modelBuilder.Entity<DatPhong>(entity =>
        {
            entity.HasKey(e => e.MaDatPhong).HasName("PK__DatPhong__6344ADEA556B1885");

            entity.ToTable("DatPhong");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.TienCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatPhong__MaKhac__3D2915A8");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.DatPhongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DatPhong__MaPhon__3E1D39E1");
        });

        modelBuilder.Entity<DichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu).HasName("PK__DichVu__C0E6DE8F6D485D05");

            entity.ToTable("DichVu");

            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonViTinh).HasMaxLength(30);
            entity.Property(e => e.TenDichVu).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<GiaHanHopDong>(entity =>
        {
            entity.HasKey(e => e.MaGiaHan).HasName("PK__GiaHanHo__C3260BA43B328D89");

            entity.ToTable("GiaHanHopDong");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.GiaThueMoi).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.GiaHanHopDongs)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GiaHanHop__MaHop__3F115E1A");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__HoaDon__835ED13BED5470BF");

            entity.ToTable("HoaDon");

            entity.Property(e => e.ConLai).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DaThanhToan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__MaHopDon__40058253");
        });

        modelBuilder.Entity<HopDong>(entity =>
        {
            entity.HasKey(e => e.MaHopDong).HasName("PK__HopDong__36DD4342997587A5");

            entity.ToTable("HopDong");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.GiaThueThang).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TienCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachDaiDienNavigation).WithMany(p => p.HopDongs)
                .HasForeignKey(d => d.MaKhachDaiDien)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HopDong__MaKhach__40F9A68C");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.HopDongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HopDong__MaPhong__41EDCAC5");
        });

        modelBuilder.Entity<KhachThue>(entity =>
        {
            entity.HasKey(e => e.MaKhach).HasName("PK__KhachThu__D0CB8DDDCEDBD5D8");

            entity.ToTable("KhachThue");

            entity.Property(e => e.Cccd)
                .HasMaxLength(20)
                .HasColumnName("CCCD");
            entity.Property(e => e.DiaChi).HasMaxLength(300);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.GioiTinh).HasMaxLength(20);
            entity.Property(e => e.HoTen).HasMaxLength(100);
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
        });

        modelBuilder.Entity<LoaiPhong>(entity =>
        {
            entity.HasKey(e => e.MaLoaiPhong).HasName("PK__LoaiPhon__23021217651F5599");

            entity.ToTable("LoaiPhong");

            entity.Property(e => e.DienTich).HasColumnType("decimal(8, 2)");
            entity.Property(e => e.GiaMacDinh).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.TenLoaiPhong).HasMaxLength(100);
        });

        modelBuilder.Entity<PhieuThuChi>(entity =>
        {
            entity.HasKey(e => e.MaPhieu).HasName("PK__ThuChi__3595D0B05862EB56");

            entity.ToTable("PhieuThuChi");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.LoaiPhieu).HasMaxLength(20);
            entity.Property(e => e.NguoiNopNhan).HasMaxLength(100);
            entity.Property(e => e.NoiDung).HasMaxLength(500);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__Phong__20BD5E5BE6AEF696");

            entity.ToTable("Phong");

            entity.Property(e => e.GiaThue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.TenPhong).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaCoSoNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaCoSo)
                .HasConstraintName("FK_Phong_CoSo");

            entity.HasOne(d => d.MaLoaiPhongNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaLoaiPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Phong__MaLoaiPho__42E1EEFE");
        });

        modelBuilder.Entity<PhuongTien>(entity =>
        {
            entity.HasKey(e => e.MaPhuongTien).HasName("PK__PhuongTi__35B6C8B08FC543EC");

            entity.ToTable("PhuongTien");

            entity.Property(e => e.BienSoXe).HasMaxLength(20);
            entity.Property(e => e.LoaiXe).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.PhuongTiens)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PhuongTien_KhachThue");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529C7B4816F");

            entity.ToTable("TaiKhoan");

            entity.HasIndex(e => e.MaKhach, "UQ_TaiKhoan_MaKhach")
                .IsUnique()
                .HasFilter("([MaKhach] IS NOT NULL)");

            entity.HasIndex(e => e.TenDangNhap, "UQ__TaiKhoan__55F68FC095C84813").IsUnique();

            entity.Property(e => e.MatKhauHash).HasMaxLength(500);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.TenDangNhap).HasMaxLength(100);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.VaiTro).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachNavigation).WithOne(p => p.TaiKhoan)
                .HasForeignKey<TaiKhoan>(d => d.MaKhach)
                .HasConstraintName("FK_TaiKhoan_KhachThue");
        });

        modelBuilder.Entity<TaiSan>(entity =>
        {
            entity.HasKey(e => e.MaTaiSan).HasName("PK__TaiSan__8DB7C7BE4F973083");

            entity.ToTable("TaiSan");

            entity.Property(e => e.DonViTinh).HasMaxLength(30);
            entity.Property(e => e.GiaTriThamKhao).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MoTa).HasMaxLength(300);
            entity.Property(e => e.TenTaiSan).HasMaxLength(100);
        });

        modelBuilder.Entity<TaiSanPhong>(entity =>
        {
            entity.HasKey(e => e.MaTaiSanPhong).HasName("PK__TaiSanPh__1346C5B268BB1E10");

            entity.ToTable("TaiSanPhong");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.TinhTrang).HasMaxLength(100);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.TaiSanPhongs)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiSanPho__MaPho__43D61337");

            entity.HasOne(d => d.MaTaiSanNavigation).WithMany(p => p.TaiSanPhongs)
                .HasForeignKey(d => d.MaTaiSan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__TaiSanPho__MaTai__44CA3770");
        });

        modelBuilder.Entity<ThanhLy>(entity =>
        {
            entity.HasKey(e => e.MaThanhLy).HasName("PK__ThanhLy__7EF122A9CCB61B6F");

            entity.ToTable("ThanhLy");

            entity.HasIndex(e => e.MaHopDong, "UQ__ThanhLy__36DD434310125A81").IsUnique();

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.TienHoanCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TienNo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TienPhat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TongQuyetToan).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHopDongNavigation).WithOne(p => p.ThanhLy)
                .HasForeignKey<ThanhLy>(d => d.MaHopDong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhLy__MaHopDo__45BE5BA9");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__ThanhToa__D4B25844ADA5B370");

            entity.ToTable("ThanhToan");

            entity.Property(e => e.GhiChu).HasMaxLength(300);
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ThanhToans)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ThanhToan__MaHoa__46B27FE2");
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PK__ThongBao__04DEB54EE4B468FF");

            entity.ToTable("ThongBao");

            entity.Property(e => e.LoaiThongBao).HasMaxLength(50);
            entity.Property(e => e.NoiDung).HasMaxLength(1000);
            entity.Property(e => e.TieuDe).HasMaxLength(200);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<ThongBaoNguoiNhan>(entity =>
        {
            entity.HasKey(e => new { e.MaThongBao, e.MaTaiKhoan });

            entity.ToTable("ThongBaoNguoiNhan");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.ThongBaoNguoiNhans)
                .HasForeignKey(d => d.MaTaiKhoan)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThongBaoNguoiNhan_TaiKhoan");

            entity.HasOne(d => d.MaThongBaoNavigation).WithMany(p => p.ThongBaoNguoiNhans)
                .HasForeignKey(d => d.MaThongBao)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThongBaoNguoiNhan_ThongBao");
        });

        modelBuilder.Entity<ViPham>(entity =>
        {
            entity.HasKey(e => e.MaViPham).HasName("PK__ViPham__F1921D8950F9662F");

            entity.ToTable("ViPham");

            entity.Property(e => e.DuongDanAnh).HasMaxLength(500);
            entity.Property(e => e.HinhThucXuLy).HasMaxLength(300);
            entity.Property(e => e.NoiDung).HasMaxLength(500);
            entity.Property(e => e.TienPhat).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.ViPhams)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViPham__MaKhach__498EEC8D");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.ViPhams)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ViPham__MaPhong__4A8310C6");
        });

        modelBuilder.Entity<YeuCauSuaChua>(entity =>
        {
            entity.HasKey(e => e.MaYeuCau).HasName("PK__YeuCauSu__CFA5DF4E158FF715");

            entity.ToTable("YeuCauSuaChua");

            entity.Property(e => e.ChiPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DuongDanAnh).HasMaxLength(500);
            entity.Property(e => e.NoiDung).HasMaxLength(500);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaKhachNavigation).WithMany(p => p.YeuCauSuaChuas)
                .HasForeignKey(d => d.MaKhach)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuCauSua__MaKha__4B7734FF");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.YeuCauSuaChuas)
                .HasForeignKey(d => d.MaPhong)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__YeuCauSua__MaPho__4C6B5938");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
