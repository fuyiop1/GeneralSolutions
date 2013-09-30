using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace WebSite.Areas.Admin.Models
{
    public interface IPager
    {

        int TotalCount { get; set; }
        int PageIndex { get; set; }
        int PageSize { get; set; }

        string Controller { get; set; }
        string Action { get; set; }
        RouteValueDictionary RouteValueDic { get; set; }

        int DisplayPageStartIndex
        {
            get;
        }

        int DisplayPageEndIndex
        {
            get;
        }

        int PreviousPageIndex
        {
            get;
        }

        int NextPageIndex
        {
            get;
        }


        int PageCount
        {
            get;
        }

        void ChangePageIndex(int pageIndex);

        void ProcessQueryString(string queryString);
    }
}
