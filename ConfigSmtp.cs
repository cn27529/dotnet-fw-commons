using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Configuration;


namespace Commons
{
    /// <summary>
    /// Mail 發送元件 , 利用 Commons.ConfigSmtp 類別快速建立網站發送 E-MAIL 解決方法
    /// </summary>
    /// <remarks>
    /// 以設定檔方式將下列參數設定於 WEB 站台組態設定檔區塊內 , 
    /// <system.net>
    /// <mailSettings>
    ///   <smtp deliveryMethod="Network">
    ///     <network defaultCredentials="false"
    ///       host="Your.SMTP.Server" port="25"
    ///       userName="MyAccount" password="ThisIsPassword" />
    ///   </smtp>
    /// </mailSettings>
    /// </system.net>
    /// 
    /// </remarks>
    public class ConfigSmtp
    {
        /// <summary>
        /// SendMail 發送郵件
        /// </summary>
        /// <param name="mailToArray">對方郵件地址</param>
        /// <param name="fromMail">來源地址</param>
        /// <param name="fromDisplayName">來源地址別名</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <returns></returns>
        public static bool SendMail(string[] mailToArray, string fromMail, string fromDisplayName, string subject, string body)
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            MailMessage smail = ConfigSmtp.CreationMailMessage();
            bool resilt = false;

            if ( fromDisplayName == "" && fromMail == "" )
                smail.From = new MailAddress(nc.UserName, ConfigSmtp.ServiceMailDiaplayName);
            else if ( fromDisplayName != "" && fromMail == "" )
                smail.From = new MailAddress(nc.UserName, fromDisplayName);
            else if ( fromDisplayName == "" && fromMail != "" )
                smail.From = new MailAddress(fromMail, ConfigSmtp.ServiceMailDiaplayName);
            else if ( fromDisplayName != "" && fromMail != "" )
                smail.From = new MailAddress(fromMail, fromDisplayName);

            try
            {
                if ( CheckMailTo(mailToArray) ) { throw MailToAddressException(); }
                smail.Subject = subject.Trim();
                smail.Body = body.Trim();
                foreach ( string address in mailToArray ) { smail.To.Add(new MailAddress(address.Trim())); }
                s.Send(smail);
                resilt = true;
            }
            catch ( System.FormatException ex )
            {
                throw ex;
            }
            catch ( System.Net.Mail.SmtpException ex )
            {
                throw ex;
            }
            finally { smail.Dispose(); }
            return resilt;
        }


        /// <summary>
        /// SendMail 發送郵件
        /// </summary>
        /// <param name="mailToArray">對方郵件地址</param>
        /// <param name="fromDisplayName">來源地址別名</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <returns></returns>
        public static bool SendMail(string[] mailToArray, string fromDisplayName, string subject, string body)
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            MailMessage smail = ConfigSmtp.CreationMailMessage();
            bool resilt = false;

            if ( fromDisplayName == "" )
                smail.From = new MailAddress(nc.UserName);
            else
                smail.From = new MailAddress(nc.UserName, fromDisplayName);

            try
            {
                if ( CheckMailTo(mailToArray) ) { throw MailToAddressException(); }
                smail.Subject = subject.Trim();
                smail.Body = body.Trim();
                foreach ( string address in mailToArray ) { smail.To.Add(new MailAddress(address.Trim())); }
                s.Send(smail);
                resilt = true;
            }
            catch ( System.FormatException ex )
            {
                throw ex;
            }
            catch ( System.Net.Mail.SmtpException ex )
            {
                throw ex;
            }
            finally { smail.Dispose(); }
            return resilt;
        }

        /// <summary>
        /// SendMail 發送郵件
        /// </summary>
        /// <param name="mailToArray">對方郵件地址</param>
        /// <param name="mailToDisplayNameArray">對方地址別名</param>
        /// <param name="fromDisplayName">來源地址別名</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <returns></returns>
        public static bool SendMail(string[] mailToArray, string[] mailToDisplayNameArray, string fromDisplayName, string subject, string body)
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            MailMessage smail = ConfigSmtp.CreationMailMessage();
            smail.From = new MailAddress(nc.UserName, fromDisplayName);
            bool resilt = false;

            try
            {
                if ( CheckMailTo(mailToArray) ) { throw MailToAddressException(); }
                smail.Subject = subject.Trim();
                smail.Body = body.Trim();
                for ( int i = 0 ; i < mailToArray.Length ; i++ )
                {
                    if ( mailToDisplayNameArray.Length == mailToArray.Length )
                        smail.To.Add(new MailAddress(mailToArray[i].Trim(), mailToDisplayNameArray[i].Trim()));
                    else
                        smail.To.Add(new MailAddress(mailToArray[i].Trim()));
                }
                s.Send(smail);
                resilt = true;
            }
            catch ( System.FormatException ex )
            {
                throw ex;
            }
            catch ( System.Net.Mail.SmtpException ex )
            {
                throw ex;
            }
            finally { smail.Dispose(); }
            return resilt;
        }

        /// <summary>
        /// SendMail 發送郵件
        /// </summary>
        /// <param name="mailToArray">對方郵件地址</param>
        /// <param name="mailToDisplayNameArray">對方地址別名</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <returns></returns>
        public static bool SendMail(string[] mailToArray, string[] mailToDisplayNameArray, string subject, string body)
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            MailMessage smail = ConfigSmtp.CreationMailMessage();
            bool resilt = false;
            try
            {
                if ( CheckMailTo(mailToArray) ) { throw MailToAddressException(); }
                smail.Subject = subject.Trim();
                smail.Body = body.Trim();
                for ( int i = 0 ; i < mailToArray.Length ; i++ )
                {
                    if ( mailToDisplayNameArray.Length == mailToArray.Length )
                        smail.To.Add(new MailAddress(mailToArray[i].Trim(), mailToDisplayNameArray[i].Trim()));
                    else
                        smail.To.Add(new MailAddress(mailToArray[i].Trim()));
                }
                s.Send(smail);
                resilt = true;
            }
            catch ( System.FormatException ex )
            {
                throw ex;
            }
            catch ( System.Net.Mail.SmtpException ex )
            {
                throw ex;
            }
           

            finally { smail.Dispose(); }
            return resilt;
        }

        /// <summary>
        /// SendMail 發送郵件
        /// </summary>
        /// <param name="mailToArray">對方郵件地址</param>
        /// <param name="subject">主旨</param>
        /// <param name="body">內容</param>
        /// <returns></returns>
        public static bool SendMail(string[] mailToArray, string subject, string body)
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            MailMessage smail = ConfigSmtp.CreationMailMessage();
            bool resilt = false;
            try
            {
                if ( CheckMailTo(mailToArray) ) { throw MailToAddressException(); }
                smail.Subject = subject.Trim();
                smail.Body = body.Trim();
                foreach ( string address in mailToArray ) { smail.To.Add(new MailAddress(address.Trim())); }
                s.Send(smail);
                resilt = true;
            }
            catch ( System.Net.Mail.SmtpException ex ) { throw ex; }
            finally { smail.Dispose(); }
            return resilt;
        }

        private static Exception MailToAddressException() { return new Exception("對方郵件引數有問題"); }

        /// <summary>
        /// 檢查郵件
        /// </summary>
        /// <param name="mailArray">郵件陣列</param>
        /// <returns></returns>
        private static bool CheckMailTo(string[] mailArray)
        {
            bool result = false;
            if ( mailArray == null || mailArray.Length == 0 ) { result = true; }
            return result;
        }

        /// <summary>
        /// 取得 SmtpClient 物件
        /// </summary>
        /// <returns>SmtpClient</returns>
        private static SmtpClient CreationSmtpClient() { return new SmtpClient(); }

        /// <summary>
        /// 取得 MailMessage 物件
        /// </summary>
        /// <returns>MailMessage</returns>
        private static MailMessage CreationMailMessage()
        {
            SmtpClient s = ConfigSmtp.CreationSmtpClient();
            NetworkCredential nc = (NetworkCredential) s.Credentials;
            //string fromMail = nc.UserName; //system mail address
            string fromDisplayName = ConfigSmtp.ServiceMailDiaplayName; //mail address display name
            MailMessage smail = new MailMessage();
            //smail.From = new MailAddress(fromMail, fromDisplayName);
            smail.IsBodyHtml = true;
            //smail.Attachments.Add(new Attachment(""));
            smail.BodyEncoding = System.Text.Encoding.UTF8;
            //smail.CC.Add(new MailAddress(""));
            //smail.DeliveryNotificationOptions = DeliveryNotificationOptions.Delay;
            //smail.From = new MailAddress(fromMail, fromDisplayName);
            smail.SubjectEncoding = System.Text.Encoding.UTF8;
            return smail;
        }

        /// <summary>
        /// SMTP 伺服器名稱
        /// </summary>
        private static string SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString();

        /// <summary>
        /// 系統服務 Mail address
        /// </summary>
        private static string ServiceEmailAddress = System.Configuration.ConfigurationManager.AppSettings["ServiceMailAddress"].ToString();

        /// <summary>
        /// 系統發送標題 Mail title
        /// </summary>
        private static string ServiceMailDiaplayName = System.Configuration.ConfigurationManager.AppSettings["ServiceMailDisplayName"].ToString();
    }
}
