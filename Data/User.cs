using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class User
{
    public string Idcode { get; set; } = null!;

    public string? UserId { get; set; }

    public string? UserPassword { get; set; }

    public int? TimeLoginFail { get; set; }

    public string? Emai { get; set; }

    public DateOnly? DateTimeLogin { get; set; }
}
