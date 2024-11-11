using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class NhomQuyen
{
    public Guid GroupId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Authorized> Authorizeds { get; set; } = new List<Authorized>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();
}
