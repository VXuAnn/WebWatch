using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Role
{
    public string RoleId { get; set; } = null!;

    public string? RoleName { get; set; }

    public string? Detail { get; set; }
}
