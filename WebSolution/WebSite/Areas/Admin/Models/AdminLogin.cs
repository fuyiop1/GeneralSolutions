using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Areas.Admin.Models
{
    public class AdminLogin
    {
        public string ReturnUrl { get; set; }
        public MyDbContext.Entities.Admin Admin { get; set; }
        public bool IsRememberAccount { get; set; }
    }
}