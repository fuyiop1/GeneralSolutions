using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace WebSite.Areas.Admin.Models
{
    public abstract class PagerBase : IPager
    {

        private int pageIndexOffset = 3;
        private int pageDefaultSize = 10;
        private string pageIndexRouteKey = "pageIndex";
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public string Controller { get; set; }
        public string Action { get; set; }
        public RouteValueDictionary RouteValueDic { get; set; }

        public int DisplayPageStartIndex
        {
            get
            {
                var makeUpCount = 0;
                if (PageCount - PageIndex < pageIndexOffset)
                {
                    makeUpCount = pageIndexOffset - (PageCount - PageIndex);
                }
                var expectedStartIndex = PageIndex - pageIndexOffset - makeUpCount;
                return expectedStartIndex < 1 ? 1 : expectedStartIndex;
            }
        }

        public int DisplayPageEndIndex
        {
            get
            {
                var expectedEndIndex = DisplayPageStartIndex + pageIndexOffset * 2;
                return expectedEndIndex > PageCount ? PageCount : expectedEndIndex;
            }
        }

        public int PreviousPageIndex
        {
            get
            {
                return PageIndex - 1 < 1 ? 1 : PageIndex - 1;
            }
        }

        public int NextPageIndex
        {
            get
            {
                return PageIndex + 1 > PageCount ? PageCount : PageIndex + 1;
            }
        }

        public int PageCount
        {
            get
            {
                if (TotalCount <= 0 || PageSize <= 0)
                {
                    return -1;
                }

                return (TotalCount % PageSize == 0) ? (TotalCount / PageSize) : (TotalCount / PageSize + 1);
            }
        }

        public PagerBase()
        {
            PageSize = pageDefaultSize;
            PageIndex = 1;
            RouteValueDic = new RouteValueDictionary();
        }

        public void ChangePageIndex(int pageIndex)
        {
            if (RouteValueDic != null)
            {
                if (RouteValueDic.ContainsKey(pageIndexRouteKey))
                {
                    RouteValueDic.Remove(pageIndexRouteKey);
                }
                RouteValueDic.Add(pageIndexRouteKey, pageIndex);
            }
        }


        public void ProcessQueryString(string queryString)
        {
            var spliter = new char[] { '?', '&' };
            var keyValueSpliter = new char[] { '=' };
            if (!string.IsNullOrEmpty(queryString) && RouteValueDic != null)
            {
                foreach (var keyValueItem in queryString.Split(spliter, StringSplitOptions.RemoveEmptyEntries))
                {
                    var keyValues = keyValueItem.Split(keyValueSpliter);
                    var key = keyValues.FirstOrDefault();
                    var value = string.Empty;
                    if (keyValues.Length > 1)
                    {
                        value = keyValues[1];
                    }
                    if (!string.IsNullOrWhiteSpace(key) && !RouteValueDic.ContainsKey(key))
                    {
                        RouteValueDic.Add(key, value);
                    }
                }
            }
        }
    }
}
