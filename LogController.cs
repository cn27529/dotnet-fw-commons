using System;
using System.IO;
using System.Configuration;
using System.Web;
//using System.Web.UI;

namespace Commons
{
    /// <summary>
    /// LOG�O����
    /// </summary>
    public class LogController
    {
        /// <summary>
        /// Log��m
        /// </summary>
        private static string _LogPath = "";

        /// <summary>
        /// IISLog�]�w��, 0=������, 1=����
        /// </summary>
        private static string _IsWriteIISLog = "";

        /// <summary>
        /// Log���ɦW
        /// </summary>
        private static string _ExtName = ".log";

        /// <summary>
        /// �إ�LOG�ɮ�
        /// </summary>
        /// <param name="tMethodName">LOG�W��</param>
        /// <param name="tMessage">�O�����T��</param>
        public static void WriteLog(string tMethodName, string tMessage)
        {
            string tExtName = _ExtName;

            if ( _LogPath == "" )
            {
                //Ū��config���]�w
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                //���s�b�����|�N�إ߸��|
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //�S���]�wlog_location�ɫh���}
            if ( string.IsNullOrEmpty(_LogPath) ) return;

            //Log���e�إߪ��ɶ�
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
        /// �إ�LOG�ɮ�
        /// </summary>
        /// <param name="tMethodName">LOG�W��</param>
        /// <param name="tMessage">�O�����T��</param>
        /// <param name="tFileName">LOG�ɪ��ɮצW��, ���t���ɦW</param>
        public static void WriteLog(string tMethodName, string tMessage, string tFileName)
        {
            string tExtName = _ExtName;
            if ( _LogPath == "" )
            {
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //�S���]�wlog_location�ɫh���}
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
        /// �N���e�g�J���ɮ�
        /// </summary>
        /// <param name="tPath">�ؼЦ�m�t�ɦW���ɦW</param>
        /// <param name="tValue">�n�g�J�����e</param>
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
        /// �ˬd�O�_���ɮ�
        /// </summary>
        /// <param name="tLogName">log�W�٧t���ɦW</param>
        public static void IsLogFile(string tLogName)
        {
            string tExtName = _ExtName;

            if ( _LogPath == "" )
            {
                _LogPath = ConfigurationManager.AppSettings["log_location"].ToString();
                if ( !Directory.Exists(_LogPath) ) Directory.CreateDirectory(_LogPath);
            }
            //�S���]�wlog_location�ɫh���}
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
        /// ���ε{�ǿ��~�T���[��windows�ƥ��˵����� create-by bruce 20091109
        /// </summary>
        public static void WriteErrorToIISlog(string tMessage)
        {
            //write error message to iis event log
            //IISLog�]�w��, 0=������, 1=����
            if ( _IsWriteIISLog == "" ) _IsWriteIISLog = ConfigurationManager.AppSettings["WriteIISLog"];
            if ( string.IsNullOrEmpty(_IsWriteIISLog) ) return;
            if ( _IsWriteIISLog == "0" ) return;

            System.Threading.Thread.Sleep(100);

            string Message = "";
            //Exception ex = Server.GetLastError();
            //Exception ex = Server.GetLastError().GetBaseException();
            //Message = "�o�Ϳ��~������:{0}���~�T��:{1}���|���e:{2}";
            Message = "���~����:{0}���~�T��:{1}";
            //Message = String.Format(Message, Request.Url.ToString() + Environment.NewLine, ex.ToString() + Environment.NewLine, Environment.NewLine + ex.StackTrace);
            Message = String.Format(Message, "" + Environment.NewLine, tMessage + Environment.NewLine);
            System.Diagnostics.EventLog.WriteEntry(Guid.NewGuid().ToString("D"), Message, System.Diagnostics.EventLogEntryType.Error); //�g�J�ƥ�ߵ���,��k�@
            //string logRoot = "Log";
            //string logTarget = DateTime.Now.ToString("yyyy-MM-dd");
            //string logPath = logRoot + "\\" + logTarget + "\\";
            //System.IO.Directory.CreateDirectory(Server.MapPath(logRoot));
            //System.IO.Directory.CreateDirectory(Server.MapPath(logPath));
            //System.IO.File.AppendAllText(Server.MapPath(string.Format("{2}\\{1}\\{0}.txt", DateTime.Now.Ticks.ToString(), logTarget, logRoot)), Message); //�g�J��r��,��k�G
            ////����k�аѦ�System.Net.Mail.MailMessage   //�H�XEmail,��k�T
            //Server.ClearError();  //�M��Error

            System.Threading.Thread.Sleep(100);
        }


    }
}
