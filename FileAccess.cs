using System;
using System.Text;
using System.IO;
using System.Web;
//using System.Web.Security;
//using System.Web.UI;

namespace Commons
{
    /// <summary>
    /// 簡單檔案存取類別
    /// </summary>
    public class FileAccess
    {
        /// <summary>
        /// 判斷指定的檔案是否存在
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <returns></returns>
        public static bool IsFileExists(string realFilePath)
        {
            bool value = File.Exists(realFilePath);
            return value;
        }

        /// <summary>
        /// 取得檔案內容文字
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        public static string ReadFile_Content(string realFilePath)
        {
            string tMethodName = "FileAccess.ReadFile_Content";
            string file_content = string.Empty;
            //加入換行
            string newLine = System.Environment.NewLine; //換行宣告

            try
            {
                // Open the stream and read it back.
                //using (StreamReader sr = File.OpenText(realFilePath)) //old code, mark by bruce 
                using (StreamReader sr = new StreamReader(realFilePath, System.Text.Encoding.Default)) //.Net Framework 讀取檔案變亂碼的處理方式 changed by bruce 20140219
                {

                    string s = string.Empty;
                    while ((s = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(s);
                        //Byte b = new Byte();
                        //UTF8Encoding utf8 = new UTF8Encoding(true);
                        //s = utf8.GetString(b, 0, 1024);
                        file_content += s;
                        //加上換行
                        file_content += newLine;
                    }
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                ConsoleHelp.outputConsoleAndWriteLog(tMethodName, msg);
                throw ex;
            }

            return file_content;
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        public static void RemoveFile(string realFilePath)
        {
            File.Delete(realFilePath);
        }

        /// <summary>
        /// 刪除檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        public static void DeleteFile(string realFilePath)
        {
            File.Delete(realFilePath);
        }

        /// <summary>
        /// 取得檔案位元組陣列
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <returns>檔案位元組陣列</returns>
        public static byte[] GetFileBytes(string realFilePath)
        {
            byte[] fileBytes = File.ReadAllBytes(realFilePath);
            return fileBytes;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <param name="tValue">檔案內容</param>
        /// <returns></returns>
        public static bool SaveFile(string tPath, string tValue)
        {
            StreamWriter streamWriter = null;
            bool flag = false;
            try
            {
                try
                {
                    streamWriter = new StreamWriter(tPath, true);
                    streamWriter.WriteLine(tValue);
                    streamWriter.Close();
                    flag = true;
                }
                catch (Exception exception)
                {
                    exception.ToString();
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                    }
                    flag = false;
                }
            }
            finally
            {
                //flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <param name="tValue">檔案內容</param>
        /// <param name="tEncoding">表示字元編碼方式</param>
        /// <returns></returns>
        public static bool SaveFile(string tPath, string tValue, Encoding tEncoding)
        {
            StreamWriter streamWriter = null;
            bool flag = false;
            try
            {
                try
                {

                    streamWriter = new StreamWriter(tPath, true, tEncoding);
                    streamWriter.WriteLine(tValue);
                    streamWriter.Close();
                    flag = true;
                }
                catch (Exception exception)
                {
                    exception.ToString();
                    if (streamWriter != null)
                    {
                        streamWriter.Close();
                    }
                    flag = false;
                }
            }
            finally
            {
                //flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <param name="fileBytes">檔案位元列陣</param>
        /// <returns></returns>
        public static bool SaveFile(string realFilePath, System.Byte[] fileBytes)
        {
            return FileAccess.SaveFile(realFilePath, fileBytes, false);
        }

        /// <summary>
        /// 儲存檔案
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <param name="fileBytes">檔案位元列陣</param>
        /// <param name="isCreatePathFolder">是否建立目的路徑</param>
        /// <returns></returns>
        public static bool SaveFile(string realFilePath, System.Byte[] fileBytes, bool isCreatePathFolder)
        {
            bool result = false;
            try
            {
                if (isCreatePathFolder) CreateDirectory(realFilePath); //是否需要建立路徑
                bool isFile = File.Exists(realFilePath);
                if (!isFile) { FileStream fs = File.Create(realFilePath); fs.Close(); }
                File.WriteAllBytes(realFilePath, fileBytes);
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

            //if (fileBytes.Length == 0) return result;
            //Stream s = new MemoryStream(); //建立IO執行個体
            //for (int i = 0; i < fileBytes.Length; i++) { s.WriteByte((byte)fileBytes[i]); } //讀取位元組
            //s.Position = 0;
            //s.Close();


            //// Now read s into a byte buffer. 準備讀取檔案內容
            //byte[] bytes = fileBytes;
            //int numBytesToRead = (int)s.Length;
            //int numBytesRead = 0;


            ////準備讀取檔案內容
            //while (numBytesToRead > 0)
            //{
            //    // Read may return anything from 0 to numBytesToRead.
            //    int n = s.Read(bytes, numBytesRead, numBytesToRead);
            //    // The end of the file is reached.
            //    if (n == 0) { break; }
            //    numBytesRead += n;
            //    numBytesToRead -= n;
            //}
            //s.Close();

            ////// numBytesToRead should be 0 now, and numBytesRead should
            ////// equal 100.
            ////Console.WriteLine("number of bytes read: " + numBytesRead);

        }

        /// <summary>
        /// 建立檔案路徑 , 不包含檔案名稱
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        /// <returns></returns>
        public static bool CreateFileFolder(string realFilePath) { return CreateDirectory(realFilePath); }

        /// <summary>
        /// 建立檔案路徑
        /// </summary>
        /// <param name="realFilePath">實體路徑檔案名稱</param>
        private static bool CreateDirectory(string realFilePath)
        {
            bool result = false;
            string[] paths = realFilePath.Split(new char[] { '\\' }); //取得路踁陣列
            StringBuilder folderPath = new StringBuilder();
            if (paths.Length == 0) { return result; }
            foreach (string value in paths)
            {
                //if ( value != "" && value.IndexOf(".") == -1  ) { folderPath.Append(value + @"\"); } //略過檔案名稱
                string theValue = value; // value.Replace(" ", "").Trim(); 不可去除空白,因為資料夾可以命名空白
                if (theValue != "") { folderPath.Append(theValue + @"\"); } //略過檔案名稱
            }
            try
            {
                if (Directory.Exists(folderPath.ToString())) {
                    result = true;
                    return result; 
                } //路徑是否存在
                Directory.CreateDirectory(folderPath.ToString()); //建立路徑 , di.Delete(); 刪除路徑 
                result = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
            return result;
        }

    }
}
