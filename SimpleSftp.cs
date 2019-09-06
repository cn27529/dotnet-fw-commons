using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Tamir.SharpSsh.jsch;
using Tamir.SharpSsh;

namespace Commons
{
    public class SimpleSftp
    {
        /// <summary>
        /// 開始進行SFTP程序
        /// </summary>
        private Sftp _sftp = null;
        private string _hostName = string.Empty;
        private string _userName = string.Empty;
        private string _password = string.Empty;
        private int _port = 0;

        /// <summary>
        /// 來源檔案
        /// </summary>
        string _fromFilePath = string.Empty;
        /// <summary>
        /// 來源檔案
        /// </summary>
        string _toFilePath = string.Empty;

        /// <summary>
        /// IP 或 主機名稱
        /// </summary>
        /// <param name="hostName"></param>
        public void setHostName(string hostName) { _hostName = hostName; }

        /// <summary>
        /// 帳戶
        /// </summary>
        /// <param name="userName"></param>
        public void setUserName(string userName) { _userName = userName; }

        /// <summary>
        /// 密碼
        /// </summary>
        /// <param name="password"></param>
        public void setPassword(string password) {
            //_password = password;
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _password = AntiXSS.GetSafeHtmlFragment(password);
            //fortify漏洞-----------------------------------------------add by bruce 20131220
        }

        /// <summary>
        /// 連接埠
        /// </summary>
        /// <param name="port"></param>
        public void setPort(int port) { _port = port; }

        private void init_Sftp()
        {
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _password = AntiXSS.GetSafeHtmlFragment(_password);
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _sftp = new Sftp(_hostName, _userName, _password);
            if ( _port > 0 ) _sftp.Connect(_port);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public SimpleSftp(string hostName, int port, string userName, string password)
        {
            _hostName = hostName;
            //_password = password;
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _password = AntiXSS.GetSafeHtmlFragment(password);
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _port = port;
            _userName = userName;
            init_Sftp();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public SimpleSftp(string hostName, string userName, string password)
        {
            _hostName = hostName;
            //_password = password;
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _password = AntiXSS.GetSafeHtmlFragment(password);
            //fortify漏洞-----------------------------------------------add by bruce 20131220
            _userName = userName;
            init_Sftp();
        }

        

        /// <summary>
        /// 指定來源檔案
        /// </summary>
        /// <param name="fromFilePath"></param>
        public void setFromFilePath(string fromFilePath) { _fromFilePath = fromFilePath; }

       

        /// <summary>
        /// 指定目的檔案
        /// </summary>
        /// <param name="toFilePath"></param>
        public void setToFilePath(string toFilePath) { _toFilePath = toFilePath; }


        /// <summary>
        /// 遠程取得
        /// </summary>
        /// <param name="fromFilePath">來源檔案</param>
        /// <param name="toFilePath">目的檔案</param>
        public void Get(string fromFilePath, string toFilePath)
        {
            try
            {
                _fromFilePath = fromFilePath;
                _toFilePath = toFilePath;
                ToGet();
            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Get", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 遠程取得
        /// </summary>
        public void Get()
        {
            try
            {
                //bool tFFP_null = false;
                //bool tTFP_null = false;
                //if ( string.IsNullOrEmpty(_fromFilePath) ) { tFFP_null = true; }
                //if ( string.IsNullOrEmpty(_toFilePath) ) { tTFP_null = true; }
                //if ( tFFP_null || tTFP_null ) return;
                ToGet();
            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Get", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 遠程取得
        /// </summary>
        private void ToGet()
        {
            try
            {
                //fortify漏洞-----------------------------------------------add by bruce 20131220
                _password = AntiXSS.GetSafeHtmlFragment(_password);
                //fortify漏洞-----------------------------------------------add by bruce 20131220
                //開始進行SFTP程序
                _sftp = new Sftp(_hostName, _userName, _password);
                _sftp.Connect(_port);
                System.Threading.Thread.Sleep(3000);
                _sftp.Get(_fromFilePath, _toFilePath);
                System.Threading.Thread.Sleep(3000);
                _sftp.Close();

                //write log
                string msg = "";
                string fullFilePath = _toFilePath;
                FileInfo oFile = new FileInfo(fullFilePath); //e.g.: c:\\Test.txt
                if ( oFile.Exists )
                {
                    msg = "本地" + _toFilePath + ", 檔案已產生。";
                    Console.WriteLine(msg);
                    LogController.WriteLog("SimpleSftp.Get", msg);
                }
                else
                {
                    msg = "本地" + _toFilePath + ", 檔案沒有產生。";
                    Console.WriteLine(msg);
                    LogController.WriteLog("SimpleSftp.Get", msg);
                }

            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Get", ex.ToString());
                throw ex;
            }
        }


        /// <summary>
        /// 推至遠程
        /// </summary>
        /// <param name="fromFilePath">來源檔案</param>
        /// <param name="toFilePath">目的檔案</param>
        public void Put(string fromFilePath, string toFilePath)
        {
            try
            {
                ToPut();
            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Get", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 推至遠程
        /// </summary>
        public void Put()
        {
            try
            {
                ToPut();
            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Get", ex.ToString());
                throw ex;
            }
        }

        /// <summary>
        /// 推至遠程
        /// </summary>
        private void ToPut()
        {
            try
            {
                //fortify漏洞-----------------------------------------------add by bruce 20131220
                _password = AntiXSS.GetSafeHtmlFragment(_password);
                //fortify漏洞-----------------------------------------------add by bruce 20131220
                //開始進行SFTP程序
                _sftp = new Sftp(_hostName, _userName, _password);


                _sftp.Connect(_port);
                System.Threading.Thread.Sleep(3000);
                _sftp.Put(_fromFilePath, _toFilePath);
                System.Threading.Thread.Sleep(3000);
                _sftp.Close();

                //write log
                string msg = "";
                msg = "遠程" + _hostName + _toFilePath + ", 檔案已產生。";
                Console.WriteLine(msg);
                LogController.WriteLog("SimpleSftp.Put", msg);

            }
            catch ( Exception ex )
            {
                LogController.WriteLog("SimpleSftp.Put", ex.ToString());
                throw ex;
            }
        }

    }
}
