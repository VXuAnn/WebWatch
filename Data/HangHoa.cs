using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class HangHoa
{
    public int MaHh { get; set; }

    public string TenHh { get; set; } = null!;

    public string? TenAlias { get; set; }

    public int MaLoai { get; set; }

    public double? DonGia { get; set; }

    public string? Hinh { get; set; }

    public double GiamGia { get; set; }

    public string? MoTa { get; set; }

    public string MaNcc { get; set; } = null!;

    public Guid? CategoryId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<BanBe> BanBes { get; set; } = new List<BanBe>();

    public virtual Category? Category { get; set; }

    public virtual ICollection<ChiTietHd> ChiTietHds { get; set; } = new List<ChiTietHd>();

    public virtual GioHang? GioHang { get; set; }

    public virtual Loai MaLoaiNavigation { get; set; } = null!;

    public virtual NhaCungCap MaNccNavigation { get; set; } = null!;

    public virtual ICollection<YeuThich> YeuThiches { get; set; } = new List<YeuThich>();
}
