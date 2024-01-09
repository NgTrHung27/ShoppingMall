using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTTTM.models{
    public class HopDong
{
    [Key]
    public int MAHD { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập tên hợp đồng")]
    public string TENHDDT { get; set; }
    [StringLength(200)]
    [Required(ErrorMessage = "Vui lòng nhập loại hợp đồng")]
    public string LOAIHOPDONG { get; set; }
    [StringLength(100)]
    [Required(ErrorMessage = "Vui lòng nhập thời hạn hợp đồng")]
    public string THOIHAN { get; set; }
    [Required(ErrorMessage = "Vui lòng chọn ngày bắt đầu hiệu lực của hợp đồng")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime HIEULUC { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập nội dung hợp đồng")]
    [StringLength(50000)]
    public string NOIDUNG {get ; set;}

    [ForeignKey("NhanVien")]
    public int MANV { get; set; }

   
}

}