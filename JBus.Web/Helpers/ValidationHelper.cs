using JBus.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JBus.Web.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsValidTextField(ModelStateDictionary modelState, string display, string field, string value, int maxLength, bool required = false)
        {
            bool ret = false;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (value.Length > maxLength)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องมีความยาวไม่เกิน {1} ตัวอักษร", display, maxLength));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidPasswordField(ModelStateDictionary modelState, string display, string field1, string value1, string field2, string value2, int maxLength, bool required = false)
        {
            bool ret = false;
            if (value1.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field1, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (value1.Length > maxLength)
            {
                modelState.AddModelError(field1, string.Format("{0}ต้องมีความยาวไม่เกิน {1} ตัวอักษร", display, maxLength));
            }
            else if (value1 != value2)
            {
                modelState.AddModelError(field2, string.Format("{0}ไม่เหมือนกัน", display));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidEmailField(ModelStateDictionary modelState, string display, string field, string value, int maxLength, bool required = false)
        {
            bool ret = false;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (value.Length > maxLength)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องมีความยาวไม่เกิน {1} ตัวอักษร", display, maxLength));
            }
            else if (!new EmailAddressAttribute().IsValid(value))
            {
                modelState.AddModelError(field, string.Format("{0}มีรูปแบบไม่ถูกต้อง", display));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidSelectField(ModelStateDictionary modelState, string display, string field, int value, bool required = false)
        {
            bool ret = false;
            if (value == 0)
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        public static bool IsValidSelectField(ModelStateDictionary modelState, string display, string field, string value, bool required = false)
        {
            bool ret = false;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            return ret;
        }

        public static bool IsValidThaiDateField(ModelStateDictionary modelState, string display, string field, string value, bool required = false)
        {
            bool ret = false;
            DateTime result;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (!DateTime.TryParseExact(value, new string[] { "dd/MM/yyyy", "d/M/yyyy" }, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
            {
                modelState.AddModelError(field, string.Format("{0}มีรูปแบบไม่ถูกต้อง", display));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidThaiDateRange(ModelStateDictionary modelState, string startDisplay, string endDisplay, string field, string startValue, string endValue, int minDayRange = 0)
        {
            if (!modelState.IsValid) return false; // จะไม่ตรวจถ้ารูปแบบไม่ถูกต้อง เพราะใช้ ParseExact จะเกิด error ได้

            if (!startValue.IsNullOrEmpty() && !endValue.IsNullOrEmpty())
            {
                DateTime startDate = DateTime.ParseExact(startValue, new string[] { "dd/MM/yyyy", "d/M/yyyy" }, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                DateTime endDate = DateTime.ParseExact(endValue, new string[] { "dd/MM/yyyy", "d/M/yyyy" }, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces);
                if ((endDate - startDate).TotalDays < minDayRange)
                {
                    modelState.AddModelError(field, string.Format("{0}อยู่หลัง{1}", startDisplay, endDisplay));
                }
            }
            return true;
        }

        public static bool IsValidTimeField(ModelStateDictionary modelState, string display, string field, string value, bool required = false)
        {
            bool ret = false;
            DateTime result;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (!DateTime.TryParseExact(value, AppUtils.TimeFormats, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces, out result))
            {
                modelState.AddModelError(field, string.Format("{0}มีรูปแบบไม่ถูกต้อง", display));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidTimeRange(ModelStateDictionary modelState, string startDisplay, string endDisplay, string field, string startValue, string endValue, int minMinuteRange = 0)
        {
            if (!modelState.IsValid) return false; // จะไม่ตรวจถ้ารูปแบบไม่ถูกต้อง เพราะใช้ ParseExact จะเกิด error ได้

            if (!startValue.IsNullOrEmpty() && !endValue.IsNullOrEmpty())
            {
                TimeSpan startTime = DateTime.ParseExact(startValue, AppUtils.TimeFormats, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces).TimeOfDay;
                TimeSpan endTime = DateTime.ParseExact(endValue, AppUtils.TimeFormats, AppUtils.ThaiCulture, System.Globalization.DateTimeStyles.AllowWhiteSpaces).TimeOfDay;
                if ((endTime - startTime).TotalMinutes < minMinuteRange)
                {
                    modelState.AddModelError(field, string.Format("{0}อยู่หลัง{1}", startDisplay, endDisplay));
                }
            }
            return true;
        }

        public static bool IsValidNumericField(ModelStateDictionary modelState, string display, string field, int value, int minValue, int maxValue)
        {
            bool ret = false;
            if (value < minValue)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องมากกว่า {1}", display, minValue));
            }
            else if (value > maxValue)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องไม่เกิน {1}", display, maxValue));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidNumericField(ModelStateDictionary modelState, string display, string field, string value, int minValue, int maxValue, bool required = false)
        {
            bool ret = false;
            int result;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (!int.TryParse(value, out result))
            {
                modelState.AddModelError(field, string.Format("{0}ต้องเป็นตัวเลข", display, minValue));
            }
            else if (result < minValue)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องมากกว่า {1}", display, minValue));
            }
            else if (result > maxValue)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องไม่เกิน {1}", display, maxValue));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidBooleanField(ModelStateDictionary modelState, string display, string field, string value, bool required = false)
        {
            bool ret = false;
            bool result;
            if (value.IsNullOrEmpty())
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (!bool.TryParse(value, out result))
            {
                modelState.AddModelError(field, display + "มีค่าไม่ถูกต้อง");
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        public static bool IsValidFileField(ModelStateDictionary modelState, string display, string field, HttpPostedFileBase file, string contentTypes, string displayFileTypes, int maxSizeInMB, bool required = false)
        {
            bool ret = false;
            if (file == null || file.ContentLength == 0)
            {
                if (required)
                {
                    modelState.AddModelError(field, "กรุณาใส่" + display);
                }
                else
                {
                    ret = true;
                }
            }
            else if (!IsValidContentType(file, contentTypes))
            {
                modelState.AddModelError(field, string.Format("{0}ต้องเป็นไฟล์ประเภท {1}", display, displayFileTypes));
            }
            else if (file.ContentLength > 1024 * 1024 * maxSizeInMB)
            {
                modelState.AddModelError(field, string.Format("{0}ต้องมีขนาดไม่เกิน {1} MB", display, maxSizeInMB));
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        private static bool IsValidContentType(HttpPostedFileBase file, string contentTypes)
        {
            bool ret = true;
            if (!contentTypes.IsNullOrEmpty())
            {
                var types = contentTypes.Split(',');
                if (!types.Contains(file.ContentType))
                {
                    ret = false;
                }
            }
            return ret;
        }
    }
}