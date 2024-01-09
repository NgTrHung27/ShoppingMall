using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;


namespace QLTTTM.models{
    public class MatBang
{
    [Key]
    public int MAMB { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập Tên Mặt Bằng")]
    public string TENMB { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập Diện Tích")]
    public float DIENTICH {get;set;}
    [Required(ErrorMessage = "Vui lòng nhập Giá Tiền")]
    public int GIATIEN{get;set;}
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng nhập Vị Trí")]
    public string VITRI { get; set; }
    [Required(ErrorMessage = "Vui lòng xác nhận Trạng Thái")]
    public bool TRANGTHAI { get; set; }
    [StringLength(300000)]
    [Required(ErrorMessage = "Vui lòng nhập Mô Tả")]
    public string MOTA { get; set; }
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm Hình Ảnh")]
    public string IMAGEPATH { get; set; } = "defaultmb.png";
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm Hình Ảnh")]
    public string IMAGEPATH1 { get; set; } = "defaultmb.png";
    [StringLength(500)]
    [Required(ErrorMessage = "Vui lòng thêm Hình Ảnh")]
    public string IMAGEPATH2 { get; set; } = "defaultmb.png";

}

}