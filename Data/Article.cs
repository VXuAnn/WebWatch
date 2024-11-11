using System;
using System.Collections.Generic;

namespace ShopBanHang.Data;

public partial class Article
{
    public Guid ArticleId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Keyword { get; set; }

    public string? Description { get; set; }

    public string? MaNv { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? Picture { get; set; }

    public virtual NhanVien? MaNvNavigation { get; set; }
}
