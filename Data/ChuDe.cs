using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class ChuDe
{
    public int MaCd { get; set; }

    public string? Title { get; set; }

    public string? MaNv { get; set; }

    public string? Description { get; set; }

    public Guid? CreateBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
