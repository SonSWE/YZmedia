using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess
{
    public class SH_UserFunctionDA
    {

        public List<SH_UserFunction_Info> Get_Lst_function_ByUser_ID(decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_get_all_by_user_id",
                    new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<SH_UserFunction_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<SH_UserFunction_Info>();
            }
        }

        public decimal User_Rights_Insert_Batch(List<SH_UserFunction_Info> p_lst_data)
        {
            try
            {
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_insert", count,
                new OracleParameter("p_Id", OracleDbType.Decimal, p_lst_data.Select(x => x.Function_Id).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Username", OracleDbType.Varchar2, p_lst_data.Select(x => x.Username).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Function_Id", OracleDbType.Varchar2, p_lst_data.Select(x => x.Function_Id).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Authcode", OracleDbType.Varchar2, p_lst_data.Select(x => x.Authcode).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Group_Id", OracleDbType.Decimal, p_lst_data.Select(x => x.Group_Id).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Notes", OracleDbType.Varchar2, p_lst_data.Select(x => x.Notes).ToArray(), ParameterDirection.Input),
                new OracleParameter("p_Deleted", OracleDbType.Decimal, p_lst_data.Select(x => x.Deleted), ParameterDirection.Input),
                new OracleParameter("p_Created_By", OracleDbType.Varchar2, p_lst_data.Select(x => x.Created_By), ParameterDirection.Input),
                new OracleParameter("p_Created_Date", OracleDbType.Date, p_lst_data.Select(x => x.Created_Date), ParameterDirection.Input),
                new OracleParameter("p_Modified_By", OracleDbType.Varchar2, p_lst_data.Select(x => x.Modified_By), ParameterDirection.Input),
                new OracleParameter("p_Modified_Date", OracleDbType.Date, p_lst_data.Select(x => x.Modified_Date), ParameterDirection.Input),
                new OracleParameter("p_user_id", OracleDbType.Decimal, p_lst_data.Select(x => x.User_Id), ParameterDirection.Input));

                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal User_Rights_Update_Batch(List<SH_UserFunction_Info> p_lst_data)
        {
            try
            {
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_update_batch", count,
                  new OracleParameter("p_function_id", OracleDbType.Varchar2, p_lst_data.Select(x => x.Function_Id).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_authcode", OracleDbType.Varchar2, p_lst_data.Select(x => x.Authcode).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_modified_by", OracleDbType.NVarchar2, p_lst_data.Select(x => x.Modified_By).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_modified_date", OracleDbType.Date, p_lst_data.Select(x => x.Modified_Date).ToArray(), ParameterDirection.Input));
                return 1;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal User_Rights_Update_Batch_V2(List<SH_UserFunction_Info> p_lst_data)
        {
            try
            {
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_update_batch_V2", count,
                  new OracleParameter("p_function_id", OracleDbType.Varchar2, p_lst_data.Select(x => x.Function_Id).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_user_name", OracleDbType.Varchar2, p_lst_data.Select(x => x.Username).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_authcode", OracleDbType.Varchar2, p_lst_data.Select(x => x.Authcode).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_modified_by", OracleDbType.NVarchar2, p_lst_data.Select(x => x.Modified_By).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_modified_date", OracleDbType.Date, p_lst_data.Select(x => x.Modified_Date).ToArray(), ParameterDirection.Input));
                return 1;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal User_Rights_DelByUser(string p_username)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_delbyUser",
                  new OracleParameter("p_username", OracleDbType.NVarchar2, p_username, ParameterDirection.Input));
                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        // thêm quên cho group thì quyên trong bảng user_right cũng đc thêm
        public decimal Set_Right_ForUser_ByGroup(decimal p_user_id, string p_created_by)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_set_right_for_user",
                  new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                  new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input)
                  );
                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }


        public decimal Reset_Non_Right_User(string p_username)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_reset_non_right_by_user",
                  new OracleParameter("p_username", OracleDbType.NVarchar2, p_username, ParameterDirection.Input));
                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal User_Rights_DelByUser_And_GroupId(string p_username, decimal p_groupid)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_delbyUser_And_GroupId",
                  new OracleParameter("p_username", OracleDbType.NVarchar2, p_username, ParameterDirection.Input),
                  new OracleParameter("p_groupid", OracleDbType.Decimal, p_groupid, ParameterDirection.Input));

                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public DataSet User_Rights_GetByUser(string p_username, decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_getbyUser",
                  new OracleParameter("p_username", OracleDbType.Varchar2, p_username, ParameterDirection.Input),
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

        public DataSet User_Rights_GetByUser_V2(string p_username, decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_getbyUser_v2",
                  new OracleParameter("p_username", OracleDbType.Varchar2, p_username, ParameterDirection.Input),
                  new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public DataSet User_Rights_GetByUser_V3(string p_username, decimal p_user_id, string p_username_use)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_sh_user_rights_getbyUser_v3",
                  new OracleParameter("p_username", OracleDbType.Varchar2, p_username, ParameterDirection.Input),
                  new OracleParameter("p_username_use", OracleDbType.Varchar2, p_username_use, ParameterDirection.Input),
                  new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public DataSet User_Rights_From_Group_GetByUserId(decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_from_group_getByUserId",
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

        public DataSet Get_Right_ByGroupId_InUser_Id(decimal p_user_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_Get_Right_ByGroupId_InUser_Id",
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

        public DataSet User_Rights_From_Group_GetByUserId_And_GroupID(decimal p_user_id, decimal p_group_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_from_group_getByUserId_And_GroupID",
                  new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                  new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                  new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }


        /// <summary>
        /// xóa quyền của user theo user và function
        /// </summary>
        public decimal Delete_User_Right_By_UserFuntion(string p_username, string p_fun_id)
        {
            try
            {
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_delete_funByUser",
                  new OracleParameter("p_username", OracleDbType.NVarchar2, p_username, ParameterDirection.Input),
                  new OracleParameter("p_fun_id", OracleDbType.NVarchar2, p_fun_id, ParameterDirection.Input));
                return 0;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }

        }

        public decimal Update_UserFunction(decimal p_user_id, string p_created_by, string p_list_function_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_USER_RIGHT.proc_update_user_function",
                 new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                 new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                 new OracleParameter("p_list_function_id", OracleDbType.Varchar2, p_list_function_id, ParameterDirection.Input),
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

    }
}
