using System;
using System.Collections.Generic;
using System.Text;

namespace Commons
{
    /// <summary>
    /// 資料加密基準 Data Encryption Standard
    /// </summary>
    public class Des
    {
        /// <summary>
        /// Construct
        /// </summary>
        private Des() { }

        /// <summary>
        /// 加解密法對應金鑰
        /// </summary>
        private string encryptKey { get { return "q1e3t5u7"; } }

        /// <summary>
        /// 加解密法對應金鑰
        /// </summary>
        private string encryptIV { get { return "w2r4y6i8"; } }

        /// <summary>
        /// 取得加密
        /// </summary>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string Encrypt(string encryptString)
        {
            try
            {
                Des des = new Des();
                byte[] rgbKey = Encoding.UTF8.GetBytes(des.encryptKey.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(des.encryptIV.Substring(0, 8));
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                System.Security.Cryptography.DESCryptoServiceProvider dCSP = new System.Security.Cryptography.DESCryptoServiceProvider();
                System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                System.Security.Cryptography.CryptoStream cStream = new System.Security.Cryptography.CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), System.Security.Cryptography.CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch ( Exception )
            {
                return encryptString;
            }
        }

        /// <summary>
        /// 取得解密
        /// </summary>
        /// <param name="decryptString"></param>
        /// <returns></returns>
        public static string Decrypt(string decryptString)
        {
            try
            {
                Des des = new Des();
                byte[] rgbKey = Encoding.UTF8.GetBytes(des.encryptKey.Substring(0, 8));
                byte[] rgbIV = Encoding.UTF8.GetBytes(des.encryptIV.Substring(0, 8));
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                System.Security.Cryptography.DESCryptoServiceProvider DCSP = new System.Security.Cryptography.DESCryptoServiceProvider();
                System.IO.MemoryStream mStream = new System.IO.MemoryStream();
                System.Security.Cryptography.CryptoStream cStream = new System.Security.Cryptography.CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), System.Security.Cryptography.CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch ( Exception )
            {
                return decryptString;
            }
        }
    }
}
