using QLTTTM.models;

namespace DAPM.Facade
{
    public class EmployeeFacade
    {
        private readonly DataSQLContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmployeeFacade(DataSQLContext context, IWebHostEnvironment _webHostEnvironment)
        {
            dbContext = context;
            webHostEnvironment = _webHostEnvironment;
        }

        public async Task AddEmployee(NhanVien nvModel, IFormFile file)
        {
            string uniqueFileName = UploadedFile(file);
            nvModel.AVATAR = uniqueFileName;
            await dbContext.NhanViens.AddAsync(nvModel);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateEmployee(NhanVien model, IFormFile newAvatar)
        {
            NhanVien? existingNhanVien = dbContext.NhanViens.SingleOrDefault(x => x.MANV == model.MANV);

            if (existingNhanVien == null)
            {
                throw new Exception("Nhân viên không tồn tại");
            }

            // Cập nhật thông tin từ model vào Nhân viên đã tồn tại
            existingNhanVien.HOTEN = model.HOTEN;
            existingNhanVien.GIOITINH = model.GIOITINH;
            existingNhanVien.SDT = model.SDT;
            existingNhanVien.NGAYSINH = model.NGAYSINH;
            existingNhanVien.CCCD = model.CCCD;
            existingNhanVien.DIACHI = model.DIACHI;
            existingNhanVien.EMAIL = model.EMAIL;
            existingNhanVien.NGAYVAOLAM = model.NGAYVAOLAM;
            existingNhanVien.MACV = model.MACV;

            // Kiểm tra xem có file hình ảnh mới được tải lên không
            if (newAvatar != null)
            {
                string uniqueFileName = UploadedFile(newAvatar);
                existingNhanVien.AVATAR = uniqueFileName;
            }

            dbContext.Update(existingNhanVien);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int? manv)
        {
            if (manv != null)
            {
                NhanVien? nhanVien = dbContext.NhanViens.SingleOrDefault(x => x.MANV == manv);
                if (nhanVien != null)
                {
                    dbContext.NhanViens.Remove(nhanVien);
                    dbContext.SaveChanges();
                }
            }
        }

        public async Task UpdateAccount(Account model)
        {
            Account? existingAccount = dbContext.Accounts.SingleOrDefault(x => x.USERNAME == model.USERNAME);

            if (existingAccount == null)
            {
                throw new Exception("Tài khoản không tồn tại");
            }
            
            // Cập nhật thông tin từ model vào Tài khoản đã tồn tại
            existingAccount.PASSWORDPASSWORD;

            dbContext.Update(existingAccount);
            await dbContext.SaveChangesAsync();
        }

        private string UploadedFile(IFormFile file)
        {
            string uniqueFileName = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }

}
