using Dapper;
using JBus.Web.Helpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace JBus.Web.Models
{
    public enum MailType
    {
        None = 0,
        Alert = 1,
        Test = 2
    }

    public enum MailStatus
    {
        Unknown = 0,// initial state
        Sending = 1,
        Success = 2,
        Error = 3
    }

    public class MailLog
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public MailType MailType { get; set; }

        public string ToAddr { get; set; }

        public string CcAddr { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string Attachment { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? SentTime { get; set; }

        public MailStatus Status { get; set; }

        public string ErrorMsg { get; set; }

        public bool IsValidTestMail(ModelStateDictionary modelState)
        {
            ValidationHelper.IsValidTextField(modelState, "ช่อง To", "ToAddr", ToAddr, 100, true);
            ValidationHelper.IsValidTextField(modelState, "เรื่อง", "Subject", Subject, 200, true);
            ValidationHelper.IsValidTextField(modelState, "ข้อความ", "Message", Message, 10000, true);
            return modelState.IsValid;
        }

        public static void Create(MailLog model)
        {
            model.Id = (int)Current.DB.MailLogs.Insert(
                new
                {
                    model.UserId,
                    model.MailType,
                    model.ToAddr,
                    model.CcAddr,
                    model.Subject,
                    model.Message,
                    model.Attachment,
                    CreateTime = DateTime.Now,
                    Status = MailStatus.Sending,
                    ErrorMsg = ""
                });
        }

        public static void Update(MailLog model)
        {
            string sql = "";
            if (model.Status == MailStatus.Success)
            {
                sql = @"
update MailLogs
set SentTime=@SentTime,
    Status=@Status
where Id = @id";
            }
            else
            {
                sql = @"
update MailLogs
set Status=@Status,
    ErrorMsg=@ErrorMsg
where Id = @id";
            }
            Current.DB.Execute(sql, model);
        }
    }
}