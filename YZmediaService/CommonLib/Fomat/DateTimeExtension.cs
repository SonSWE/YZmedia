using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public static class DateTimeExtension
    {

        public static DateTime FirstDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public static DateTime LastDateOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        public static DateTime NextWorkingDay(this DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Friday: return dateTime.AddDays(3);
                case DayOfWeek.Saturday: return dateTime.AddDays(2);
                default: return dateTime.AddDays(1);
            }
        }

        /// <summary>
        /// Format dd/MM/yyyy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateStringN0(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.DATE_FORMAT_N0, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format dd/MM/yy
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateStringN1(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.DATE_FORMAT_N1, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Format yyMMdd
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateStringY2(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.DATE_FORMAT_Y2);
        }

        /// <summary>
        /// Format HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToTimeStringN0(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.TIME_FORMAT_N0);
        }

        /// <summary>
        /// Format dd/MM/yyyy - HH:mm:ss
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStringN0(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.DATETIME_FORMAT_N0);
        }

        /// <summary>
        /// Format dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStringN1(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : dateTime.ToString(DatetimeFormat.DATETIME_FORMAT_N1);
        }

        public static string To_First_Date_Of_Month_Text(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : new DateTime(dateTime.Year, dateTime.Month, 1).ToString(DatetimeFormat.DATE_FORMAT_N0);
        }

        public static string To_Last_Date_Of_Month_Text(this DateTime dateTime)
        {
            return (dateTime == null || dateTime <= DateTime.MinValue) ? string.Empty : new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1).ToString(DatetimeFormat.DATE_FORMAT_N0);
        }
    }


    public class DatetimeFormat
    {
        public const string DATE_MARK = "__/__/____";
        public const string DATE_FORMAT_N0 = "dd/MM/yyyy";
        public const string DATE_FORMAT_N1 = "dd/MM/yy";
        public const string DATE_FORMAT_Y2 = "yyMMdd";
        public const string TIME_FORMAT_N0 = "HH:mm:ss";
        public const string DATETIME_FORMAT_N0 = "dd/MM/yyyy - HH:mm:ss";
        public const string DATETIME_FORMAT_N1 = "dd/MM/yyyy HH:mm";
    }
}
