using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace QLTTTM.models{
    public class DoiTac
{
    [Key]
    public int MADT { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập tên đối tác")]
    public string TENDT { get; set; }
    [ForeignKey("LoaiDoiTac")]
    [Required(ErrorMessage = "Vui lòng nhập chọn loại đối tác")]
    public int MALOAIDOITAC { get; set; }
    [StringLength(10)]
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại đối tác")]
    public string SDT { get; set; }
    [StringLength(200)]
    [Required(ErrorMessage = "Vui lòng nhập email đối tác")]
    [EmailAddress(ErrorMessage = "Phải là email")]
    public string EMAIL { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập đường dẫn tới trang website của đối tác")]
    public string DIRECTION { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm hình ảnh của đối tác")]
    public string IMAGEPATH { get; set; } = "defaultdt.png";

}

}