using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLTTTM.models{

    public class PhanQuyen{

        [Key]
        public int ID{get;set;}

        [ForeignKey("ChucNang")]
        [Required(ErrorMessage = "Vui chọn chức năng")]
        public int MACN {get;set;}

        [ForeignKey("ChucVu")]
        [Required(ErrorMessage = "Vui lòng chọn chức vụ")]
        public int MACV {get;set;}


        [StringLength(500)]
        public string GHICHU {get;set;} = "Empty";

    }
}