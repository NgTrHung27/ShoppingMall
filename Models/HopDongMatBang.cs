using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTTTM.models{
    public class HopDongMatBang
{
    [Key]
    public int ID {get;set;}
    [ForeignKey("HopDong")]
    public int MAHD { get; set; }
    [ForeignKey("MatBang")]
    public int MAMB { get; set; }
    [ForeignKey("KhachHang")]
    public int MAKH { get; set; }

 
}

}