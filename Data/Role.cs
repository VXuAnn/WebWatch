using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public Guid? CategoryId { get; set; }

    public virtual ICollection<Authorized> Authorizeds { get; set; } = new List<Authorized>();

    public virtual Category? Category { get; set; }
}
