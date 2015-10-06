using DKC.JBus.Domains;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DKC.JBus.Helpers
{
    // ถ้า timeout แสดงว่า port ผิด
    // error 5.7.3 แสดงว่า username/password ผิด กรณี hotmail จะต้องใช้ password ที่กำหนดเฉพาะของแต่ละ app ที่กำหนดใน account setting
    // 30/7/2556 hotmail - smtp.live.com, 587 / smtp.gmail.com, 587

    //string[] toAddress = toAddr.Split(new char[] { ',', ';' });
    //new MailClient(setting.SmtpHost, setting.SmtpPort, setting.SmtpUsername,
    //        setting.SmtpPassword, setting.SmtpEnableSsl)
    //    .SendMail(setting.FromEmail, setting.DisplayName, toAddress, subjectText,
    //        messageBody, attachmentFiles, "ARS", false);

    public class MailClient
    {
        //private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private SmtpClient _smtpClient = null;

        public MailClient(string host, int port, string username, string password, bool enableSsl)
        {
            _smtpClient = new SmtpClientEx(host, port);
            //_smtpClient.ServicePoint.ConnectionLeaseTimeout = 0;
            if (!username.IsNullOrEmpty())
            {
                _smtpClient.UseDefaultCredentials = false;
                _smtpClient.Credentials = new NetworkCredential(username, password);
            }
            else
            {
                _smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            _smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            _smtpClient.EnableSsl = enableSsl;

            //ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        }

        public void SendMail(string fromAddress, string fromDisplayName, string[] toAddress, string[] ccAddress,
            string subjectText, string messageBody, bool isBodyHtml, string[] attachmentFiles, object userToken, /* UserState */
            bool isAsync)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress, fromDisplayName); // Mailbox of message author
            mail.Sender = new MailAddress(fromAddress, fromDisplayName); // Mailbox of message sender
            foreach (string address in toAddress)
            {
                mail.To.Add(address);
            }
            if (ccAddress != null && ccAddress.Length > 0)
            {
                foreach (string address in ccAddress)
                {
                    mail.CC.Add(address);
                }
            }
            mail.Subject = subjectText;
            mail.SubjectEncoding = System.Text.Encoding.GetEncoding("windows-874");
            mail.Body = messageBody;
            mail.BodyEncoding = System.Text.Encoding.GetEncoding("windows-874");
            mail.IsBodyHtml = isBodyHtml;
            mail.Priority = MailPriority.Normal;

            if (attachmentFiles != null)
            {
                foreach (string filename in attachmentFiles)
                {
                    mail.Attachments.Add(new Attachment(filename));
                }
            }

            //send an email to the reply address of any failures after all retries
            //mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            if (isAsync)
            {
                //attach the delegate to handle the callback
                _smtpClient.SendCompleted += new SendCompletedEventHandler(this.SendCompleted);

                //send the message and return immediately
                _smtpClient.SendAsync(mail, userToken);
            }
            else
                _smtpClient.Send(mail);
            //TODO: Do not close the stream
        }

        private void SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            SendCompleted(e);
            //if (e.Cancelled)
            //    logger.Info(string.Format("The message [{0}] was canceled.", e.UserState.ToString()));

            //if (e.Error != null) {
            //    StringBuilder errorMsg = new StringBuilder();
            //    errorMsg.Append(string.Format("An error occurred sending message [{0}]. {1}", e.UserState.ToString(), e.Error.Message));

            //    if (e.Error.InnerException != null)
            //        errorMsg.Append(Environment.NewLine + "Inner exception message: " + e.Error.InnerException.Message);

            //    logger.Error(errorMsg, e.Error);
            //} else {
            //    logger.Info(string.Format("[{0}] mail sent.", e.UserState.ToString()));
            //}
        }

        public void CancelMail()
        {
            _smtpClient.SendAsyncCancel();
        }

        private void SendCompleted(AsyncCompletedEventArgs e)
        {
            var mailLog = (MailLog)e.UserState;
            if (e.Error != null)
            {
                var sb = new StringBuilder();
                sb.Append(e.Error.Message);
                if (e.Error.InnerException != null)
                {
                    sb.Append(Environment.NewLine + "Inner exception message: " + e.Error.InnerException.Message);
                }
                mailLog.Status = MailStatus.Error;
                mailLog.ErrorMsg = sb.ToString();
            }
            else
            {
                mailLog.Status = MailStatus.Success;
                mailLog.SentTime = DateTime.Now;
            }
            MailLog.Update(mailLog);
        }
    }
}