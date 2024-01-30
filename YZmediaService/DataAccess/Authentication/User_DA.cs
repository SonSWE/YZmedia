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
    public class User_DA
    {

        public UserInfo User_Login(string p_user_name, string p_password)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_users.proc_login_user",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, p_password, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<UserInfo>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new UserInfo();
            }
        }

        public DataSet User_Rights_GetByUser(decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_get_all_by_user_id",
                  new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                  new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }
    }
}
