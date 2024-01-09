using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{
    public class LoaiDoiTac
{
    [Key]
    public int MALOAIDT { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập tên loại đối tác")]
    public string TENLOAI { get; set; }
}

}