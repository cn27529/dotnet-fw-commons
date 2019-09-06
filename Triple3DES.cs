using System;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Commons
{

    /// <summary>
    /// 參考http://www.iwms.net/n924c43.aspx
    /// 參考http://msdn.microsoft.com/zh-tw/library/system.security.cryptography.passwordderivebytes.aspx
    /// 建立者:Bruce.Huang TEL:0928230520
    /// 建立日:2013/1/2
    /// </summary>
    public class Triple3DES
    {
        /// <summary>
        /// 預設金鑰
        /// </summary>
        private string _tKey = "qJzGEh6hESZDVJeCnFPGuxzaiB7NLQM3";

        /// <summary>
        /// 預設向量值，向量值可以為空白
        /// </summary>
        private string _tIV = "qcDY6X+aPLw=";

        /// <summary>
        /// 產生金鑰的密碼
        /// DOH表示由衛生署要傳給中心的加密密碼, 中心需要用DOH密碼來解密
        /// IAFI表示由中心傳給衛生署的的加密密碼, 中心需要用IAFI密碼來解密
        /// </summary>
        private string _tPassword = "IAFI";

        /// <summary>
        /// 用來衍生金鑰的密碼
        /// </summary>
        private byte[] _pwd = new byte[] { };

        /// <summary>
        /// 用來衍生金鑰的金鑰 Salt
        /// </summary>
        private byte[] _salt = new byte[] { };

        /// <summary>
        /// 建立一個對稱演算法
        /// </summary>
        private SymmetricAlgorithm _TripleDES = new TripleDESCryptoServiceProvider();

        /// <summary>
        /// 建構
        /// </summary>
        public Triple3DES() { }

        /// <summary>
        /// 建構
        /// </summary>
        /// <param name="tKEY">輸入金鑰KEY</param>
        /// <param name="tIV">輸入向量值IV</param>
        public Triple3DES(string tKEY, string tIV)
        {
            //this._tKey = tKEY;
            //this._tIV = tIV;
            this.set_KEY_IV(tKEY, tIV);
        }

        /// <summary>
        /// 設定金鑰KEY與向量值IV
        /// </summary>
        /// <param name="tKEY">輸入金鑰KEY</param>
        /// <param name="tIV">輸入向量值IV</param>
        public void set_KEY_IV(string tKEY, string tIV)
        {
            this._tKey = tKEY;
            this._tIV = tIV;
            _TripleDES.Key = Convert.FromBase64String(_tKey);
            _TripleDES.IV = Convert.FromBase64String(_tIV);
        }

        /// <summary>
        /// 設定要產生金鑰的密碼
        /// </summary>
        /// <param name="tPassword">密碼</param>
        public void set_Password(string tPassword)
        {
            //this._tPassword = tPassword;
            //this._pwd = Encoding.Unicode.GetBytes(tPassword);
            //this._salt = CreateRandomSalt(7);
            //PasswordDeriveBytes oPDB = new PasswordDeriveBytes(_pwd, _salt);
            //_TripleDES.Key = oPDB.CryptDeriveKey("TripleDES", "SHA1", 192, _TripleDES.IV);

            this._tPassword = tPassword;
            this._pwd = Encoding.Unicode.GetBytes(tPassword);
            this._salt = CreateRandomSalt(7);
            PasswordDeriveBytes oPDB = new PasswordDeriveBytes(_pwd, _salt);
            _TripleDES.Key = oPDB.CryptDeriveKey("TripleDES", "SHA1", 192, _TripleDES.IV);
        }

        /// <summary>
        /// Generates a random salt value of the specified length.
        /// 生成指定長度的隨機salt值
        /// </summary>
        /// <param name="length">長度</param>
        /// <returns></returns>
        private static byte[] CreateRandomSalt(int length)
        {
            // Create a buffer
            byte[] randBytes;
            if ( length >= 1 )
                randBytes = new byte[length];
            else
                randBytes = new byte[1];
            // Create a new RNGCryptoServiceProvider.
            RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
            // Fill the buffer with random bytes.
            rand.GetBytes(randBytes);
            // return the bytes.
            return randBytes;
        }

        /// <summary>
        /// Clear the bytes in a buffer so they can't later be read from memory.
        /// 清除，使他們不能以後可以從內存中讀取的字節的緩衝區
        /// </summary>
        /// <param name="buffer">緩衝區變數</param>
        private void ClearBytes(byte[] buffer)
        {
            // Check arguments.
            if ( buffer == null ) throw new ArgumentException("buffer");
            // Set each byte in the buffer to 0.
            for ( int x = 0 ; x < buffer.Length ; x++ ) buffer[x] = 0;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="tValue">要加密的字串</param>
        /// <returns>加密後字串</returns>
        public string EncryptString(string tValue)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] bt;
            string tReaultValue = "";

            try
            {
                //_CSP.Key = Convert.FromBase64String(_tKey);
                //_CSP.IV = Convert.FromBase64String(_tIV);
                //指定加密的運算模式
                _TripleDES.Mode = System.Security.Cryptography.CipherMode.ECB;
                //獲取或設置加密演算法的填充模式
                _TripleDES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = _TripleDES.CreateEncryptor(_TripleDES.Key, _TripleDES.IV);
                bt = Encoding.UTF8.GetBytes(tValue);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                tReaultValue = Convert.ToBase64String(ms.ToArray());
            }
            catch ( Exception ex )
            {
                //Console.WriteLine(e.Message);
                throw ex;
            }
            finally
            {
                // Clear the buffers
                ClearBytes(this._pwd);
                ClearBytes(this._salt);
                // Clear the key.
                //this._TripleDES.Clear();
            }
            return tReaultValue;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="tValue">要解密的字串</param>
        /// <returns>解密後字串</returns>
        public string DecryptString(string tValue)
        {
            ICryptoTransform ct;
            MemoryStream ms;
            CryptoStream cs;
            byte[] bt;
            string tReaultValue = "";

            try
            {
                //_CSP.Key = Convert.FromBase64String(_tKey);
                //_CSP.IV = Convert.FromBase64String(_tIV);
                //指定加密的運算模式
                _TripleDES.Mode = System.Security.Cryptography.CipherMode.ECB;
                //獲取或設置加密演算法的填充模式
                _TripleDES.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
                ct = _TripleDES.CreateDecryptor(_TripleDES.Key, _TripleDES.IV);
                bt = Convert.FromBase64String(tValue);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(bt, 0, bt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                tReaultValue = Encoding.UTF8.GetString(ms.ToArray());
            }
            catch ( Exception ex )
            {
                //Console.WriteLine(e.Message);
                throw ex;
            }
            finally
            {
                // Clear the buffers
                ClearBytes(this._pwd);
                ClearBytes(this._salt);
                // Clear the key.
                //this._TripleDES.Clear();
            }
            return tReaultValue;

        }
    }
}