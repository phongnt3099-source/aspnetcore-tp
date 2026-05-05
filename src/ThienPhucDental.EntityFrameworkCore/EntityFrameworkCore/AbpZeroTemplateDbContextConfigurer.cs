using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ThienPhucDental.EntityFrameworkCore
{
    public static class ThienPhucDentalDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ThienPhucDentalDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<ThienPhucDentalDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}