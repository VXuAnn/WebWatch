using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Authorized
{
    public Guid Id { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? GroupId { get; set; }

    public virtual NhomQuyen? Group { get; set; }

    public virtual Role? Role { get; set; }
}
