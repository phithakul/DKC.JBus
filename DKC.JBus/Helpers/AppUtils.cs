using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DKC.JBus.Helpers
{
    public static class AppUtils
    {
        private static CultureInfo _thaiCulture = System.Globalization.CultureInfo.GetCultureInfo("th-TH");
        private static string[] _timeFormats = new string[] { "HH:mm", "H:mm", "HH.mm", "H.mm" };

        public static CultureInfo ThaiCulture
        {
            get { return _thaiCulture; }
        }

        public static string[] TimeFormats
        {
            get { return _timeFormats; }
        }

        public static bool IsValidMobileNo(string value)
        {
            string s = Regex.Replace(value, "[^0-9]", "");
            return (s.Length == 10 && s[0] == '0');
        }

        public static string FormatDateInterval(DateTime startDate, DateTime endDate)
        {
            string start = "";
            if (startDate == endDate)
            {
                return startDate.ToString("d MMM yy", AppUtils.ThaiCulture);
            }
            else if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
            {
                start = startDate.Day.ToString();
            }
            else
            {
                start = startDate.ToString("d MMM", AppUtils.ThaiCulture);
            }
            return string.Format("{0}-{1}", start, endDate.ToString("d MMM yy", AppUtils.ThaiCulture));
        }

        public static string FormatTimeInterval(TimeSpan? startTime, TimeSpan? endTime)
        {
            if (startTime == null) return "";
            if (startTime == endTime)
            {
                return new DateTime(startTime.Value.Ticks).ToString("H:mm", AppUtils.ThaiCulture);
            }
            else
            {
                return string.Format("{0}-{1}", new DateTime(startTime.Value.Ticks).ToString("H:mm", AppUtils.ThaiCulture),
                    new DateTime(endTime.Value.Ticks).ToString("H:mm", AppUtils.ThaiCulture));
            }
        }

        public static string FormatReportDate(DateTime value)
        {
            return value.ToString("d MMM yy", AppUtils.ThaiCulture);
        }

        public static string FormatReportDateTime(DateTime value)
        {
            return value.ToString("d MMM yy H:mm", AppUtils.ThaiCulture);
        }

        public static string FormatReportTime(TimeSpan? startTime, TimeSpan? endTime)
        {
            if (startTime != null && endTime != null)
            {
                return string.Format("{0}-{1}",
                new DateTime(startTime.Value.Ticks).ToString("H:mm", AppUtils.ThaiCulture),
                new DateTime(endTime.Value.Ticks).ToString("H:mm", AppUtils.ThaiCulture));
            }
            else
            {
                return "";
            }
        }

        public static string CleanFileName(string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        public static string SettingDisplay(string value)
        {
            return value.IsNullOrEmpty() ? "-----" : value;
        }
    }
}