using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected MyDbContext.MyDbContext DbContext = new MyDbContext.MyDbContext();
    }
}
