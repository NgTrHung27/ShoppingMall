using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QLTTTM.models;


namespace QLTTTM.Datas
{
    // Singleton
    public class SingletonDbContext
    {
        private static DataSQLContext instance = null;

        public static DataSQLContext Instance
        {
            get
            {
                if (instance == null)
                {
                    var options = new DbContextOptionsBuilder<DataSQLContext>()
                        .UseSqlServer("Server=LAPTOP-FDR7M9C3;Database=QLTTTM;User ID=SA;Password=123456aA@$;Encrypt=true;TrustServerCertificate=true;Integrated Security=True;MultipleActiveResultSets=true;")
                        .Options;

                    instance = new DataSQLContext(options);
                }
                return instance;
            }
        }

        private SingletonDbContext() { }
    }
}