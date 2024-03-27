using DAPM.StatePattern;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTTTM.models{
    public class SuKien
    {
    [Key]
    [Required]public int MASK { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập tên sự kiện")]
    public string TENSK { get; set; }

    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng chọn loại sự kiện")]
    public string LOAISK { get; set; } = "td";

    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập thời gian (từ ngày mấy tới ngày mấy)")]
    public string THOIGIAN { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập vị trí diễn ra sự kiện")]
    public string VITRI { get; set; }
    [StringLength(300000)]
    [Required(ErrorMessage = "Vui lòng nhập mô tả của sự kiện")]
    public string MOTA { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm ảnh tiêu đề của sự kiện")]
    public string IMAGEPATH { get; set; } = "defaultsk.png";
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm ảnh chủ đạo của sự kiện")]
    public string IMAGEPATH1 { get; set; } = "defaultsk.png";
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm ảnh nội dung của sự kiện")]
    public string IMAGEPATH2 { get; set; } = "defaultsk.png";

    [Required(ErrorMessage = "Vui lòng chọn trạng thái sự kiện")]
    public bool TRANGTHAI { get; set; }
    [ForeignKey("NhanVien")]
    public int MANV { get; set; }
     [NotMapped]
    public IEventState State { get; set; }
    }

}