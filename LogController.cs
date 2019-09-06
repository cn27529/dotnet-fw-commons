using System;
using System.IO;
using System.Configuration;
using System.Web;
//using System.Web.UI;

namespace Commons
{
    /// <summary>
    /// LOG記錄檔
    /// </summary>
    public class LogController
    {
        /// <summary>
        /// Log位置
        /// </summary>
        private static string _LogPath = "";

        /// <summary>
        /// IISLog設定值, 0=不執行, 1=執行
        /// </summary>
        private static string _IsWriteIISLog = "";

        /// <summary>
        /// Log副檔名
        /// </summary>
        private static string _ExtName = ".log";

        /// <summary>
        /// 建立LOG檔案
        /// </summary>
        /// <param name="tMethodName">LOG名稱</param>
        /// <param name="tMessage">記錄的訊息</param>
        public static void WriteLog(string tMethodName, string tMessage)
        {
            string tExtName = _ExtName;

            if ( _LogPath == "" )
            {
                //讀取config的設定
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                //不存在的路徑就建立路徑
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //沒有設定log_location時則離開
            if ( string.IsNullOrEmpty(_LogPath) ) return;

            //Log內容建立的時間
            DateTime thisNow = DateTime.Now;
            string tNowDate = thisNow.ToString("yyyyMMdd");
            string tNowTime = thisNow.ToString("HH:mm:ss");

            // Compose a string that consists of three lines.
            //string tLines = tThisDate + " " + tThisTime + " (" + tMethodName + "): " + tMessage;
            string tLines = tNowDate + " " + tNowTime + "(" + tMethodName + ")" + tMessage;
            string tPathAndFile = _LogPath + "\\" + tNowDate + tExtName;

            //write log to file
            WriteToFile(tPathAndFile, tLines);
            //write log to file
            //WriteToFile(tPathAndFile, "\n\r");
            //WriteToFile(tPathAndFile, string.Empty);

            //write error message to iis event log
            WriteErrorToIISlog(tMessage);

        }



        /// <summary>
        /// 建立LOG檔案
        /// </summary>
        /// <param name="tMethodName">LOG名稱</param>
        /// <param name="tMessage">記錄的訊息</param>
        /// <param name="tFileName">LOG檔的檔案名稱, 不含副檔名</param>
        public static void WriteLog(string tMethodName, string tMessage, string tFileName)
        {
            string tExtName = _ExtName;
            if ( _LogPath == "" )
            {
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //沒有設定log_location時則離開
            if ( string.IsNullOrEmpty(_LogPath) ) return;


            DateTime tNow = DateTime.Now;
            string tNowDate = tNow.ToString("yyyyMMdd");
            string tNowTime = tNow.ToString("HH:mm:ss");

            // Compose a string that consists of three lines.
            //string tLines = tThisDate + " " + tThisTime + " - (" + tMethodName + "): " + tMessage;
            string tLines = tNowDate + " " + tNowTime + "(" + tMethodName + ")" + tMessage;
            string tPathAndFile = _LogPath + "\\" + tFileName + tExtName;

            //write log to file
            WriteToFile(tPathAndFile, tLines);
            //write log to file
            //WriteToFile(tPathAndFile, "\n\r");
            WriteToFile(tPathAndFile, string.Empty);


            //write error message to iis event log
            WriteErrorToIISlog(tMessage);

        }

        /// <summary>
        /// 將內容寫入至檔案
        /// </summary>
        /// <param name="tPath">目標位置含檔名副檔名</param>
        /// <param name="tValue">要寫入的內容</param>
        private static void WriteToFile(string tPath, string tValue)
        {
            StreamWriter oStreamWriter = null;
            try
            {
                // Write the string to a file.
                //oStreamWriter = new StreamWriter(tPath, true);
                //oStreamWriter = new StreamWriter(tPath, true, System.Text.Encoding.UTF8);
                //oStreamWriter = new StreamWriter(tPath, true, System.Text.Encoding.ASCII);

                oStreamWriter = new StreamWriter(tPath, true, System.Text.Encoding.Unicode);

                oStreamWriter.WriteLine(tValue);
                oStreamWriter.Close();
            }
            catch ( Exception ex )
            {
                string msg = ex.ToString();
                if ( oStreamWriter != null ) oStreamWriter.Close();
            }
            finally
            {
                //file.Close();
            }
        }

        


        /// <summary>
        /// 檢查是否有檔案
        /// </summary>
        /// <param name="tLogName">log名稱含副檔名</param>
        public static void IsLogFile(string tLogName)
        {
            string tExtName = _ExtName;

            if ( _LogPath == "" )
            {
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //沒有設定log_location時則離開
            if ( string.IsNullOrEmpty(_LogPath) ) return;

            DateTime thisNow = DateTime.Now;
            string tNowDate = thisNow.ToString("yyyyMMdd");
            string tBeginDate = thisNow.AddDays(-7).ToString("yyyyMMdd");

            FileInfo oFileInfo = new FileInfo(_LogPath + tNowDate + tExtName);

            if ( !oFileInfo.Exists )
            {
                // Create a reference to a file.
                FileInfo oFileInfo1 = new FileInfo(_LogPath + tNowDate + tExtName);
                // Actually create the file.
                FileStream fs = oFileInfo1.Create();
                // Modify the file as required, and then close the file.
                fs.Close();
            }
            else
            {
                FileInfo oFileInfo2 = new FileInfo(_LogPath + tBeginDate + tExtName);
                if ( oFileInfo2.Exists )
                {
                    oFileInfo2.Delete();
                }
            }
        }

        /// <summary>
        /// 應用程序錯誤訊息加至windows事件檢視器中 create-by bruce 20091109
        /// </summary>
        public static void WriteErrorToIISlog(string tMessage)
        {
            //write error message to iis event log
            //IISLog設定值, 0=不執行, 1=執行
            if ( _IsWriteIISLog == "" ) _IsWriteIISLog = ConfigurationManager.AppSettings["WriteIISLog"];
            if ( string.IsNullOrEmpty(_IsWriteIISLog) ) return;
            if ( _IsWriteIISLog == "0" ) return;

            System.Threading.Thread.Sleep(100);

            string Message = "";
            //Exception ex = Server.GetLastError();
            //Exception ex = Server.GetLastError().GetBaseException();
            //Message = "發生錯誤的網頁:{0}錯誤訊息:{1}堆疊內容:{2}";
            Message = "錯誤網頁:{0}錯誤訊息:{1}";
            //Message = String.Format(Message, Request.Url.ToString() + Environment.NewLine, ex.ToString() + Environment.NewLine, Environment.NewLine + ex.StackTrace);
            Message = String.Format(Message, "" + Environment.NewLine, tMessage + Environment.NewLine);
            System.Diagnostics.EventLog.WriteEntry(Guid.NewGuid().ToString("D"), Message, System.Diagnostics.EventLogEntryType.Error); //寫入事件撿視器,方法一
            //string logRoot = "Log";
            //string logTarget = DateTime.Now.ToString("yyyy-MM-dd");
            //string logPath = logRoot + "\\" + logTarget + "\\";
            //System.IO.Directory.CreateDirectory(Server.MapPath(logRoot));
            //System.IO.Directory.CreateDirectory(Server.MapPath(logPath));
            //System.IO.File.AppendAllText(Server.MapPath(string.Format("{2}\\{1}\\{0}.txt", DateTime.Now.Ticks.ToString(), logTarget, logRoot)), Message); //寫入文字檔,方法二
            ////此方法請參考System.Net.Mail.MailMessage   //寄出Email,方法三
            //Server.ClearError();  //清除Error

            System.Threading.Thread.Sleep(100);
        }


    }
}
