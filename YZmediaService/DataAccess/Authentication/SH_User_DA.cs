using CommonLib;
using ObjectInfo;
using ObjectInfo.Users;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Sh_User_DA
    {
        public decimal User_Active_TCPH(decimal p_com_id, decimal p_user_id, string p_key, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_sh_email.proc_active_user_tcph",
                    new OracleParameter("p_com_id", OracleDbType.Decimal, p_com_id, ParameterDirection.Input),
                    new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                    new OracleParameter("p_key", OracleDbType.Varchar2, p_key, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                  paramReturn
                  );
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal Check_Active_Code(decimal p_com_id, decimal p_user_id, string p_active_code, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_sh_email.proc_check_active_code",
                    new OracleParameter("p_com_id", OracleDbType.Decimal, p_com_id, ParameterDirection.Input),
                    new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                    new OracleParameter("p_active_code", OracleDbType.Varchar2, p_active_code, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                  paramReturn
                  );
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public static bool UserMap_Insert(List<SH_User_Map_Info> p_lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_user_map.proc_insert_user_map", count,
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_lst_data.Select(x => x.USER_NAME).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_share_holder_id", OracleDbType.Decimal, p_lst_data.Select(x => x.Share_Holder_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_com_id", OracleDbType.Decimal, p_lst_data.Select(x => x.COM_ID).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_lst_data.Select(x => x.STATUS).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_map_type", OracleDbType.Decimal, p_lst_data.Select(x => x.MAP_TYPE).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_avaiable_map_ekyc", OracleDbType.Decimal, p_lst_data.Select(x => x.AVAIABLE_MAP_EKYC).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_avaiable_map_phone", OracleDbType.Decimal, p_lst_data.Select(x => x.AVAIABLE_MAP_PHONE).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_avaiable_map_email", OracleDbType.Decimal, p_lst_data.Select(x => x.AVAIABLE_MAP_EMAIL).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_avaiable_map_owner_qtty", OracleDbType.Decimal, p_lst_data.Select(x => x.AVAIABLE_MAP_OWNER_QTTY).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_avaiable_map_account_source", OracleDbType.Decimal, p_lst_data.Select(x => x.AVAIABLE_MAP_ACCOUNT).ToArray(), ParameterDirection.Input),

                    new OracleParameter("p_user_map_ekyc", OracleDbType.Varchar2, p_lst_data.Select(x => x.USER_MAP_EKYC).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_user_map_phone", OracleDbType.Varchar2, p_lst_data.Select(x => x.USER_MAP_PHONE).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_user_map_email", OracleDbType.Varchar2, p_lst_data.Select(x => x.USER_MAP_EMAIL).ToArray(), ParameterDirection.Input),

                    new OracleParameter("p_identify_no_map", OracleDbType.Varchar2, p_lst_data.Select(x => x.IDENTIFY_NO_MAP).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_phone_map", OracleDbType.Varchar2, p_lst_data.Select(x => x.PHONE_MAP).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_email_map", OracleDbType.Varchar2, p_lst_data.Select(x => x.EMAIL_MAP).ToArray(), ParameterDirection.Input),

                    new OracleParameter("p_key_crypt", OracleDbType.Varchar2, p_lst_data.Select(x => x.Key_Encrypt).ToArray(), ParameterDirection.Input),

                  paramReturn);
                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.ManagedDataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    _result = Convert.ToDecimal(_item.Value.ToString());
                    if (_result < 0)
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return false;
            }
            return true;
        }

    }
}
