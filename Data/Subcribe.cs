using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Subcribe
{
    public int? Id { get; set; }

    public string? Email { get; set; }

    public DateOnly? CreatedDate { get; set; }
}
