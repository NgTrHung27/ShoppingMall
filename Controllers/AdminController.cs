using System.Globalization;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using MySqlX.XDevAPI;
using QLTTTM.models;
using Microsoft.IdentityModel.Tokens;
using ZstdSharp.Unsafe;
using System.Data.Entity; // Để sử dụng JSON serialization/deserialization


namespace DAPM.Controllers
{
    public class AdminController : Controller
    {
        private DataSQLContext _data;

        public AdminController(DataSQLContext data)
        {
            _data = data;
        }



        public IActionResult HomeAdmin(int manv)
        {
            if (manv != 0)
            {
                NhanVien? nhanVien = _data.NhanViens.FirstOrDefault(x => x.MANV == manv);
                if(nhanVien != null){
                    ViewBag.CV = _data.ChucVus.FirstOrDefault(x => x.MACV == nhanVien.MACV);
                    return View(nhanVien);
                }
            }
            return RedirectToAction("Login", "Admin");
        }

        public IActionResult ThongTinNV()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null)
            {
                var data = _data;
                return View(data);
            }
            return RedirectToAction("Login", "Admin");
        }


        public IActionResult ThongTinMB()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null)
            {
                List<MatBang> list_mb = _data.MatBangs.ToList();
                return View(list_mb);
            }
            return RedirectToAction("Login", "Admin");

        }


        public IActionResult ThongTinSK()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null)
            {
                List<SuKien> list_sk = _data.SuKiens.ToList();
                return View(list_sk);
            }
            return RedirectToAction("Login", "Admin");

        }

        public IActionResult ThongTinDT()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null)
            {
                var data = _data;
                return View(data);
            }
            return RedirectToAction("Login", "Admin");
        }


        public IActionResult ThongTinKT()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null)
            {
                var data = _data;
                return View(data);
            }
            return RedirectToAction("Login", "Admin");
        }


        public IActionResult ThongTinPQ()
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (manv != null && manv == 1)
            {
                var data = _data;
                return View(data);
            }
            return RedirectToAction("Login", "Admin");
        }



        [HttpGet]
        [ActionName("Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string taikhoan, string matkhau)
        {
            if (!string.IsNullOrEmpty(taikhoan) && !string.IsNullOrEmpty(matkhau))
            {
                // Tìm tài khoản trong cơ sở dữ liệu
                Account? account = _data.Accounts.SingleOrDefault(x => x.TAIKHOAN == taikhoan && x.MATKHAU == matkhau);

                if (account != null)
                {
                    // Đặt thời gian timeout cho session
                    HttpContext.Session.SetString("SessionTimeout", DateTime.Now.AddMinutes(30).ToString()); // Timeout sau 30 phút

                    //Luu vao sesson : 
                    HttpContext.Session.SetString("TAIKHOAN", account.TAIKHOAN);
                    HttpContext.Session.SetString("MATKHAU", account.MATKHAU);
                    HttpContext.Session.SetInt32("MANV", account.MANV);


                    return RedirectToAction("HomeAdmin", new { manv = account.MANV }); // Thay "Dashboard" bằng tên action/method bạn muốn chuyển hướng đến sau khi đăng nhập thành công.
                }
                else
                {
                    // Đăng nhập không thành công, có thể cung cấp thông báo lỗi cho người dùng
                    ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu không đúng.";
                    return View();
                }
            }
            else
            {
                // Dữ liệu không hợp lệ, có thể cung cấp thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Vui lòng điền đầy đủ tên tài khoản và mật khẩu.";
                return View();
            }
        }


        public IActionResult Logout()
        {
            // Xóa các session đã lưu trữ khi người dùng đăng xuất
            HttpContext.Session.Remove("TAIKHOAN");
            HttpContext.Session.Remove("MATKHAU");
            HttpContext.Session.Remove("MANV");
            // Điều hướng người dùng về trang đăng nhập hoặc trang chính (tuỳ bạn)
            return RedirectToAction("Login", "Admin"); // Điều hướng đến trang đăng nhập sau khi đăng xuất.
        }




        [HttpPost]
        [ActionName("ThemPhanQuyen")]
        public async Task<IActionResult> ThemPhanQuyen(int macv, int macn, string noidung)
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (macv != 0 && macn != 0)
            {
                PhanQuyen phanQuyen = new PhanQuyen();
                phanQuyen.MACV = macv;
                phanQuyen.MACN = macn;
                if (string.IsNullOrEmpty(noidung))
                {
                    noidung = "Empty";
                }
                phanQuyen.GHICHU = noidung;
                await _data.PhanQuyens.AddAsync(phanQuyen);
                await _data.SaveChangesAsync();
                return RedirectToAction("ThongTinPQ", "Admin");
            }
            return RedirectToAction("HomeAdmin", "Admin", new { MANV = manv });
        }



        [HttpPost]
        [ActionName("XoaPQ")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaPQ(int ID)
        {

            if (ID != 0)
            {
                PhanQuyen? phanQuyen = _data.PhanQuyens.SingleOrDefault(x => x.ID == ID);
                if (phanQuyen != null)
                {
                    _data.PhanQuyens.Remove(phanQuyen);
                    await _data.SaveChangesAsync();
                    return RedirectToAction("ThongTinPQ", "Admin");
                }
            }
            return RedirectToAction("ThongTinPQ", "Admin");
        }



        [HttpPost]
        [ActionName("ThemCV")]
        public async Task<IActionResult> ThemCV(string tencv)
        {
            int? manv = HttpContext.Session.GetInt32("MANV");
            if (!string.IsNullOrEmpty(tencv))
            {
                ChucVu chucVu = new ChucVu();
                chucVu.TENCV = tencv;
                await _data.ChucVus.AddAsync(chucVu);
                await _data.SaveChangesAsync();
                return RedirectToAction("ThongTinPQ", "Admin");
            }
            return RedirectToAction("HomeAdmin", "Admin", new { MANV = manv });
        }


        [HttpPost]
        [ActionName("XoaCV")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaCV(int macv)
        {

            if (macv != 0)
            {
                ChucVu? chucVu = _data.ChucVus.SingleOrDefault(x => x.MACV == macv);
                if (chucVu != null)
                {
                    _data.ChucVus.Remove(chucVu);
                    await _data.SaveChangesAsync();
                    return RedirectToAction("ThongTinPQ", "Admin");
                }
            }
            return RedirectToAction("ThongTinPQ", "Admin");
        }



    }
}