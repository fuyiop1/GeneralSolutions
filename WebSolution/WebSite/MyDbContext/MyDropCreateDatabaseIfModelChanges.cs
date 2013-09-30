using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using WebSite.MyDbContext.Entities;

namespace WebSite.MyDbContext
{
    public class MyDropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<MyDbContext>
    {
        protected override void Seed(MyDbContext context)
        {
            base.Seed(context);
            context.Admins.Add(new Admin()
            {
                UserName = "admin",
                Password = "123qwe"
            });
            context.SaveChanges();
        }
    }
}