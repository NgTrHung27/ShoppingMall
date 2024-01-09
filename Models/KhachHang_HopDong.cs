using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{
    public class KhachHangAndHopDong {
        [Required(ErrorMessage = "Vui lòng điền đủ thông tin Khách Hàng")]
        public KhachHang KHModels {get ; set;}
        [Required(ErrorMessage = "Vui lòng điền đủ thông tin hợp đồng")]
        public HopDong HDModels {get ; set;}
    }



}