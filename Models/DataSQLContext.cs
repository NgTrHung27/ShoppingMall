using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;


namespace QLTTTM.models{
    public class DataSQLContext : DbContext{

        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<HopDong> HopDongs { get; set; }
        public DbSet<ChucVu> ChucVus { get; set; }
        public DbSet<SuKien> SuKiens { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<DoiTac> DoiTacs { get; set; }
        public DbSet<LoaiDoiTac> LoaiDoiTacs { get; set; }
        public DbSet<HopDongDoiTac> HopDongDoiTacs { get; set; }
        public DbSet<HopDongMatBang> HopDongMatBangs { get; set; }
        public DbSet<MatBang> MatBangs { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<ChucNang> ChucNangs { get; set; }
        public DbSet<PhanQuyen> PhanQuyens { get; set; }
        


        public DataSQLContext(DbContextOptions<DataSQLContext> options):base(options){
            
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // //Bo ten tien to ASP.NET cua db tich hop : 
            // foreach(var entitytype in modelBuilder.Model.GetEntityTypes()){
            //     var tableName = entitytype.GetTableName();
            //     if(tableName.StartsWith("AspNet")){
            //         entitytype.SetTableName(tableName.Substring(6));
            //     }
            // }
            
        }




    }
}