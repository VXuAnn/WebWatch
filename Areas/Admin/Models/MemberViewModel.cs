using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Models
{
    public class MemberViewModel
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? LoginName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public Guid? GroupId { get; set; }

    }
}
