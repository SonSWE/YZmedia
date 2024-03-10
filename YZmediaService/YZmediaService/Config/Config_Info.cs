using CommonLib;

namespace YZmediaService
{
    public class Read_Config
    {
        public static void Read_Config_Info(Microsoft.Extensions.Configuration.IConfigurationRoot Configuration)
        {

            try
            {
                //string _tem_Connection = Configuration["DBConnection"]?.ToString();
                //Config_Info.gConnectionString = CommonFunc.DecryptString_AES(_tem_Connection);

                
                //string _tem_connect = Configuration["User_Connect"]?.ToString();
                //Config_Info.c_user_connect = CommonFunc.DecryptString_AES(_tem_connect);

                Config_Info.gConnectionString = Configuration["DBConnection"]?.ToString();
                Config_Info.c_user_connect = Configuration["User_Connect"]?.ToString();

                Config_Info.Jwt_Issuer = Configuration["Jwt:Issuer"]?.ToString();
                Config_Info.Jwt_Key = Configuration["Jwt:Key"]?.ToString();

                Config_Info.TimeOutLogin = Convert.ToInt32(Configuration["TimeOutLogin"]?.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
        }
    }
}
