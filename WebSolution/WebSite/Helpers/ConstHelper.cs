using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace WebSite.Helpers
{
    public static class ConstHelper
    {
        public static readonly string RememerAccountCookieName = "TheRestoGuideAdminRemeberedAccount";
        public static readonly int PageIndexOffset;
        public const int PageDefaultSize = 10;
        public static readonly string PageSizeFilmCookie = "PageSizeAlbumCookie";

        public static readonly int AlbumFetchSize = 6;

        public static readonly string WebSiteDefaultTitle = "The Resto Guide";

        public static readonly string DirectorNamePlaceHolder = "DirectorNamePlaceHolder";
        public static readonly string ProducerNamePlaceHolder = "ProducerNamePlaceHolder";

        static ConstHelper()
        {
            var pageIndexOffset = 0;
            int.TryParse(ConfigurationManager.AppSettings["PageIndexOffset"], out pageIndexOffset);
            if (pageIndexOffset == 0)
            {
                pageIndexOffset = 3;
            }
            PageIndexOffset = pageIndexOffset;
            
        }
    }
}