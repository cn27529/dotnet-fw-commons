using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;

using System.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// AES
/// </summary>
public class AES
{
    const string string_pwd = "P@$$w0rd";

    /// <summary>
    /// �[�K�禡
    /// </summary>
    /// <param name="string_secretContent">���[�K�r��</param>
    /// <returns>�[�K��r��</returns>
    public static string Encrypt(string string_secretContent)
    {
        if (string_secretContent == null || string_secretContent.Trim() == "")
            return "";
        byte[] byte_secretContent = Encoding.UTF8.GetBytes(string_secretContent);
        byte[] byte_pwd = Encoding.UTF8.GetBytes(string_pwd);

        MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
        byte[] byte_pwdMD5 = provider_MD5.ComputeHash(byte_pwd);
        
        RijndaelManaged provider_AES = new RijndaelManaged();
        ICryptoTransform encrypt_AES = provider_AES.CreateEncryptor(byte_pwdMD5, byte_pwdMD5);

        byte[] output = encrypt_AES.TransformFinalBlock(byte_secretContent, 0, byte_secretContent.Length);
        return Convert.ToBase64String(output);
    }

    /// <summary>
    /// �ե[�Ksql
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public string EncryptSQL(string parameter)
    {
        return string.Format("EncryptByKey(Key_GUID('encrypt'), {0})", parameter);
    }

    /// <summary>
    //  �ѱK�禡
    ///  </summary>
    /// <param name="ciphertext">���ѱK�r��</param>
    /// <returns>�ѱK��r��</returns>
    public static string Decrypt(string ciphertext)
    {
        if (ciphertext == null || ciphertext.Trim() == "")
            return "";
        byte[] byte_ciphertext = Convert.FromBase64String(ciphertext);
        byte[] byte_pwd = Encoding.UTF8.GetBytes(string_pwd);

        MD5CryptoServiceProvider provider_MD5 = new MD5CryptoServiceProvider();
        byte[] byte_pwdMD5 = provider_MD5.ComputeHash(byte_pwd);

        RijndaelManaged provider_AES = new RijndaelManaged();
        ICryptoTransform decrypt_AES = provider_AES.CreateDecryptor(byte_pwdMD5, byte_pwdMD5);

        byte[] byte_secretContent = decrypt_AES.TransformFinalBlock(byte_ciphertext, 0, byte_ciphertext.Length);
        string string_secretContent = Encoding.UTF8.GetString(byte_secretContent);
        return string_secretContent;
    }

    public string DecryptSQL(string columns)
    {
        //return " convert(varchar, DecryptByKey(" + columns + ")) ";
        return " bd ";
    }

    /// <summary>
    /// �H������(���ҽX)
    /// </summary>
    /// <param name="NO">�n���ͪ��K�X��</param>
    /// �� BC.BC.ValidateCode.ashx.GenerateCheckCode
    /// <returns></returns>
    public string RandomCheckCode(int NO)
    {
        int number;
        char code;
        string checkCode = String.Empty;

        //System.Random random = new Random();

        for (int i = 0; i < NO; i++)
        {
            //number = random.Next();
            number = RngCrypRandom();

            if (number % 3 == 0)
            {
                code = (char)('0' + (char)(number % 10));
                checkCode += code.ToString();
            }
            else if (number % 3 == 1)
            {
                code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString().ToLower();
            }
            else if (number % 3 == 2)
            {
                code = (char)('a' + (char)(number % 26));
                checkCode += code.ToString().ToLower();
            }
        }

        //�x�s�bcookie
        //context.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));

        //�x�s�bsession
        //context.Session["CheckCode"] = checkCode;

        return checkCode;
    }

    /// <summary>
    /// �H������(�K�X��)
    /// </summary>
    /// <param name="NO">�n���ͪ��K�X��</param>
    /// <returns></returns>
    public string RandomCheckCodePwd(int NO)
    {
        int number;
        char code;
        string checkCode = String.Empty;

        System.Random random = new Random();

        for (int i = 0; i < NO; i++)
        {
            //number = random.Next();
            number = RngCrypRandom();

            if (number % 3 == 0)
            {
                code = (char)('0' + (int)(number % 10));
                checkCode += code.ToString();
            }
            else if (number % 3 == 1)
            {
                code = (char)('A' + (int)(number % 26));
                checkCode += code.ToString();
            }
            else if (number % 3 == 2)
            {
                code = '#';
                checkCode += code.ToString();
            }
        }

        return checkCode;
    }

    public static int RngCrypRandom()
    {
        byte[] bytes = new byte[4];
        RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        rand.GetBytes(bytes);
        int number = BitConverter.ToInt32(bytes, 0);
        return number * (number < 0 ? -1 : 1);
    }

    public static int RngCrypRandom(int intMax)
    {
        byte[] bytes = new byte[4];
        RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();

        rand.GetBytes(bytes);
        int number = (int)((decimal)bytes[0] / 256 * intMax) + 1;
        return number;
    }
}
