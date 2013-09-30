using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using WebSite.Helpers;
using System.Globalization;
using WebSite.MyDbContext.Entities;

namespace WebSite.Areas.Admin.Controllers
{
    public abstract class AbstractController : Controller
    {
        protected MyDbContext.MyDbContext DbContext = new MyDbContext.MyDbContext();

        private int pageSize;

        protected int PageSize
        {
            get
            {
                if (this.pageSize == 0)
                {
                    ProcessPageSize();
                }
                return this.pageSize;
            }
        }

        private int pageIndex;

        protected int PageIndex
        {
            get
            {
                if (this.pageIndex == 0)
                {
                    this.pageIndex = ProtoTypeHelper.TryToParseInt(Request["pageIndex"], 1);
                }
                if (this.pageIndex < 1)
                {
                    this.pageIndex = 1;
                }
                return this.pageIndex;
            }
        }

        private bool? isPublished;

        protected bool? IsPublished
        {
            get
            {
                if (this.isPublished == null)
                {
                    var isPublishedString = Request["isPublished"];
                    if (string.IsNullOrWhiteSpace(isPublishedString))
                    {
                        this.isPublished = null;
                    }
                    else
                    {
                        if (isPublishedString.ToLower() == "true")
                        {
                            this.isPublished = true;
                        }
                        else if (isPublishedString.ToLower() == "false")
                        {
                            this.isPublished = false;
                        }
                    }
                }
                return this.isPublished;
            }
        }

        protected int PageSkipCount(int pageIndex = 0)
        {
            var result = 0;
            if (pageIndex < 1)
            {
                pageIndex = PageIndex;
            }
            if (pageIndex > 1)
            {
                result = (pageIndex - 1) * PageSize;
            }
            return result;
        }

        protected void RefreshSiteMap()
        {
            MvcSiteMapProvider.SiteMaps.ReleaseSiteMap();
        }

        private void ProcessPageSize()
        {

            var cookieName = string.Empty;
            var defaultPageSize = 10;
            var finalPageSize = defaultPageSize;

            //if (this is AlbumController)
            //{
            //    cookieName = ConstHelper.PageSizeFilmCookie;
            //}

            var pageSizeFromRequest = ProtoTypeHelper.TryToParseInt(Request["pageSize"]);

            var pageSizeFromCookie = 0;
            var pageSizeCookie = Request.Cookies[cookieName];
            if (pageSizeCookie != null)
            {
                pageSizeFromCookie = ProtoTypeHelper.TryToParseInt(Request.Cookies[cookieName].Value);
            }

            if (pageSizeFromRequest != 0)
            {
                finalPageSize = pageSizeFromRequest;
            }
            else
            {
                if (pageSizeFromCookie != 0)
                {
                    finalPageSize = pageSizeFromCookie;
                }
            }

            if (pageSizeFromCookie != finalPageSize && !string.IsNullOrEmpty(cookieName))
            {
                Response.Cookies.Add(new HttpCookie(cookieName, finalPageSize.ToString()));
            }

            this.pageSize = finalPageSize;

        }

        protected string SaveFile(int id, string fileRelativePath, string type, string subType)
        {
            var targetRelativeUrl = Path.Combine("/BizContents", type, id.ToString(), subType);
            var targetDirectory = Server.MapPath("~" + targetRelativeUrl);

            var extension = string.Empty;
            var entensionSpliterIndex = fileRelativePath.LastIndexOf('.');

            if (entensionSpliterIndex > -1 && entensionSpliterIndex < fileRelativePath.Length - 1)
            {
                extension = fileRelativePath.Substring(entensionSpliterIndex);
            }

            var targetFileName = Guid.NewGuid().ToString().Replace("-", string.Empty) + extension;
            targetRelativeUrl = Path.Combine(targetRelativeUrl, targetFileName);
            targetRelativeUrl = targetRelativeUrl.Replace('\\', '/');
            var targetFileFullPath = Path.Combine(targetDirectory, targetFileName);

            var originalFile = Server.MapPath("~" + fileRelativePath);

            FileHelper.CopyFile(targetFileFullPath, originalFile);
            return targetRelativeUrl;
        }

        protected void DeleteFileAndEmptyDirectory(string file)
        {
            var fileFullPath = Server.MapPath("~" + file);
            FileHelper.DeleteFileAndEmptyDirectory(fileFullPath);
        }

        protected void DeleteItemDirectory(int id, string type)
        {
            var itemRelativeDirectory = Path.Combine("/BizContents", type, id.ToString());
            var itemDirectory = Server.MapPath("~" + itemRelativeDirectory);
            DeleteFolder(itemDirectory);
        }

        private void DeleteFolder(string dir)
        {
            var direcotry = new DirectoryInfo(dir);
            if (direcotry.Exists) //如果存在这个文件夹删除之 
            {
                foreach (var f in direcotry.GetFiles())
                {
                    f.Delete();
                }
                foreach (var d in direcotry.GetDirectories())
                {
                    DeleteFolder(d.FullName);
                }
                direcotry.Delete(true);
                System.Threading.Thread.Sleep(10);
            }
            else if (System.IO.File.Exists(dir))
            {
                System.IO.File.Delete(dir);
            }
        }

        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            return base.BeginExecute(requestContext, callback, state);
        }

        protected void ModelTop(IQueryable<SortableEntity> list, SortableEntity model)
        {
            if (model != null)
            {
                var target = list
                    .OrderBy(x => x.TimeForSort).FirstOrDefault();
                if (target.Id != model.Id)
                {
                    var temp = target.TimeForSort;
                    target.TimeForSort = model.TimeForSort;
                    model.TimeForSort = temp;
                    DbContext.SaveChanges();
                }
            }
        }

        protected void ModelUp(IQueryable<SortableEntity> list, SortableEntity model)
        {
            var minTime = list.Min(x => x.TimeForSort);
            if (model != null && model.TimeForSort > minTime)
            {
                var target = list.Where(x => x.TimeForSort < model.TimeForSort)
                    .OrderByDescending(x => x.TimeForSort).FirstOrDefault();
                var temp = target.TimeForSort;
                target.TimeForSort = model.TimeForSort;
                model.TimeForSort = temp;
                DbContext.SaveChanges();
            }
        }

        protected void ModelDown(IQueryable<SortableEntity> list, SortableEntity model)
        {
            var maxTime = list.Max(x => x.TimeForSort);
            if (model != null && model.TimeForSort < maxTime)
            {
                var target = list.Where(x => x.TimeForSort > model.TimeForSort)
                    .OrderBy(x => x.TimeForSort).FirstOrDefault();
                var temp = target.TimeForSort;
                target.TimeForSort = model.TimeForSort;
                model.TimeForSort = temp;
                DbContext.SaveChanges();
            }
        }

        protected void ModelBottom(SortableEntity model)
        {
            model.TimeForSort = DateTime.Now.ToDefaultTargetGmtTime();
            DbContext.SaveChanges();
        }

    }
}
