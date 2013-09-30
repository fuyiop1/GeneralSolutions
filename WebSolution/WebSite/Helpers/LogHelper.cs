using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite.Helpers
{
    public static class LogHelper
    {
        public static string LogFilePath { get; set; }

        public static void WriteLog(string text)
        {
            if (!System.IO.File.Exists(LogFilePath))
            {
                using (var fileStream = System.IO.File.Create(LogFilePath))
                {
                }
            }
            using (var fileWriter = new System.IO.StreamWriter(LogFilePath, true))
            {
                fileWriter.WriteLine(string.Format("{0}: {1}", DateTime.Now.ToDefaultTargetGmtTime(), text));
            }
        }
    }
}