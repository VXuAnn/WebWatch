using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class GioHang
{
    public int MaGh { get; set; }

    public string? MaKh { get; set; }

    public int? MaHh { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public virtual HangHoa MaGhNavigation { get; set; } = null!;

    public virtual KhachHang? MaKhNavigation { get; set; }
}
