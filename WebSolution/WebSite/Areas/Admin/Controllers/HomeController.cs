using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSite.Areas.Admin.Models;
using WebSite.Helpers;
using System.Web.Security;

namespace WebSite.Areas.Admin.Controllers
{
    public class HomeController : AbstractController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            var viewModel = new AdminLogin();
            viewModel.ReturnUrl = Request["ReturnUrl"];
            var admin = new WebSite.MyDbContext.Entities.Admin();

            var rememberedAccountCookie = Request.Cookies[ConstHelper.RememerAccountCookieName];
            if (rememberedAccountCookie != null && !string.IsNullOrEmpty(rememberedAccountCookie.Value))
            {
                admin.UserName = rememberedAccountCookie.Value;
                viewModel.IsRememberAccount = true;
            }

            viewModel.Admin = admin;
            ViewBag.IsAdminLoginPage = true;
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Login(AdminLogin viewModel)
        {
            if (ModelState.IsValid)
            {
                var admin = DbContext.Admins.FirstOrDefault(x => x.UserName == viewModel.Admin.UserName);
                if (admin != null)
                {
                    if (admin.Password == viewModel.Admin.Password)
                    {
                        FormsAuthentication.SetAuthCookie(admin.UserName, false);

                        var newRememberedAccountCookie = new HttpCookie(ConstHelper.RememerAccountCookieName, admin.UserName);
                        newRememberedAccountCookie.Expires = DateTime.Now.AddMonths(1);
                        if (viewModel.IsRememberAccount)
                        {
                            Response.Cookies.Remove(ConstHelper.RememerAccountCookieName);
                            Response.Cookies.Add(newRememberedAccountCookie);
                        }
                        else
                        {
                            Response.Cookies.Remove(ConstHelper.RememerAccountCookieName);
                            newRememberedAccountCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(newRememberedAccountCookie);
                        }
                        if (string.IsNullOrWhiteSpace(viewModel.ReturnUrl))
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            return Redirect(viewModel.ReturnUrl);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Admin.Password", "Incorrect password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Admin.UserName", "Invalid username");
                }
            }
            ViewBag.IsAdminLoginPage = true;
            return View(viewModel);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}
