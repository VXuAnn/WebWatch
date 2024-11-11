using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string? Name { get; set; }

    public Guid? ParentId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();

    public virtual ICollection<NhanVien> NhanViens { get; set; } = new List<NhanVien>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
