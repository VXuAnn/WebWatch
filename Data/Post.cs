using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Post
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Desciption { get; set; }

    public string? Image { get; set; }

    public string? Detail { get; set; }

    public int? MaLoai { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? ModifiedrDate { get; set; }

    public string? ModifierBy { get; set; }
}
