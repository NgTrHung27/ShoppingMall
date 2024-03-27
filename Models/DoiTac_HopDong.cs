using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{

    public class HopDongAndDoiTac
    {
        [Required(ErrorMessage = "Vui lòng điền đủ thông tin đối tác")]
        public DoiTac DTModels {get ; set;}
        [Required(ErrorMessage = "Vui lòng điền đủ thông tin hợp đồng")]
        public HopDong HDModels {get ; set;}
    }

    
}