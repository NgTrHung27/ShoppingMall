using System.ComponentModel.DataAnnotations;

namespace QLTTTM.models{
        public class ChucVu
    {
        [Key]
        public int MACV { get; set; }
        [StringLength(500)]
        [Required]
        public string TENCV { get; set; }
    }

}