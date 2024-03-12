using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using QLTTTM.models;


namespace DAPM.Controllers{
    public class FunctionMBController: Controller{
        private DataSQLContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public FunctionMBController(DataSQLContext context, IWebHostEnvironment _webHostEnvironment ){
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;
        }


        [HttpGet]
        [ActionName("ThemMB")]
        public async Task<IActionResult> ThemMB(){
            return View();
        }

        [HttpPost]
        [ActionName("ThemMB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemMB(MatBang mbModel, IFormFile tieude, IFormFile chudao, List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {
                // Xử lý tệp hình ảnh tiêu đề (tieude) và lưu đường dẫn vào IMAGEPATH
                if (tieude != null && tieude.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + tieude.FileName;
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/MatBang", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        tieude.CopyTo(stream);
                    }

                    mbModel.IMAGEPATH = "/uploads/MatBang/" + uniqueFileName;
                }

                // Xử lý tệp hình ảnh chủ đạo (chudao) và lưu đường dẫn vào IMAGEPATH1
                if (chudao != null && chudao.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + chudao.FileName;
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/MatBang", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        chudao.CopyTo(stream);
                    }

                    mbModel.IMAGEPATH1 = "/uploads/MatBang/" + uniqueFileName;
                }

                // Xử lý tệp hình ảnh nội dung (noidung) và lưu đường dẫn vào IMAGEPATH2
                if (noidung != null && noidung.Count > 0)
                {
                    string noidungPaths = string.Empty;

                    foreach (var file in noidung)
                    {
                        if (file != null && file.Length > 0)
                        {
                            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/MatBang", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            // Thêm đường dẫn vào chuỗi IMAGEPATH2, ngăn chúng bằng dấu chấm phẩy
                            if (string.IsNullOrEmpty(noidungPaths))
                            {
                                noidungPaths = "/uploads/MatBang/" + uniqueFileName;
                            }
                            else
                            {
                                noidungPaths += ";" + "/uploads/MatBang/" + uniqueFileName;
                            }
                        }
                    }

                    mbModel.IMAGEPATH2 = noidungPaths;
                }

                // Lưu dữ liệu vào cơ sở dữ liệu
                await dbContext.MatBangs.AddAsync(mbModel);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("ThongTinMB", "Admin");
            }


            return View(mbModel);
        }



        // //Xu ly chuc nang thay doi trang thai : 
        // [HttpPost]
        // [ActionName("ChangeStatus")]
        // [ValidateAntiForgeryToken]
        // public async Task <IActionResult> ChangeStatus(int mamb)
        // {
        //     MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == mamb);
        //     if(matBang != null){
        //         bool check = matBang.TRANGTHAI;
        //         bool oppositeCheck = !check;
        //         matBang.TRANGTHAI = oppositeCheck;
        //         dbContext.MatBangs.Update(matBang);
        //         await dbContext.SaveChangesAsync();
        //         return RedirectToAction("ThongTinMB" , "Admin");
        //     }
        //     return RedirectToAction("TrangChu","Admin");
        // }


        //Thuc hien xoa mat bang : 
        [HttpPost]
        [ActionName("XoaMB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaMB(int? mamb){
            if(mamb != null){
                MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x=>x.MAMB == mamb);
                if(matBang != null){
                    // Xóa hình ảnh cũ nếu có
                    string relativePath = matBang.IMAGEPATH;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                    
                    // Xóa hình ảnh cũ nếu có
                    string relativePath1 = matBang.IMAGEPATH1;
                    string absolutePath1 = Path.Combine(webHostEnvironment.WebRootPath, relativePath1.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath1))
                    {
                        System.IO.File.Delete(absolutePath1);
                    }

                    // Tách các đường dẫn hình ảnh từ chuỗi IMAGEPATH2
                    string[] imagePaths = matBang.IMAGEPATH2.Split(';');

                    // Xóa từng hình ảnh trong mảng imagePaths
                    foreach (string imagePath in imagePaths)
                    {
                        string relativePath2 = imagePath.Trim(); // Loại bỏ khoảng trắng thừa
                        string absolutePath2 = Path.Combine(webHostEnvironment.WebRootPath, relativePath2.TrimStart('/'));

                        if (System.IO.File.Exists(absolutePath2))
                        {
                            System.IO.File.Delete(absolutePath2);
                        }
                    }

                    dbContext.MatBangs.Remove(matBang);
                    dbContext.SaveChanges();
                    return RedirectToAction("ThongTinMB" , "Admin");
                }
            }            

            return RedirectToAction("HomeAdmin" , "Admin");

        }




        //Chuc nang cap nhat MB : 
        [HttpGet]
        [ActionName("CapNhatMB")]
        public async Task<IActionResult> CapNhatMB(int? mamb){
            if(mamb != null){
                MatBang? matBang = dbContext.MatBangs.SingleOrDefault(x => x.MAMB == mamb);
                if(matBang != null){
                    return View(matBang);
                }
            }
            return RedirectToAction("HomeAdmin", "Admin");
        }



        [HttpPost]
        [ActionName("CapNhatMB")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CapNhatMB(MatBang model, IFormFile tieude, IFormFile chudao , List<IFormFile> noidung)
        {
            if (ModelState.IsValid)
            {   
                MatBang? existingMatBang =  dbContext.MatBangs.SingleOrDefault(x => x.MAMB == model.MAMB);

                if (existingMatBang == null)
                {
                    // Xử lý khi không tìm thấy Nhân Viên cần cập nhật
                    return NotFound();
                }

                // Cập nhật thông tin từ model vào Nhân Viên đã tồn tại
                existingMatBang.TENMB = model.TENMB;
                existingMatBang.DIENTICH = model.DIENTICH;
                existingMatBang.GIATIEN = model.GIATIEN;
                existingMatBang.VITRI = model.VITRI;
                existingMatBang.TRANGTHAI = model.TRANGTHAI;
                existingMatBang.MOTA = model.MOTA;


                // Kiểm tra xem có file hình ảnh mới được tải lên không
                string imagePath = ImagePath(tieude , existingMatBang.IMAGEPATH);
                string imagePath1 = ImagePath(chudao , existingMatBang.IMAGEPATH1);
                string imagePath2 = ImagePaths(noidung, existingMatBang.IMAGEPATH2);
                existingMatBang.IMAGEPATH = imagePath;
                existingMatBang.IMAGEPATH1 = imagePath1;
                existingMatBang.IMAGEPATH2 = imagePath2;

                dbContext.Update(existingMatBang);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("ThongTinMB", "Admin");
            }

            // Xử lý khi ModelState không hợp lệ (có lỗi nhập liệu)
            return View(model);
        }




        //Phuong Thuc xu ly anh : 
        private string ImagePath(IFormFile image , string imagepath){
            // Kiểm tra xem có file hình ảnh mới được tải lên không
                if (image != null && image.Length > 0)
                {
                    // Xử lý lưu đường dẫn đến hình ảnh mới vào thư mục uploads
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/MatBang");
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
                    string newImagePath = "/uploads/MatBang/" + uniqueFileName;
                    return newImagePath;
                }

            return imagepath;
        }

        public string ImagePaths(List<IFormFile> images, string existingImagePaths)
        {
            if (images != null && images.Count > 0)
            {
                // Thư mục lưu trữ hình ảnh
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/MatBang");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Xóa hình ảnh cũ nếu có
                string[] oldImagePaths = existingImagePaths.Split(';');

                foreach (string oldImagePath in oldImagePaths)
                {
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, oldImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }
                }

                string newImagePaths = string.Empty;

                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        // Thêm đường dẫn vào chuỗi IMAGEPATH2, ngăn chúng bằng dấu chấm phẩy
                        if (string.IsNullOrEmpty(newImagePaths))
                        {
                            newImagePaths = "/uploads/MatBang/" + uniqueFileName;
                        }
                        else
                        {
                            newImagePaths += ";" + "/uploads/MatBang/" + uniqueFileName;
                        }
                    }
                }

                return newImagePaths;
            }

            return existingImagePaths;
        }

        
    }
}