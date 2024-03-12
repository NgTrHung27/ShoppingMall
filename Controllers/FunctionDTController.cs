using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using QLTTTM.models;


namespace DAPM.Controllers
{
    public class FunctionDTController : Controller
    {
        private DataSQLContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public FunctionDTController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment)
        {
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;

        }

        [HttpGet]
        [ActionName("AddDT")]
        public IActionResult AddDT()
        {
            ViewBag.loaidts = dbContext.LoaiDoiTacs.ToList();
            return View();
        }

        [HttpPost]
        [ActionName("AddDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDT(HopDongAndDoiTac hddtModel, IFormFile image_dt)
        {
            if (ModelState.IsValid)
            {
                if (image_dt != null && image_dt.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image_dt.FileName;
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/DoiTac", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        image_dt.CopyTo(stream);
                    }

                    hddtModel.DTModels.IMAGEPATH = "/uploads/DoiTac/" + uniqueFileName;
                }
                // Lưu dữ liệu vào cơ sở dữ liệu
                await dbContext.DoiTacs.AddAsync(hddtModel.DTModels);
                await dbContext.HopDongs.AddAsync(hddtModel.HDModels);
                await dbContext.SaveChangesAsync();

                var hopdongIDMAX = dbContext.HopDongs.Max(x => x.MAHD);
                var doitacIDMAX = dbContext.DoiTacs.Max(x => x.MADT);
                HopDongDoiTac hopDongDoiTac = new HopDongDoiTac();
                hopDongDoiTac.MAHD = hopdongIDMAX;
                hopDongDoiTac.MADT = hddtModel.DTModels.MADT;

                dbContext.HopDongDoiTacs.Add(hopDongDoiTac);
                dbContext.SaveChanges();

                return RedirectToAction("ThongTinDT", "Admin");
            }
            ViewBag.loaidts = dbContext.LoaiDoiTacs.ToList();
            return View(hddtModel);
        }
        // //Thuc hien xoa doi tac : 
        [HttpPost]
        [ActionName("DeleteDT")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDT(int? madt)
        {
            if (madt != null)
            {
                DoiTac? doiTac = dbContext.DoiTacs.SingleOrDefault(x => x.MADT == madt);
                if (doiTac != null)
                {
                    // Xóa hình ảnh cũ nếu có
                    string relativePath = doiTac.IMAGEPATH;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }

                    HopDongDoiTac? hopDongDoiTac = dbContext.HopDongDoiTacs.SingleOrDefault(x => x.MADT == madt);
                    HopDong? hopDong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == hopDongDoiTac.MAHD);
                    dbContext.HopDongDoiTacs.Remove(hopDongDoiTac);
                    dbContext.HopDongs.Remove(hopDong);
                    dbContext.DoiTacs.Remove(doiTac);
                    dbContext.SaveChanges();
                    return RedirectToAction("ThongTinDT", "Admin");
                }
            }

            return RedirectToAction("TrangChu", "Admin");

        }
        //Chuc nang cap nhat DT : 
        [HttpGet]
        [ActionName("CapNhatDT")]
        public async Task<IActionResult> CapNhatDT(int? madt)
        {
            if (madt != null)
            {
                DoiTac? doiTac = dbContext.DoiTacs.SingleOrDefault(x => x.MADT == madt);
                if (doiTac != null)
                {
                    HopDongDoiTac? hopDongDoiTac = dbContext.HopDongDoiTacs.SingleOrDefault(x => x.MADT == madt);
                    if (hopDongDoiTac != null)
                    {
                        HopDong? hopDong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == hopDongDoiTac.MAHD);
                        HopDongAndDoiTac hopDongAndDoiTac = new HopDongAndDoiTac();
                        hopDongAndDoiTac.DTModels = doiTac;
                        hopDongAndDoiTac.HDModels = hopDong;
                        ViewBag.loaidts = dbContext.LoaiDoiTacs.ToList();
                        return View(hopDongAndDoiTac);
                    }
                }
            }
            return RedirectToAction("TrangChu", "Admin");
        }
        [HttpPost]
        [ActionName("CapNhatDT")]
        public IActionResult CapNhatDT(HopDongAndDoiTac model, IFormFile image_dt, int MADT, int MAHD)
        {
            if (ModelState.IsValid)
            {
                HopDong? existingHopdong = dbContext.HopDongs.SingleOrDefault(x => x.MAHD == MAHD);
                DoiTac? existingDoitac = dbContext.DoiTacs.SingleOrDefault(x => x.MADT == MADT);

                if (existingDoitac == null || existingHopdong == null)
                {
                    // Xử lý khi không tìm thấy Nhân Viên cần cập nhật
                    return NotFound();
                }

                existingDoitac.TENDT = model.DTModels.TENDT;
                existingDoitac.MALOAIDOITAC = model.DTModels.MALOAIDOITAC;
                existingDoitac.SDT = model.DTModels.SDT;
                existingDoitac.EMAIL = model.DTModels.EMAIL;
                existingDoitac.DIRECTION = model.DTModels.DIRECTION;



                existingHopdong.TENHDDT = model.HDModels.TENHDDT;
                existingHopdong.LOAIHOPDONG = model.HDModels.LOAIHOPDONG;
                existingHopdong.THOIHAN = model.HDModels.THOIHAN;
                existingHopdong.HIEULUC = model.HDModels.HIEULUC;
                existingHopdong.MANV = model.HDModels.MANV;

                // Kiểm tra xem có file hình ảnh mới được tải lên không
                existingDoitac.IMAGEPATH = ImagePath(image_dt, existingDoitac.IMAGEPATH);

                dbContext.DoiTacs.Update(existingDoitac);
                dbContext.HopDongs.Update(existingHopdong);
                dbContext.SaveChangesAsync();

                return RedirectToAction("ThongTinDT", "Admin");
            }
            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            ViewBag.loaidts = dbContext.LoaiDoiTacs.ToList();
            return View(model);
        }

        //Phuong Thuc xu ly anh : 
        private string ImagePath(IFormFile image, string imagepath)
        {
            // Kiểm tra xem có file hình ảnh mới được tải lên không
            if (image != null && image.Length > 0)
            {
                // Xử lý lưu đường dẫn đến hình ảnh mới vào thư mục uploads
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/DoiTac");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Xóa hình ảnh cũ nếu có
                string relativePath = imagepath;
                string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                if (System.IO.File.Exists(absolutePath))
                {
                    System.IO.File.Delete(absolutePath);
                }


                //Them lai duong dan moi : 
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                // Cập nhật đường dẫn hình ảnh mới vào Nhân Viên
                string newImagePath = "/uploads/DoiTac/" + uniqueFileName;
                return newImagePath;
            }

            return imagepath;
        }
    }
}