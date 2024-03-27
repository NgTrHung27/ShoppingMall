using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTTTM.models{
    public class HopDongDoiTac
    {
    [Key]
    public int ID {get;set;}
    [ForeignKey("HopDong")]
    public int MAHD { get; set; }
    [ForeignKey("DoiTac")]
    public int MADT { get; set; }
}
    
}