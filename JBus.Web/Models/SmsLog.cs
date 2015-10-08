using JBus.Web.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace JBus.Web.Models
{
    public enum SmsStatus
    {
        None = 0,
        Sending = 1,
        Success = 2,
        Error = 3,
    }

    public class SmsLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string MobileNo { get; set; }

        public string Message { get; set; }

        public DateTime SentTime { get; set; }

        public SmsStatus SmsStatus { get; set; }

        public string TaskId { get; set; }

        public string MessageId { get; set; }
        public string ErrorMsg { get; set; }
    }
}