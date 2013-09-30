using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Configuration;
using WebSite.MyDbContext.Entities;

namespace WebSite.MyDbContext
{
    public class MyDbContext : DbContext
    {
        static MyDbContext()
        {
            var isAutoUpdateDatabase = ConfigurationManager.AppSettings["IsAutoUpdateDatabase"];
            if (isAutoUpdateDatabase == "true")
            {
                Database.SetInitializer(new MyDropCreateDatabaseIfModelChanges());
            }
            else
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<MyDbContext>());
            }
        }
        public DbSet<Admin> Admins { get; set; }
    }
}