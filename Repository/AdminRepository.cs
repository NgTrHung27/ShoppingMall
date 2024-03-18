using DAPM.Interfaces;
using QLTTTM.Datas;
using QLTTTM.models;
using System.Data.Entity;

namespace DAPM.Repository
{
    public class AdminRepository : IAdminRepository
    {
        DataSQLContext _context = SingletonDbContext.Instance;

        public bool AddChucVu(ChucVu chucVu)
        {
            _context.ChucVus.Add(chucVu);

            return SaveChanges();
        }

        public bool AddQuyen(PhanQuyen phanQuyen)
        {
            _context.PhanQuyens.Add(phanQuyen);

            return SaveChanges();
        }

        public bool DeleteChucVu(int id)
        {
            _context.ChucVus.Remove(_context.ChucVus.Find(id));

            return SaveChanges();
        }

        public bool DeleteQuyen(int id)
        {
            _context.PhanQuyens.Remove(_context.PhanQuyens.Find(id));

            return SaveChanges();
        }

        public ChucVu? GetChucVuById(int macv)
        {
            return  _context.ChucVus.Where(x => x.MACV == macv).FirstOrDefault();
        }

        public List<MatBang> GetMatBangs()
        {
            return  _context.MatBangs.ToList();
        }

        public NhanVien? GetNhanVienById(int manv)
        {
            return  _context.NhanViens.FirstOrDefault(x => x.MANV == manv);
        }

        public PhanQuyen? GetPhanQuyenById(int maquyen)
        {
            return _context.PhanQuyens.FirstOrDefault(x => x.ID == maquyen);
        }

        public List<SuKien> GetSuKiens()
        {
            return  _context.SuKiens.ToList();
        }

        public Account? Login(string username, string password)
        {
            return  _context.Accounts.FirstOrDefault(x => x.TAIKHOAN == username && x.MATKHAU == password);
        }

        public bool SaveChanges()
        {
            var saved = _context.SaveChanges();

            return saved > 0 ? true : false;
        }
    }
}
