using System.ComponentModel.DataAnnotations;

public class NgayVaoLam : ValidationAttribute{

    public NgayVaoLam() => ErrorMessage = "Ngày Vào Làm Không Hợp Lệ";
    
    public override bool IsValid(object value){
        if(value == null) return false;


        // Lấy ngày giờ hiện tại
        DateTime now = DateTime.Now;

        // Giả sử bạn có một giá trị ngày cụ thể từ form hoặc từ bất kỳ nguồn dữ liệu nào đó
        DateTime ngayNhanTuForm = (DateTime)value;

        // So sánh ngày từ form với ngày hiện tại
        if (ngayNhanTuForm <= now)
        {
            int year_now  = now.Year;
            int year_ns = ngayNhanTuForm.Year;
            if(year_now - year_ns > 100){
                return false;
            }else{
                return true;
            }
        }
        else
        {
           return false;
        }
    }   

}