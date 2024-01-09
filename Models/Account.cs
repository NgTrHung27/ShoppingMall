using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{
   public class Account
{
    public int ID { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập Tài Khoản")]
    public string TAIKHOAN { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu")]
    public string MATKHAU { get; set; }
    [Required]
    public int MANV { get; set; }

}

}