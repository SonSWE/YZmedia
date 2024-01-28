using CommonLib;
using ObjectInfo.Users;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectInfo;
namespace DataAccess
{
    public class SH_GroupRightsDA
    {
        public decimal Group_Rights_Delete_ByGroupId(decimal p_group_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_delete_byGroupId",
                      new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                      paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal Group_Rights_Insert(List<SH_Group_Rights_Info> p_lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_insert", count,
                    new OracleParameter("p_Id", OracleDbType.Decimal, p_lst_data.Select(x=>x.Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Function_Id", OracleDbType.Varchar2, p_lst_data.Select(x=>x.Function_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Authcode", OracleDbType.Varchar2, p_lst_data.Select(x => x.Authcode).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Notes", OracleDbType.Varchar2, p_lst_data.Select(x => x.Notes).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Deleted", OracleDbType.Decimal, p_lst_data.Select(x => x.Deleted).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Created_By", OracleDbType.Varchar2, p_lst_data.Select(x => x.Created_By).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Created_Date", OracleDbType.Date, p_lst_data.Select(x => x.Created_Date).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Modified_By", OracleDbType.Varchar2, p_lst_data.Select(x => x.Modified_By).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Modified_Date", OracleDbType.Date, p_lst_data.Select(x => x.Modified_Date).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_Group_Id", OracleDbType.Decimal, p_lst_data.Select(x => x.Group_Id).ToArray(), ParameterDirection.Input),
                  paramReturn);
                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.ManagedDataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    _result = Convert.ToDecimal(_item.Value.ToString());
                    if (_result < 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
            return _result;
        }





        public decimal Group_Rights_Insert_Batch(List<SH_Group_Rights_Info> p_lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);
                int count = p_lst_data.Count;
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_insert", count,
                  new OracleParameter("p_function_id", OracleDbType.Varchar2, p_lst_data.Select(x => x.Function_Id).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_authcode", OracleDbType.Varchar2, p_lst_data.Select(x => x.Authcode).ToArray(), ParameterDirection.Input),
                  new OracleParameter("p_group_id", OracleDbType.Decimal, p_lst_data.Select(x => x.Group_Id).ToArray(), ParameterDirection.Input),
                  paramReturn);
                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.ManagedDataAccess.Types.OracleDecimal _item in totalReturn)
                {
                    _result = Convert.ToDecimal(_item.Value.ToString());
                    if (_result < 0)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
            return _result;
        }

        

        public decimal Set_Righ_User_InGroup(decimal p_group_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_set_right_for_user_In_group",
                      new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                      paramReturn);
                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }


        public DataSet Get_Group_Rights_By_GroupId(decimal p_group_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_cb_group_rights_by_groupid",
                    new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }


        public DataSet Group_Rights_GetByGroup(  decimal p_group_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_cb_groups_rights_getbyGroup",
                  //new OracleParameter("p_system", OracleDbType.Varchar2, p_system, ParameterDirection.Input),
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


        public DataSet Group_Rights_GetByGroup_V2( decimal p_group_id, string p_user_name)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_RIGHT.proc_cb_groups_rights_getbyGroup_V2",
                  //new OracleParameter("p_system", OracleDbType.Varchar2, p_system, ParameterDirection.Input),
                  new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                  new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
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
