using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QLTTTM.models{
    public class KhachHang
{
    [Key]
    public int MAKH { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập họ và tên")]
    public string HOTEN { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn ngày sinh")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    [NgaySinh]
    public  DateTime NGAYSINH { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn giới tính")]
    public bool GIOITINH { get; set; }
    [StringLength(10)]
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    public string SDT { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không đúng . Vui lòng kiểm tra lại")]
    public string EMAIL { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
    public string DIACHI { get; set; }


    [Required(ErrorMessage = "Vui lòng xác nhận trạng thái")]
    public bool TRANGTHAI { get; set; }
    
    [ForeignKey("MatBang")]
    public int MAMB {get ; set;}



   
}

}