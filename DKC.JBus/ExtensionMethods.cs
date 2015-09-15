using FastMember;
using DKC.JBus.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DKC.JBus
{
    public static class ExtensionMethods
    {
        // Enum
        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            if (field != null)
            {
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    return attr.Description;
                }
            }
            return "";
        }

        //public static int SetAuthCookie<T>(this HttpResponseBase responseBase, string name, bool rememberMe, T userData)
        //{
        //    /// In order to pickup the settings from config, we create a default cookie and use its values to create a
        //    /// new one.
        //    var cookie = FormsAuthentication.GetAuthCookie(name, rememberMe);
        //    var ticket = FormsAuthentication.Decrypt(cookie.Value);

        //    var newTicket = new FormsAuthenticationTicket(ticket.Version, ticket.Name, ticket.IssueDate, ticket.Expiration,
        //        ticket.IsPersistent, userData.ToJson(), ticket.CookiePath);
        //    var encTicket = FormsAuthentication.Encrypt(newTicket);

        //    /// Use existing cookie. Could create new one but would have to copy settings over...
        //    cookie.Value = encTicket;

        //    responseBase.Cookies.Add(cookie);

        //    return encTicket.Length;
        //}

        /// <summary>
        /// Answers true if this String is either null or empty.
        /// </summary>
        /// <remarks>I'm so tired of typing String.IsNullOrEmpty(s)</remarks>
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Answers true if this String is neither null or empty.
        /// </summary>
        /// <remarks>I'm also tired of typing !String.IsNullOrEmpty(s)</remarks>
        public static bool HasValue(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        /// <summary>
        /// Returns the first non-null/non-empty parameter when this String is null/empty.
        /// </summary>
        public static string IsNullOrEmptyReturn(this string s, params string[] otherPossibleResults)
        {
            if (s.HasValue())
                return s;

            for (int i = 0; i < (otherPossibleResults ?? new string[0]).Length; i++)
            {
                if (otherPossibleResults[i].HasValue())
                    return otherPossibleResults[i];
            }

            return "";
        }

        public static T IsNullOrEmptyReturn<T>(this string s)
        {
            if (s.HasValue())
                return (T)Convert.ChangeType(s, typeof(T));
            return default(T);
        }

        public static void SetPageTitle(this WebViewPage page, string title)
        {
            //title = HtmlUtilities.Encode(title);
            page.ViewBag._Title = title;
        }

        // DateTime
        public static string ToThai_d_MMM_yy_Hmm(this DateTime value)
        {
            return value.ToString("d MMM yy H:mm", AppUtils.ThaiCulture);
        }

        public static string ToThai_d_MMM_yy(this DateTime value)
        {
            return value.ToString("d MMM yy", AppUtils.ThaiCulture);
        }

        public static string ToThai_ddMMyyyy(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy", AppUtils.ThaiCulture);
        }

        public static string ToThai_dMyyyy(this DateTime value)
        {
            return value.ToString("d/M/yyyy", AppUtils.ThaiCulture);
        }

        public static string ToThai_dMyyyy(this DateTime? value)
        {
            return (value == null) ? "" : ((DateTime)value).ToThai_dMyyyy();
        }

        public static string ToTime_HHmm(this TimeSpan? value)
        {
            return (value == null) ? "" : new DateTime(value.Value.Ticks).ToString("HH:mm", AppUtils.ThaiCulture);
        }

        public static DateTime? ParseThaiDate(this string value)
        {
            DateTime result;
            if (DateTime.TryParseExact(value, new string[] { "dd/MM/yyyy", "d/M/yyyy" },
                AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.None, out result))
            {
                return result;
            }
            return null;
        }

        // IEnumerable
        public static DataTable CreateDataTable<T>(this IEnumerable<T> source)
        {
            var table = new DataTable();
            using (var reader = ObjectReader.Create(source))
            {
                table.Load(reader);
            }
            return table;
        }

        public static string Absolute(this UrlHelper url, string relativeContentPath)
        {
            Uri contextUri = HttpContext.Current.Request.Url;

            var baseUri = string.Format("{0}://{1}{2}", contextUri.Scheme,
               contextUri.Host, contextUri.Port == 80 ? "" : ":" + contextUri.Port);

            return string.Format("{0}{1}", baseUri, VirtualPathUtility.ToAbsolute(relativeContentPath));
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }
}