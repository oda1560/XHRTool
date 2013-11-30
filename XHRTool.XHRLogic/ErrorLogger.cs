using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XHRTool.XHRLogic
{
    public static class ErrorLogger
    {
        public static void WriteLog(string logEntry)
        {
            try
            {
                File.AppendAllText("ErrorLog.txt", string.Format("{0} {1}{2}", DateTime.Now.ToString(), logEntry, Environment.NewLine));
            }
            catch
            {
                
            }
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog(ex.ToString());
        }
    }
}
