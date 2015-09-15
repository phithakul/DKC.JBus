using DKC.JBus.Models;
using NIGateway;
using System;
using System.Threading.Tasks;

namespace DKC.JBus.Helpers
{
    public static class SmsSender
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static SmsSender()
        {
            Init();
        }

        public static void Init()
        {
            SmsClient.HttpProxy.ProxyMode = AppSettings.UseProxyServer ? HttpProxyMode.ManualConfig : HttpProxyMode.None;
            SmsClient.HttpProxy.Host = AppSettings.HttpProxyAddress;
            SmsClient.HttpProxy.Port = AppSettings.HttpProxyPort;
            SmsClient.HttpProxy.Username = AppSettings.HttpProxyUsername;
            SmsClient.HttpProxy.Password = AppSettings.HttpProxyPassword;
            SmsClient.HttpProxy.Domain = "";
        }

        public static void SendMessageAsync(string AccountUsername, string AccountPassword, string mobileNo, string message)
        {
            SendMessageAsync(AccountUsername, AccountPassword, mobileNo, message, DateTime.MinValue);
        }

        public static void SendMessageAsync(string AccountUsername, string AccountPassword, string mobileNo, string message, DateTime scheduleTime)
        {
            var smsClient = new SmsClient(AccountUsername, AccountPassword);

            var smsLog = new SmsLog
            {
                UserId = Current.User.Id,
                MobileNo = mobileNo,
                Message = message,
                SentTime = DateTime.Now,
                SmsStatus = SmsStatus.Sending,
                MessageId = "",
                TaskId = "",
                ErrorMsg = ""
            };
            smsLog.Id = (int)Current.DB.SmsLogs.Insert(smsLog);

            Task.Factory.StartNew(() =>
            {
                try
                {
                    SendMessageResult result;
                    if (scheduleTime == DateTime.MinValue)
                    {
                        result = smsClient.SendMessage(mobileNo, message);
                    }
                    else
                    {
                        result = smsClient.SendMessage(mobileNo, message, scheduleTime);
                    }
                    Current.DB.Execute(@"update SmsLogs
set SmsStatus=@SmsStatus,
    MessageId=@MessageId,
    TaskId=@TaskId
where Id = @id", new
                    {
                        SmsStatus = (int)SmsStatus.Success,
                        MessageId = result.MessageId,
                        TaskId = result.TaskId,
                        id = smsLog.Id
                    });
                }
                catch (GatewayException ge)
                {
                    Current.DB.Execute(@"update SmsLogs
set SmsStatus=@SmsStatus,
    ErrorMsg=@ErrorMsg
where Id = @id", new
                    {
                        SmsStatus = (int)SmsStatus.Error,
                        ErrorMsg = ge.Message,
                        id = smsLog.Id
                    });
                }
                catch (Exception ex)
                {
                    Current.DB.Execute(@"update SmsLogs
set SmsStatus=@SmsStatus,
    ErrorMsg=@ErrorMsg
where Id = @id", new
                    {
                        SmsStatus = (int)SmsStatus.Error,
                        ErrorMsg = ex.Message,
                        id = smsLog.Id
                    });
                }
            });
        }
    }
}