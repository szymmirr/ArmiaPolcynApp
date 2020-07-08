using System;
using System.Diagnostics;

namespace AppStudio
{
    public class AppLogs
    {
        static public void WriteError(string source, Exception exception)
        {
            WriteLog(source, exception.ToString(), "Error");
        }

        static public void WriteError(string source, string message)
        {
            WriteLog(source, message, "Error");
        }

        static public void WriteWarning(string source, string message)
        {
            WriteLog(source, message, "Warning");
        }

        static public void WriteInfo(string source, string message)
        {
            WriteLog(source, message, "Info");
        }

        static public void WriteLog(string source, string message, string messageType)
        {
            try
            {
                string logMessage = String.Format("{0} {1}\r\n{2}: {3}\r\n", DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), messageType, source, message);
                UserStorage.WriteLineToFile("AppLogs.txt", logMessage);
            }
            catch { /* Avoid any exception at this point. */ }
        }

        static public void Clear()
        {
            try
            {
                UserStorage.Delete("AppLogs.txt");
            }
            catch { /* Avoid any exception at this point. */ }
        }
    }
}
