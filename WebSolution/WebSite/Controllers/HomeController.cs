using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace WebSite.Controllers
{
    public class HomeController : AbstractController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
