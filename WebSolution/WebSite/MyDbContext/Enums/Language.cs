using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace WebSite.MyDbContext.Enums
{
    public enum Language
    {
        [Description("EN")]
        English,

        [Description("FR")]
        French,

        [Description("NL")]
        Netherlands
    }
}