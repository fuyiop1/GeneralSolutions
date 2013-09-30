using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using MvcFileUploader.Models;
using MvcFileUploader;
using WebSite.Helpers;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace WebSite.Areas.Admin.Controllers
{
    public class FileUploadController : Controller
    {
        public string UploadFile(int? entityId) // optionally receive values specified with Html helper
        {
            // here we can send in some extra info to be included with the delete url 
            var statuses = new List<ViewDataUploadFileResult>();
            var storageDirectory = string.Format("/BizContents/tmp");
            for (var i = 0; i < Request.Files.Count; i++)
            {
                var st = FileSaver.StoreFile(x =>
                {
                    x.File = Request.Files[i];
                    //note how we are adding an additional value to be posted with delete request
                    //and giving it the same value posted with upload
                    x.DeleteUrl = Url.Action("DeleteFile", new { entityId = entityId });
                    x.StorageDirectory = Server.MapPath("~" + storageDirectory);
                    if (!Directory.Exists(x.StorageDirectory))
                    {
                        Directory.CreateDirectory(x.StorageDirectory);
                    }
                    x.UrlPrefix = storageDirectory;


                    //overriding defaults
                    //x.FileName = Request.Files[i].FileName;// default is filename suffixed with filetimestamp
                    //x.ThrowExceptions = true;//default is false, if false exception message is set in error property
                });

                statuses.Add(st);
            }

            //statuses contains all the uploaded files details (if error occurs then check error property is not null or empty)
            //todo: add additional code to generate thumbnail for videos, associate files with entities etc

            //adding thumbnail url for jquery file upload javascript plugin
            statuses.ForEach(x => x.thumbnail_url = x.url + "?width=80&height=80"); // uses ImageResizer httpmodule to resize images from this url

            //setting custom download url instead of direct url to file which is default
            statuses.ForEach(x => x.url = Url.Action("DownloadFile", new { fileUrl = x.url, mimetype = x.type }));
            return JsonConvert.SerializeObject(new { files = statuses });
        }

        //here i am receving the extra info injected
        [HttpPost] // should accept only post
        public ActionResult DeleteFile(int? entityId, string fileUrl)
        {
            var filePath = Server.MapPath("~" + fileUrl);

            FileHelper.DeleteFileAndEmptyDirectory(filePath);

            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            return new HttpStatusCodeResult(200); // trigger success
        }


        public ActionResult DownloadFile(string fileUrl, string mimetype)
        {
            var filePath = Server.MapPath("~" + fileUrl);

            if (System.IO.File.Exists(filePath))
                return File(filePath, mimetype);
            else
            {
                return new HttpNotFoundResult("Fiel not found");
            }
        }

        public ActionResult CropImageCanvas(string imageSrc)
        {
            ViewBag.ImageSrc = imageSrc;
            var imagePath = Server.MapPath("~" + imageSrc);
            using (var image = Image.FromFile(imagePath))
            {
                ViewBag.ImageSrcForDisPlay = string.Format("{0}?width={1}&height={2}", imageSrc, image.Width, image.Height);
                ViewBag.ImageWidth = image.Width;
                ViewBag.ImageHeight = image.Height;
            }
            return PartialView();
        }

        public string CropImage(string imageUrl, double width, double height, double x, double y)
        {
            if (width > 0 && height > 0)
            {
                var imagePath = Server.MapPath("~" + imageUrl);
                var image = Image.FromFile(imagePath);
                var bmp = new Bitmap(Convert.ToInt32(width), Convert.ToInt32(height), PixelFormat.Format24bppRgb);
                bmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                using (var graphic = Graphics.FromImage(bmp))
                {
                    graphic.SmoothingMode = SmoothingMode.AntiAlias;
                    graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphic.DrawImage(image, new Rectangle(0, 0, Convert.ToInt32(width), Convert.ToInt32(height)), Convert.ToInt32(x), Convert.ToInt32(y), Convert.ToInt32(width), Convert.ToInt32(height), GraphicsUnit.Pixel);
                }
                image.Dispose();
                bmp.Save(imagePath);
            }
            return imageUrl;
        }

        public string ConfirmImageSize(string images, int minWidth, int minHeight)
        {
            var result = new ImageSizeResponse()
            {
                MinWidth = minWidth,
                MinHeight = minHeight,
                ImageSizeResponseItems = new List<ImageSizeResponseItem>()
            };
            var filteredImageList = new List<string>();
            if (!string.IsNullOrWhiteSpace(images) && (minWidth > 0 || minHeight > 0))
            {
                var imagePaths = images.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in imagePaths)
                {
                    using (var image = Image.FromFile(Server.MapPath("~" + item)))
                    {
                        if (image.Width < minWidth || image.Height < minHeight)
                        {
                            result.ImageSizeResponseItems.Add(new ImageSizeResponseItem()
                            {
                                Url = item,
                                Width = image.Width,
                                Height = image.Height,
                                FileName = FileHelper.ExtractFileName(item)
                            });
                        }
                        else
                        {
                            filteredImageList.Add(item);
                        }
                    }
                }
            }
            result.IsValid = result.ImageSizeResponseItems.Count == 0;
            result.FileNames = string.Join(",", result.ImageSizeResponseItems.Select(x => x.FileName).ToArray());
            result.FilteredImages = string.Join("|", filteredImageList.ToArray());
            var resultJson = JsonConvert.SerializeObject(result);
            return resultJson;
        }

    }

    public class ImageSizeResponse
    {
        public bool IsValid { get; set; }
        public int MinWidth { get; set; }
        public int MinHeight { get; set; }
        public string FileNames { get; set; }
        public string FilteredImages { get; set; }
        public List<ImageSizeResponseItem> ImageSizeResponseItems { get; set; }
    }

    public class ImageSizeResponseItem
    {
        public string Url { get; set; }
        public string FileName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

}