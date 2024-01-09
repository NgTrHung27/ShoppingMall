using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{

    public class ChucNang{

        [Key]
        public int MACN {get;set;}

        [Required]
        [StringLength(500)]
        public string TENCN {get;set;}

    }
}