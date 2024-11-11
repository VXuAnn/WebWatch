using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Models
{
    public class ProductViewModel
    {
        public string? Id { get; set; }
        public string? Tenhh { get; set; }
        public string? Mota { get; set; }
        /*public string? Content { get; set; }*/
        public double? Price { get; set; }
        public string? Picture { get; set; }
        public Guid? CategoryId { get; set; }

        public string TenLoai { get; set; }

        public int MaLoai { get; set; }

        public string TenCongTy { get; set; }
        public string MaNcc { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
    }
}
