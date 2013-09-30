using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebSite.MyDbContext.Entities
{
    public class Admin
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        public virtual string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public virtual string Password { get; set; }
    }
}