using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data
{
    public class ApplicationDBContext : IdentityDbContext<UserInfo>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<Core.Models.Cafe> Cafe { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<Core.Models.CafeComment> CafeComments { get; set; }


    }


    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
    {
        public ApplicationDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(@Directory.GetCurrentDirectory() + "/../CafeJWTMVC/appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<ApplicationDBContext>();

            var connectionString = configuration.GetConnectionString("ApplicationDBContext");
            builder.UseSqlServer(connectionString);
            return new ApplicationDBContext(builder.Options);
        }

    }

}
