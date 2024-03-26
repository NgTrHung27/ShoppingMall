using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using QLTTTM.Datas;
using QLTTTM.models;
using System.Threading.Tasks;

namespace DAPM.Services
{
    public class EventServices : IFactoryServices<SuKien>
    {
        private readonly DataSQLContext _context = SingletonDbContext.Instance;
        List<SuKien> suKiens = new List<SuKien>();
        private readonly IWebHostEnvironment webHostEnvironment;


        public void Add(SuKien entity, IFormFile? image, IWebHostEnvironment webHostEnvironment, IFormFile? tieude, IFormFile? chudao, List<IFormFile>? noidung)
        {
            (entity.IMAGEPATH, entity.IMAGEPATH1, entity.IMAGEPATH2) = InsertImage(tieude, chudao, noidung);
            // Lưu dữ liệu vào cơ sở dữ liệu
            _context.SuKiens.Add(entity);
            _context.SaveChanges();

            //InsertImage method           
            (string h, string h1, string h2) InsertImage(IFormFile tieude, IFormFile chudao, List<IFormFile> noidung)
            {

                string h = "defaultsk.png";
                string h1 = "defaultsk.png";
                string h2 = "defaultsk.png";

                // Xử lý tệp hình ảnh tiêu đề (tieude) và lưu đường dẫn vào IMAGEPATH
                if (tieude != null && tieude.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + tieude.FileName;
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/SuKien", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        tieude.CopyTo(stream);
                    }

                    h = "/uploads/SuKien/" + uniqueFileName;
                }

                // Xử lý tệp hình ảnh chủ đạo (chudao) và lưu đường dẫn vào IMAGEPATH1
                if (chudao != null && chudao.Length > 0)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + chudao.FileName;
                    string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/SuKien", uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        chudao.CopyTo(stream);
                    }

                    h1 = "/uploads/SuKien/" + uniqueFileName;
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
                            string filePath = Path.Combine(webHostEnvironment.WebRootPath, "uploads/SuKien", uniqueFileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(stream);
                            }

                            // Thêm đường dẫn vào chuỗi IMAGEPATH2, ngăn chúng bằng dấu chấm phẩy
                            if (string.IsNullOrEmpty(noidungPaths))
                            {
                                noidungPaths = "/uploads/SuKien/" + uniqueFileName;
                            }
                            else
                            {
                                noidungPaths += ";" + "/uploads/SuKien/" + uniqueFileName;
                            }
                        }
                    }

                    h2 = noidungPaths;
                }
                return (h, h1, h2);

            }
        }

        public void Delete(int? id, IWebHostEnvironment webHostEnvironment)
        {
            if (id != null)
            {
                SuKien? suKien = _context.SuKiens.SingleOrDefault(x => x.MASK == id);
                if (suKien != null)
                {
                    // Xóa hình ảnh cũ nếu có
                    string relativePath = suKien.IMAGEPATH;
                    string absolutePath = Path.Combine(webHostEnvironment.WebRootPath, relativePath.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath))
                    {
                        System.IO.File.Delete(absolutePath);
                    }

                    // Xóa hình ảnh cũ nếu có
                    string relativePath1 = suKien.IMAGEPATH1;
                    string absolutePath1 = Path.Combine(webHostEnvironment.WebRootPath, relativePath1.TrimStart('/'));

                    if (System.IO.File.Exists(absolutePath1))
                    {
                        System.IO.File.Delete(absolutePath1);
                    }

                    // Tách các đường dẫn hình ảnh từ chuỗi IMAGEPATH2
                    string[] imagePaths = suKien.IMAGEPATH2.Split(';');

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

                    _context.SuKiens.Remove(suKien);
                    _context.SaveChanges();
                }
            }
        }

        public void Update(SuKien entity, IFormFile? tieude, IFormFile chudao, List<IFormFile> noidung, IWebHostEnvironment webHostEnvironment)
        {
            SuKien? existingSuKien = _context.SuKiens.SingleOrDefault(x => x.MASK == entity.MASK);

            if (existingSuKien == null)
            {
                // Xử lý khi không tìm thấy Nhân Viên cần cập nhật
                existingSuKien = _context.SuKiens.SingleOrDefault(x => x.MASK == entity.MASK);
            }
                
                // Cập nhật thông tin từ model vào Nhân Viên đã tồn tại
                existingSuKien.TENSK = entity.TENSK;
                existingSuKien.LOAISK = entity.LOAISK;
                existingSuKien.THOIGIAN = entity.THOIGIAN;
                existingSuKien.VITRI = entity.VITRI;
                existingSuKien.TRANGTHAI = entity.TRANGTHAI;
                existingSuKien.MOTA = entity.MOTA;


                // Kiểm tra xem có file hình ảnh mới được tải lên không
                string imagePath = ImagePath(tieude, existingSuKien.IMAGEPATH);
                string imagePath1 = ImagePath(chudao, existingSuKien.IMAGEPATH1);
                string imagePath2 = ImagePaths(noidung, existingSuKien.IMAGEPATH2);
                existingSuKien.IMAGEPATH = imagePath;
                existingSuKien.IMAGEPATH1 = imagePath1;
                existingSuKien.IMAGEPATH2 = imagePath2;

                _context.Update(existingSuKien);
                _context.SaveChanges();

                //Phuong Thuc xu ly anh : 
                string ImagePath(IFormFile image, string imagepath)
                {
                    // Kiểm tra xem có file hình ảnh mới được tải lên không
                    if (image != null && image.Length > 0)
                    {
                        // Xử lý lưu đường dẫn đến hình ảnh mới vào thư mục uploads
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/SuKien");
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
                        string newImagePath = "/uploads/SuKien/" + uniqueFileName;
                        return newImagePath;
                    }

                    return imagepath;
                }
                string ImagePaths(List<IFormFile> images, string existingImagePaths)
                {
                    if (images != null && images.Count > 0)
                    {
                        // Thư mục lưu trữ hình ảnh
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads/SuKien");
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
                                    newImagePaths = "/uploads/SuKien/" + uniqueFileName;
                                }
                                else
                                {
                                    newImagePaths += ";" + "/uploads/SuKien/" + uniqueFileName;
                                }
                            }
                        }
                        return newImagePaths;
                    }
                    return existingImagePaths;
                };
            }
    }
}
