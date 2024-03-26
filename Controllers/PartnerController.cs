using System.Data.Entity;
using DAPM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using QLTTTM.Datas;
using QLTTTM.models;


namespace DAPM.Controllers
{
    public class PartnerController : Controller
    {
        private DataSQLContext _context = SingletonDbContext.Instance ;
        private readonly IWebHostEnvironment webHostEnvironment;
        public PartnerController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment)
        {
            _context = context;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        [ActionName("AddPartner")]
        public IActionResult AddPartner()
        {
            ViewBag.loaidts = _context.LoaiDoiTacs.ToList();
            return View();
        }

        [HttpPost]
        [ActionName("AddPartner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPartner(HopDongAndDoiTac hddtModel, IFormFile image_dt)
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
                _context.DoiTacs.AddAsync(hddtModel.DTModels);
                _context.HopDongs.AddAsync(hddtModel.HDModels);
                _context.SaveChangesAsync();

                var hopdongIDMAX = _context.HopDongs.Max(x => x.MAHD);
                var doitacIDMAX = _context.DoiTacs.Max(x => x.MADT);
                HopDongDoiTac hopDongDoiTac = new HopDongDoiTac();
                hopDongDoiTac.MAHD = hopdongIDMAX;
                hopDongDoiTac.MADT = hddtModel.DTModels.MADT;

                _context.HopDongDoiTacs.Add(hopDongDoiTac);
                _context.SaveChanges();
                return RedirectToAction("Partner", "Admin");
            }
            ViewBag.loaidts = _context.LoaiDoiTacs.ToList();
            return View(hddtModel);
        }
        // //Thuc hien xoa doi tac : 
        [HttpPost]
        [ActionName("DeletePartner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePartner(int? madt)
        {
            if (madt != null)
            {
                DoiTac? doiTac = _context.DoiTacs.SingleOrDefault(x => x.MADT == madt);
                if (doiTac != null)
                {
                    // Xóa hình ảnh cũ nếu có
                    string relativePath = doiTac.IMAGEPATH;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }

                    HopDongDoiTac? hopDongDoiTac = _context.HopDongDoiTacs.SingleOrDefault(x => x.MADT == madt);
                    HopDong? hopDong = _context.HopDongs.SingleOrDefault(x => x.MAHD == hopDongDoiTac.MAHD);
                    _context.HopDongDoiTacs.Remove(hopDongDoiTac);
                    _context.HopDongs.Remove(hopDong);
                    _context.DoiTacs.Remove(doiTac);
                    _context.SaveChanges();
                    return RedirectToAction("Partner", "Admin");
                }
            }
            return RedirectToAction("HomeAdmin" , "Admin");
        }
        //Chuc nang cap nhat DT : 
        [HttpGet]
        [ActionName("UpdatePartner")]
        public async Task<IActionResult> UpdatePartner(int? madt)
        {
            if (madt != null)
            {
                DoiTac? doiTac = _context.DoiTacs.SingleOrDefault(x => x.MADT == madt);
                if (doiTac != null)
                {
                    HopDongDoiTac? hopDongDoiTac = _context.HopDongDoiTacs.SingleOrDefault(x => x.MADT == madt);
                    if (hopDongDoiTac != null)
                    {
                        HopDong? hopDong = _context.HopDongs.SingleOrDefault(x => x.MAHD == hopDongDoiTac.MAHD);
                        HopDongAndDoiTac hopDongAndDoiTac = new HopDongAndDoiTac();
                        hopDongAndDoiTac.DTModels = doiTac;
                        hopDongAndDoiTac.HDModels = hopDong;
                        ViewBag.loaidts = _context.LoaiDoiTacs.ToList();
                        return View(hopDongAndDoiTac);
                    }
                }
            }
            return RedirectToAction("HomeAdmin","Admin");
        }
        [HttpPost]
        [ActionName("UpdatePartner")]
        public IActionResult UpdatePartner(HopDongAndDoiTac model, IFormFile image_dt, int MADT, int MAHD)
        {
            if (ModelState.IsValid)
            {
                HopDong? existingHopdong = _context.HopDongs.SingleOrDefault(x => x.MAHD == MAHD);
                DoiTac? existingDoitac = _context.DoiTacs.SingleOrDefault(x => x.MADT == MADT);

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

                _context.DoiTacs.Update(existingDoitac);
                _context.HopDongs.Update(existingHopdong);
                _context.SaveChangesAsync();

                return RedirectToAction("Partner", "Admin");
            }
            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            ViewBag.loaidts = _context.LoaiDoiTacs.ToList();
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