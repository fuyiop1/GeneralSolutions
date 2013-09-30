using System.Web.Mvc;

namespace WebSite.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_defaultRoute",
                "Admin/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Login", id = UrlParameter.Optional },
                namespaces: new string[] { "WebSite.Areas.Admin.Controllers" }
            );
        }
    }
}
