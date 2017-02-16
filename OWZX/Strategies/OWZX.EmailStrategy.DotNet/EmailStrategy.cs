using System;
using System.Net;
using System.Text;
using System.Net.Mail;

using OWZX.Core;

namespace OWZX.EmailStrategy.DotNet
{
    /// <summary>
    /// 基于.Net自带的邮件框架的策略
    /// </summary>
    public partial class EmailStrategy : IEmailStrategy
    {
        private string _host;
        private int _port;
        private string _username;
        private string _password;
        private string _from;
        private string _fromname;
        private Encoding _bodyencoding = Encoding.GetEncoding("utf-8");
        private bool _isbodyhtml = true;

        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string Host
        {
            get { return _host; }
            set { _host = value; }
        }

        /// <summary>
        /// 邮件服务器端口
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }

        /// <summary>
        /// 发送邮件的账号
        /// </summary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 发送邮件的密码
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        public string From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// 发送邮件的昵称
        /// </summary>
        public string FromName
        {
            get { return _fromname; }
            set { _fromname = value; }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">接收邮件</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string subject, string body)
        {
            if (_port == 465 || _port == 587 || _port ==995)
            {
                SmtpClient client = new SmtpClient(_host,_port);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false; 
                client.Credentials = new System.Net.NetworkCredential(_from, _password);
                MailAddress from = new MailAddress(_from, "发财28", Encoding.UTF8);//初始化发件人  
                MailAddress toadd = new MailAddress(to, "", Encoding.UTF8);//初始化收件人  
                //设置邮件内容  
                MailMessage message = new MailMessage(from, toadd);
                message.Body = body;
                message.BodyEncoding = _bodyencoding;
                message.Subject = subject;
                message.Priority = MailPriority.High;
                message.SubjectEncoding = System.Text.Encoding.Default;
                message.IsBodyHtml = _isbodyhtml;

                //发送邮件  
                try
                {
                    client.Send(message);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {

                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                if (_port != 25)
                    smtp.EnableSsl = true;

                smtp.Host = _host;
                smtp.Port = _port;
                smtp.Credentials = new NetworkCredential(_username, _password);

                MailMessage mm = new MailMessage();
                mm.Priority = MailPriority.Normal;
                mm.From =new MailAddress(_from, subject, _bodyencoding);
                mm.To.Add(to);
                mm.Subject = subject;
                mm.Body = body;
                mm.Priority = MailPriority.High;
                mm.BodyEncoding = _bodyencoding;
                mm.IsBodyHtml = _isbodyhtml;

                try
                {
                    smtp.Send(mm);
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to">接收邮件</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="bodyEncoding">邮件内容编码</param>
        /// <param name="isBodyHtml">邮件内容是否html化</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string to, string subject, string body, Encoding bodyEncoding, bool isBodyHtml)
        {
            SmtpClient smtp = new SmtpClient();
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            if (_port != 25)
                smtp.EnableSsl = true;

            smtp.Host = _host;
            smtp.Port = _port;
            smtp.Credentials = new NetworkCredential(_username, _password);

            MailMessage mm = new MailMessage();
            mm.Priority = MailPriority.Normal;
            mm.From = new MailAddress(_from, subject, bodyEncoding);
            mm.To.Add(to);
            mm.Subject = subject;
            mm.Body = body;
            mm.BodyEncoding = bodyEncoding;
            mm.IsBodyHtml = isBodyHtml;

            try
            {
                smtp.Send(mm);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
