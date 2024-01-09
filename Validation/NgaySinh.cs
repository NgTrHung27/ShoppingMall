using System.ComponentModel.DataAnnotations;

public class NgaySinh : ValidationAttribute{

    public NgaySinh() => ErrorMessage = "Ngày Sinh Không Hợp Lệ";
    
    public override bool IsValid(object value){
        if(value == null) return false;


        // Lấy ngày giờ hiện tại
        DateTime now = DateTime.Now;

        // Giả sử bạn có một giá trị ngày cụ thể từ form hoặc từ bất kỳ nguồn dữ liệu nào đó
        DateTime ngayNhanTuForm = (DateTime)value;

        // So sánh ngày từ form với ngày hiện tại
        if (ngayNhanTuForm >= now)
        {
            return false;
        }
        else
        {
            int year_now  = now.Year;
            int year_ns = ngayNhanTuForm.Year;
            if(year_now - year_ns >= 18){
                return true;
            }else{
                return false;
            }

        }
    }   

}