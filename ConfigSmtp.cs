using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
//using System.Web.Configuration;


namespace Commons
{
    /// <summary>
    /// Mail �o�e���� , �Q�� Commons.ConfigSmtp ���O�ֳt�إߺ����o�e E-MAIL �ѨM��k
    /// </summary>
    /// <remarks>
    /// �H�]�w�ɤ覡�N�U�C�ѼƳ]�w�� WEB ���x�պA�]�w�ɰ϶��� , 
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
        /// SendMail �o�e�l��
        /// </summary>
        /// <param name="mailToArray">���l��a�}</param>
        /// <param name="fromMail">�ӷ��a�}</param>
        /// <param name="fromDisplayName">�ӷ��a�}�O�W</param>
        /// <param name="subject">�D��</param>
        /// <param name="body">���e</param>
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
        /// SendMail �o�e�l��
        /// </summary>
        /// <param name="mailToArray">���l��a�}</param>
        /// <param name="fromDisplayName">�ӷ��a�}�O�W</param>
        /// <param name="subject">�D��</param>
        /// <param name="body">���e</param>
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
        /// SendMail �o�e�l��
        /// </summary>
        /// <param name="mailToArray">���l��a�}</param>
        /// <param name="mailToDisplayNameArray">���a�}�O�W</param>
        /// <param name="fromDisplayName">�ӷ��a�}�O�W</param>
        /// <param name="subject">�D��</param>
        /// <param name="body">���e</param>
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
        /// SendMail �o�e�l��
        /// </summary>
        /// <param name="mailToArray">���l��a�}</param>
        /// <param name="mailToDisplayNameArray">���a�}�O�W</param>
        /// <param name="subject">�D��</param>
        /// <param name="body">���e</param>
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
        /// SendMail �o�e�l��
        /// </summary>
        /// <param name="mailToArray">���l��a�}</param>
        /// <param name="subject">�D��</param>
        /// <param name="body">���e</param>
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

        private static Exception MailToAddressException() { return new Exception("���l��޼Ʀ����D"); }

        /// <summary>
        /// �ˬd�l��
        /// </summary>
        /// <param name="mailArray">�l��}�C</param>
        /// <returns></returns>
        private static bool CheckMailTo(string[] mailArray)
        {
            bool result = false;
            if ( mailArray == null || mailArray.Length == 0 ) { result = true; }
            return result;
        }

        /// <summary>
        /// ���o SmtpClient ����
        /// </summary>
        /// <returns>SmtpClient</returns>
        private static SmtpClient CreationSmtpClient() { return new SmtpClient(); }

        /// <summary>
        /// ���o MailMessage ����
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
        /// SMTP ���A���W��
        /// </summary>
        private static string SmtpServer = System.Configuration.ConfigurationManager.AppSettings["SmtpServer"].ToString();

        /// <summary>
        /// �t�ΪA�� Mail address
        /// </summary>
        private static string ServiceEmailAddress = System.Configuration.ConfigurationManager.AppSettings["ServiceMailAddress"].ToString();

        /// <summary>
        /// �t�εo�e���D Mail title
        /// </summary>
        private static string ServiceMailDiaplayName = System.Configuration.ConfigurationManager.AppSettings["ServiceMailDisplayName"].ToString();
    }
}
