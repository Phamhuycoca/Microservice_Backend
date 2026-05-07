using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tài khoản")]
        public string tai_khoan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string mat_khau { get; set; }
        public bool RememberMe { get; set; }
    }
}
