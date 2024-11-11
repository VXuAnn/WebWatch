using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class NhanVien
{
    public string MaNv { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? LoginName { get; set; }

    public string? Password { get; set; }

    public Guid? GroupId { get; set; }

    public Guid? CategoryId { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = new List<Article>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<ChuDe> ChuDes { get; set; } = new List<ChuDe>();

    public virtual NhomQuyen? Group { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<HoiDap> HoiDaps { get; set; } = new List<HoiDap>();

    public virtual ICollection<PhanCong> PhanCongs { get; set; } = new List<PhanCong>();
}
