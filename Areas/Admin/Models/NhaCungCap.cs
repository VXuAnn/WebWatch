using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Models
{
    public class NhaCungCap
    {
        public string MaNcc { get; set; }
        public string TenCongTy { get; set; }

        public virtual ICollection<HangHoa> HangHoas { get; set; }
    }
}
