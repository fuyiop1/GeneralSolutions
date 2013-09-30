using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebSite.Helpers
{
    public static class FileHelper
    {

        public static void DeleteFileAndEmptyDirectory(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                if (fileInfo.Directory.GetFiles().Count() == 0)
                {
                    fileInfo.Directory.Delete();
                }
            }
        }

        public static void CopyFile(string targetFileFullPath, string orginalFilePath)
        {
            var fileInfo = new FileInfo(targetFileFullPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            var orginalFile = new FileInfo(orginalFilePath);
            if (orginalFile.Exists)
            {
                orginalFile.CopyTo(targetFileFullPath, true);
                DeleteFileAndEmptyDirectory(orginalFilePath);
            }
            
        }

        public static string ExtractFileName(string pathOrUrl)
        {
            var name = pathOrUrl;
            var spliterIndex = name.LastIndexOf("-");
            if (spliterIndex > -1)
            {
                var startIndex = name.LastIndexOfAny(new char[] { '/', '\\' }) + 1;
                name = name.Substring(startIndex, spliterIndex - startIndex);
            }
            return name;
        }
    }
}