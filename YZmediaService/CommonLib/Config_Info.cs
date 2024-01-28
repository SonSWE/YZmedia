using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class Config_Info
    {
        public static string NumberRedis = "0";

        public static string Jwt_Key = "";
        public static string Jwt_Issuer = "";

        public static int TimeOutLogin = 0;
        public static string ApiClient_CKMS = "";
        public static string ApiClient_SSO = "";
        public static string SH_WEB = "";

        public static string c_user_connect = "SHOLDER_CODE.";
        public static string gConnectionString = "";

        public static string FolderSaveTemp = "";
        //public static SendEmail_Interface c_SendEmail_Interface;
    }
}
