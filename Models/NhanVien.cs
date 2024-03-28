
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace QLTTTM.models{
    public class NhanVien
    {
       [Key]
        public int MANV { get; set; }
        [StringLength(500)]
        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên")]
        public string HOTEN { get; set; }



        [Required(ErrorMessage = "Vui lòng chọn Giới Tính")]
        [Range(0, 1, ErrorMessage = "Giới Tính không hợp lệ")]  
        public bool GIOITINH { get; set; }

    

        [StringLength(10)]
        [Required (ErrorMessage = "Vui lòng nhập Số Điện Thoại")]
        public string SDT { get; set; }

       

        [Required(ErrorMessage = "Vui lòng nhập Ngày Sinh")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NgaySinh]
        public  DateTime NGAYSINH { get; set; }


        [StringLength(15)]
        [Required(ErrorMessage ="Vui lòng nhập CCCD/CMND")]
        public string CCCD { get; set; }


        [StringLength(500)]
        [Required(ErrorMessage = "Vui lòng nhập Địa Chỉ")]
        public string DIACHI { get; set; }
       

        [StringLength(500)]
        [Required(ErrorMessage ="Vui lòng nhập Email")]
        [EmailAddress(ErrorMessage = "Phải là địa chỉ email")]
        public string EMAIL { get; set; }


        [Required(ErrorMessage = "Vui lòng ập Ngày Vào Làm")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [NgayVaoLam]
        public DateTime NGAYVAOLAM { get; set; }
       
        
        [StringLength(200)]
        [Required(ErrorMessage = "Vui lòng thêm hình ảnh")]
        public string AVATAR { get; set; } = "/default-avatar.png";


        [ForeignKey("ChucVu")]
        [Required(ErrorMessage = "Vui lòng chọn chức Vụ")]
        public int MACV { get; set; }

    }

}