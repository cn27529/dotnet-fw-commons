using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;

namespace Commons
{
    /// <summary>
    /// Mail �o�e���� , �Q�� Commons.SimpleSmtp ���O�ֳt�إߺ����o�e E-MAIL �ѨM��k
    /// </summary>
    /// <remarks>
    /// �N�U�C�ѼƳ]�w�� WEB ���x�պA�]�w�� appSettings �϶��� , 
    /// 1. add key="SmtpServer" value="mail.24drs.com"
    /// 2. add key="ServiceMailDisplayName" value="�t�Φ۰ʵo�e�H��!"
    /// 3. add key="ServiceMailAddress" value="webservice@tty.com.tw"
    /// </remarks>
    public class SimpleSmtp
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
            SmtpClient s = SimpleSmtp.CreationSmtpClient();

            //���o�γ]�w�b�K-----------------add by bruce 20150119
            string user_name = SimpleSmtp.MailUserName;
            string password = SimpleSmtp.MailPassword;
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                s.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
            //���o�γ]�w�b�K-----------------add by bruce 20150119

            MailMessage smail = SimpleSmtp.CreationMailMessage();
            bool resilt = false;

            if ( fromDisplayName == "" && fromMail == "" )
                smail.From = new MailAddress(SimpleSmtp.ServiceEmailAddress, SimpleSmtp.ServiceMailDiaplayName);
            else if ( fromDisplayName != "" && fromMail == "" )
                smail.From = new MailAddress(SimpleSmtp.ServiceEmailAddress, fromDisplayName);
            else if ( fromDisplayName == "" && fromMail != "" )
                smail.From = new MailAddress(fromMail, SimpleSmtp.ServiceMailDiaplayName);
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
            SmtpClient s = SimpleSmtp.CreationSmtpClient();

            //���o�γ]�w�b�K-----------------add by bruce 20150119
            string user_name = SimpleSmtp.MailUserName;
            string password = SimpleSmtp.MailPassword;
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                s.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
            //���o�γ]�w�b�K-----------------add by bruce 20150119


            MailMessage smail = SimpleSmtp.CreationMailMessage();
            bool resilt = false;
            if ( fromDisplayName == "" )
                smail.From = new MailAddress(SimpleSmtp.ServiceEmailAddress);
            else
                smail.From = new MailAddress(SimpleSmtp.ServiceEmailAddress, fromDisplayName);
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
            SmtpClient s = SimpleSmtp.CreationSmtpClient();

            //���o�γ]�w�b�K-----------------add by bruce 20150119
            string user_name = SimpleSmtp.MailUserName;
            string password = SimpleSmtp.MailPassword;
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                s.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
            //���o�γ]�w�b�K-----------------add by bruce 20150119

            MailMessage smail = SimpleSmtp.CreationMailMessage();
            smail.From = new MailAddress(SimpleSmtp.ServiceEmailAddress, fromDisplayName);
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
            SmtpClient s = SimpleSmtp.CreationSmtpClient();

            //���o�γ]�w�b�K-----------------add by bruce 20150119
            string user_name = SimpleSmtp.MailUserName;
            string password = SimpleSmtp.MailPassword;
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                s.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
            //���o�γ]�w�b�K-----------------add by bruce 20150119

            MailMessage smail = SimpleSmtp.CreationMailMessage();
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
            SmtpClient s = SimpleSmtp.CreationSmtpClient();

            //���o�γ]�w�b�K-----------------add by bruce 20150119
            string user_name = SimpleSmtp.MailUserName;
            string password = SimpleSmtp.MailPassword;
            if (!string.IsNullOrEmpty(user_name) && !string.IsNullOrEmpty(password))
            {
                s.Credentials = new System.Net.NetworkCredential(user_name, password);
            }
            //���o�γ]�w�b�K-----------------add by bruce 20150119

            MailMessage smail = SimpleSmtp.CreationMailMessage();
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
        private static SmtpClient CreationSmtpClient() 
        {
            return new SmtpClient(SimpleSmtp.SmtpServer);
        }

        /// <summary>
        /// ���o MailMessage ����
        /// </summary>
        /// <returns>MailMessage</returns>
        private static MailMessage CreationMailMessage()
        {
            string fromMail = SimpleSmtp.ServiceEmailAddress; //system mail address
            string fromDisplayName = SimpleSmtp.ServiceMailDiaplayName; //mail address display name
            MailMessage smail = new MailMessage();
            smail.From = new MailAddress(fromMail, fromDisplayName);
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

        /// <summary>
        /// MailUserName
        /// NetworkCredential, ���ѱK�X�[�c������ (Authentication) ���� (�Ҧp�򥻡B�K�n�BNTLM �M Kerberos ����) ���{�ҡC
        /// </summary>
        private static string MailUserName = System.Configuration.ConfigurationManager.AppSettings["MailUserName"].ToString();

        /// <summary>
        /// MailPassword
        /// NetworkCredential, ���ѱK�X�[�c������ (Authentication) ���� (�Ҧp�򥻡B�K�n�BNTLM �M Kerberos ����) ���{�ҡC
        /// </summary>
        private static string MailPassword = System.Configuration.ConfigurationManager.AppSettings["MailPassword"].ToString();

    }
}
