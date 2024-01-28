using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class CommonData
    {
        public static string FileAttach = "";
        public static string Link_Server_file = "";
        public static string Address_Server_Save_file = "";
    }

    public static class User_Type
    {
        public const decimal admin = 0;
        public const decimal company = 1;
        public const decimal shareHolder = 2;
    }

    public static class Map_Type
    {
        public const decimal Owner_User_1 = 1;
        public const decimal User_Map_2 = 2;
    }

    public static class Authen_Status
    {
        public const decimal Da_Xac_Thuc_1 = 1;
        public const decimal Chua_Xac_Thuc_0 = 0;
    }

    public static class Authen_Type
    {
        /// <summary>
        /// Xác thực bằng eKYC
        /// </summary>
        public const decimal eKyc_1 = 1;

        /// <summary>
        /// Xác thực bằng SMS
        /// </summary>
        public const decimal Phone_2 = 2;

        /// <summary>
        /// xác thực bằng email
        /// </summary>
        public const decimal Email_3 = 3;

        /// <summary>
        /// Xác thực nhập thông tin sở hữu
        /// </summary>
        public const decimal Onwer_Qtty_4 = 4;

        /// <summary>
        /// Xác thực bằng tài khoản gốc
        /// </summary>
        public const decimal Account_Source_5 = 5;

    }
}
