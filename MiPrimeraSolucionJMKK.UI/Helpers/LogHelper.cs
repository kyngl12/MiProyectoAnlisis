using System;
using System.IO;
using System.Web.Hosting;

namespace MiPrimeraSolucionJMKK.UI.Helpers
{
    public static class LogHelper
    {
        private static string GetLogPath()
        {
            var dir = HostingEnvironment.MapPath("~/App_Data");
            if (dir == null) dir = AppDomain.CurrentDomain.BaseDirectory + "App_Data";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return Path.Combine(dir, "errors.log");
        }

        public static void Log(Exception ex)
        {
            try
            {
                var path = GetLogPath();
                File.AppendAllText(path, DateTime.Now.ToString("s") + " - Exception: " + ex.ToString() + Environment.NewLine);
            }
            catch { }
        }

        public static void LogMessage(string message)
        {
            try
            {
                var path = GetLogPath();
                File.AppendAllText(path, DateTime.Now.ToString("s") + " - Message: " + message + Environment.NewLine);
            }
            catch { }
        }
    }
}
