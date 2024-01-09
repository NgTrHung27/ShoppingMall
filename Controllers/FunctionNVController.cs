using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLTTTM.models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QLTTTM.Controllers
{
    public class FunctionNVController : Controller
    {
        private readonly DataSQLContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public FunctionNVController(DataSQLContext context, IWebHostEnvironment hostEnvironment)
        {
            dbContext = context;
            webHostEnvironment = hostEnvironment;
        }


        [HttpGet]
        [ActionName("ThemNV")]
        public async Task<IActionResult> ThemNV(){
            List<ChucVu> list_cv = dbContext.ChucVus.ToList();
            ViewBag.cvs = list_cv;

            return View();
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ThemNV")]
        public async Task<IActionResult> ThemNV(NhanVien nvModel, IFormFile file)
        {

            if (ModelState.IsValid)
            {   
                if (file != null && file.Length > 0)
                {
                    // Xử lý lưu đường dẫn vào hình ảnh
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/Avatar");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // Lưu đường dẫn vào thuộc tính AVATAR của model
                    nvModel.AVATAR = "/uploads/Avatar/" + uniqueFileName;
                }
                else
                {
                    // Nếu không có hình ảnh được tải lên, bạn có thể gán một hình ảnh mặc định hoặc xử lý khác tùy ý.
                    nvModel.AVATAR = "/default-avatar.png"; // Đường dẫn đến hình ảnh mặc định
                }

                // Tiếp tục với xử lý thêm Nhân Viên và tạo tài khoản như bạn đã làm trước đây.
                await dbContext.NhanViens.AddAsync(nvModel);
                await dbContext.SaveChangesAsync();
                //
                var nhanvienIDMAX = dbContext.NhanViens.Max(x=> x.MANV);
                Account account = new Account();
                account.TAIKHOAN = "HTCACC" + nhanvienIDMAX;
                account.MATKHAU = nvModel.SDT;
                account.MANV = nhanvienIDMAX;
                await dbContext.Accounts.AddAsync(account);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("ThongTinNV", "Admin");
            }
            List<ChucVu> list_cv = dbContext.ChucVus.ToList();
            ViewBag.cvs = list_cv;
            return View(nvModel);         
        }



        [HttpPost]
        [ActionName("XoaNV")]
        public async Task<IActionResult> XoaNV(int? manv){
            if(manv != null){
                NhanVien? nhanVien = dbContext.NhanViens.SingleOrDefault(x=>x.MANV == manv);
                if(nhanVien != null){
                    // Xóa hình ảnh cũ nếu có
                    string relativePath = nhanVien.AVATAR;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }


                    Account? account = dbContext.Accounts.SingleOrDefault(x => x.MANV == nhanVien.MANV);
                    dbContext.Accounts.Remove(account);
                    dbContext.NhanViens.Remove(nhanVien);
                    dbContext.SaveChanges();
                    return RedirectToAction("ThongTinNV" , "Admin");
                }
            }            

            return RedirectToAction("TrangChu" , "Admin");

        }


        [HttpGet]
        [ActionName("CapNhatNV")]
        public async Task<IActionResult> CapNhatNV(int? manv){
            if(manv != null){
                NhanVien? nv = dbContext.NhanViens.SingleOrDefault(x=> x.MANV == manv);
                if(nv != null){
                    List<ChucVu> list_cv = dbContext.ChucVus.ToList();
                    ViewBag.cvs = list_cv;
                   
                    return View(nv);
                }
            }

            return RedirectToAction("ThongTinNV" , "Admin");
        }

        [HttpPost]
        [ActionName("CapNhatNV")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatNV(NhanVien model, IFormFile newAvatar)
        {
            if (ModelState.IsValid)
            {
                var existingNhanVien = await dbContext.NhanViens.FirstOrDefaultAsync(x => x.MANV == model.MANV);

                if (existingNhanVien == null)
                {
                    // Xử lý khi không tìm thấy Nhân Viên cần cập nhật
                    return NotFound();
                }

                // Cập nhật thông tin từ model vào Nhân Viên đã tồn tại
                existingNhanVien.HOTEN = model.HOTEN;
                existingNhanVien.GIOITINH = model.GIOITINH;
                existingNhanVien.SDT = model.SDT;
                existingNhanVien.NGAYSINH = model.NGAYSINH;
                existingNhanVien.CCCD = model.CCCD;
                existingNhanVien.DIACHI = model.DIACHI;
                existingNhanVien.EMAIL = model.EMAIL;
                existingNhanVien.NGAYVAOLAM = model.NGAYVAOLAM;


                // Kiểm tra xem có file hình ảnh mới được tải lên không
                if (newAvatar != null && newAvatar.Length > 0)
                {
                    // Xử lý lưu đường dẫn đến hình ảnh mới vào thư mục uploads
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/Avatar");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Xóa hình ảnh cũ nếu có
                    string relativePath = existingNhanVien.AVATAR;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }


                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + newAvatar.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        newAvatar.CopyTo(stream);
                    }

                    // Cập nhật đường dẫn hình ảnh mới vào Nhân Viên
                    existingNhanVien.AVATAR = "/uploads/Avatar/" + uniqueFileName;
                }

                dbContext.Update(existingNhanVien);
                await dbContext.SaveChangesAsync();
                int? manv = HttpContext.Session.GetInt32("MANV");
                if(manv != null){
                    if(manv == model.MANV){
                        return RedirectToAction("Logout" ,"Admin");
                    }
                }
                return RedirectToAction("ThongTinNV", "Admin");
            }

            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            List<ChucVu> list_cv = dbContext.ChucVus.ToList();
            ViewBag.cvs = list_cv;
            return View(model);
        }


        [HttpGet]
        [ActionName("CapNhatAcc")]
        public async Task<IActionResult> CapNhatAcc(int? id){
            if(id != null){
                Account? account = dbContext.Accounts.SingleOrDefault(x => x.ID == id);
                if(account != null){
                    return View(account);
                }
            }
            return RedirectToAction("ThongTinNV","Admin");
        }


        [HttpPost]
        [ActionName("CapNhatAcc")]
        public async Task<IActionResult> CapNhatAcc(Account model){
            if(ModelState.IsValid){
                Account? account = dbContext.Accounts.SingleOrDefault(x => x.ID == model.ID);
                if(account != null){
                    account.MATKHAU = model.MATKHAU;
                    dbContext.Accounts.Update(account);
                    await dbContext.SaveChangesAsync(); 
                    return RedirectToAction("ThongTinNV","Admin");                   
                }
            }

            return View(model);
        }



    }
}
