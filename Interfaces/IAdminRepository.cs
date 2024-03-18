using QLTTTM.models;

namespace DAPM.Interfaces
{
    public interface IAdminRepository
    {
        public NhanVien GetNhanVienById(int manv);
        public ChucVu? GetChucVuById(int macv);
        public PhanQuyen? GetPhanQuyenById(int maquyen);
        public List<MatBang> GetMatBangs();
        public List<SuKien> GetSuKiens();
        public Account? Login(string username, string password);
        public bool AddQuyen(PhanQuyen phanQuyen);
        public bool DeleteQuyen(int id);
        public bool AddChucVu(ChucVu chucVu);
        public bool DeleteChucVu(int id);
        public bool SaveChanges();
    }
}
