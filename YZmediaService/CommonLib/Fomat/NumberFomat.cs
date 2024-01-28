using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Fomat
{
    public class NumberFormat
    {
        public const string NUMBER_FORMAT_2_DECIMAL = "_( #,##0.00_);_( (#,##0.00);_(* \"0\"??_);_(@_)";
        public const string NUMBER_FORMAT_PERCENTAGE = "0.00%";
        public const string NUMBER_FORMAT_INT = "_( #,##0_);_( (#,##0);_(* \"0\"_);_(@_)";
        public const string NUMBER_FORMAT_N0_0 = "#,###";
        public const string NUMBER_FORMAT_N0_1 = "#,##0";
        public const string NUMBER_FORMAT_N1_0 = "#,###.#";
        public const string NUMBER_FORMAT_N1_1 = "#,##0.#";
        public const string NUMBER_FORMAT_N2_0 = "#,###.##";
        public const string NUMBER_FORMAT_N2_1 = "#,##0.##";
        public const string NUMBER_FORMAT_N3_0 = "#,###.###";
        public const string NUMBER_FORMAT_N3_1 = "#,##0.###";
        public const string NUMBER_FORMAT_RATE = "#,##0.00";
        public const string NUMBER_FORMAT_RATE_3 = "#,##0.000";
        public const string NUMBER_FORMAT_RATE_4 = "#,##0.#0####";
        public const string NUMBER_FORMAT_N4_1 = "#,##0.####";
        public const string NUMBER_FORMAT_RATE_5 = "#,##0.#0";
        public const string NUMBER_FORMAT_RATE_6 = "#,##0.#00";
        public const string NUMBER_FORMAT_RATE_7 = "#,##0.####";
        public const string NUMBER_FORMAT_N10 = "#,##0.##########";
        public const string NUMBER_FORMAT_4Digit = "#,##0.###0";
    }

    public static class NumberFormatExtension
    {
        public static string ToNumberStringN0(this decimal number)
        {
            //return ((number > 0 && number < 1) ? number.ToString(NumberFormat.NUMBER_FORMAT_N0_1) : number.ToString(NumberFormat.NUMBER_FORMAT_N0_0));
            try
            {
                return Convert.ToInt64(number).ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static string ToNumberStringN10(this decimal number)
        {
            //return ((number > 0 && number < 1) ? number.ToString(NumberFormat.NUMBER_FORMAT_N0_1) : number.ToString(NumberFormat.NUMBER_FORMAT_N0_0));
            return number.ToString(NumberFormat.NUMBER_FORMAT_N0_1);
        }

        // 1 digit after decimal point
        public static string ToNumberStringN1(this decimal number)
        {
            return ((number > 0 && number < 1) ? number.ToString(NumberFormat.NUMBER_FORMAT_N1_1) : number.ToString(NumberFormat.NUMBER_FORMAT_N1_0));
        }

        // 2 digits after decimal point
        public static string ToNumberStringN2(this decimal number)
        {
            return ((number > 0 && number < 1) ? number.ToString(NumberFormat.NUMBER_FORMAT_N2_1) : number.ToString(NumberFormat.NUMBER_FORMAT_N2_0));
        }

        // 3 digits after decimal point
        public static string ToNumberStringN3(this decimal number)
        {
            return ((number > 0 && number < 1) ? number.ToString(NumberFormat.NUMBER_FORMAT_N2_1) : number.ToString(NumberFormat.NUMBER_FORMAT_N3_0));
        }

        /// <summary>
        /// Giữ nguyên giá trị, thêm dấu ngăn cách phần nghìn, trả về empty string nếu = 0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberStringN4(this decimal number)
        {
            return (number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_N0_1));
        }

        /// <summary>
        /// Giữ nguyên giá trị, thêm dấu ngăn cách phần nghìn, trả về empty string nếu = 0 hoặc =-1
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberStringN5(this decimal number)
        {
            return (number <= 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_N0_1));
        }


        /// <summary>
        /// Giữ nguyên giá trị, thêm dấu phân cách phần nghìn
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberRate(this decimal number)
        {
            return number.ToString(NumberFormat.NUMBER_FORMAT_N10);
        }

        /// <summary>
        /// Giữ nguyên giá trị, trả về empty string nếu = 0
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberRate_2(this decimal number)
        {
            return (number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_N10));
        }

        public static string ToNumberRate_3(this decimal number)
        {
            return number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_N10);
        }

        //Thêm đuôi %, 0 để trống
        public static string ToNumber_Fee_Rate(this decimal number)
        {
            return number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_N10) + "%";
        }

        public static string ToNumberRate_4(this decimal number)
        {
            return (number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_RATE_4));
        }

        public static string ToNumberRate_4Digit(this decimal number)
        {
            return (number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_4Digit));
        }
        public static string ToNumberRate_41(this decimal number)
        {
            return (number == 0 ? "0" : number.ToString(NumberFormat.NUMBER_FORMAT_RATE_4));
        }
        /// <summary>
        /// Cắt 4 giá trị sau thập phân, trả về 0.0000 nếu = 0 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberRate_42(this decimal number)
        {
            return (number == 0 ? "0.0000" : number.ToString(NumberFormat.NUMBER_FORMAT_N4_1));
        }
        public static string ToNumberRate_7(this decimal number)
        {
            //if (number == 0 || number == -1)
            //    return "";
            //else
            return number.ToString(NumberFormat.NUMBER_FORMAT_RATE_7);
        }

        public static string ToIntNumber_Fload_Per(this decimal number)
        {
            string str_value = number.ToString("#,##0.##");
            return (decimal)number == 0 ? "0" : (decimal)number == 100 ? "100" : str_value;
        }


        /// <summary>
        /// Cắt 2 số sau dấu thập phân, không làm tròn 
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberRate_5(this decimal number)
        {
            return number == 0 ? "" : (Math.Truncate(100 * number) / 100).ToString(NumberFormat.NUMBER_FORMAT_RATE_5);
        }

        /// <summary>
        /// Cắt 3 số sau dấu thập phân, có làm tròn
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ToNumberRate_6(this decimal number)
        {
            return (number == 0 ? "" : number.ToString(NumberFormat.NUMBER_FORMAT_RATE_6));
        }
        public static string ZeroIfEmpty(this string s)
        {
            return string.IsNullOrEmpty(s) ? "0" : s;
        }
    }

}
