using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuNews.DAL
{
    public class FuNewsDbContextFactory : IDesignTimeDbContextFactory<FuNewsDbContext>
    {
        public FuNewsDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json") // <-- hoặc appsettings.Development.json nếu bạn dùng file này
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FuNewsDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new FuNewsDbContext(optionsBuilder.Options);
        }
    }
}
