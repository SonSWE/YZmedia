using CommonLib;
using DataAccess.User;
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
    public class SH_GroupUsersDA
    {
        public DataSet Get_User_By_GroupId(decimal p_group_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_get_users_by_groupid",
                    new OracleParameter("p_group_id", OracleDbType.Decimal, p_group_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public DataSet Get_Groups_By_UserId(decimal p_user_id)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_get_groups_by_userid",
                    new OracleParameter("p_user_id", OracleDbType.Decimal, p_user_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public decimal AddUserToGroup(SH_Group_User_Info groupUsers)
        {
            try
            {
                OracleParameter _result = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_add_user_to_group",
                    new OracleParameter("p_user_id", OracleDbType.Varchar2, groupUsers.User_Id, ParameterDirection.Input),
                    new OracleParameter("p_group_id", OracleDbType.Decimal, groupUsers.Group_Id, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, groupUsers.Created_By, ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, groupUsers.Created_Date, ParameterDirection.Input),
                    _result);

                return Convert.ToDecimal(_result.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal AddListUserToGroup(List<SH_Group_User_Info> _lst_groupUsers)
        {
            try
            {
                decimal _result = -1;

                OracleParameter paramReturn = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_add_user_to_group", _lst_groupUsers.Count,
                    new OracleParameter("p_user_id", OracleDbType.Varchar2, _lst_groupUsers.Select(x=>x.User_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_group_id", OracleDbType.Decimal, _lst_groupUsers.Select(x=>x.Group_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, _lst_groupUsers.Select(x => x.Created_By).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, _lst_groupUsers.Select(x => x.Created_Date).ToArray(), ParameterDirection.Input),
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
                return _result;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }


        public decimal RemoveUserFromGroup(SH_Group_User_Info groupUsers)
        {
            try
            {
                decimal _result = -1;
                OracleParameter paramReturn = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_remove_user_from_group",
                    new OracleParameter("p_user_id", OracleDbType.Varchar2, groupUsers.User_Id, ParameterDirection.Input),
                    new OracleParameter("p_group_id", OracleDbType.Decimal, groupUsers.Group_Id, ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, groupUsers.Created_By, ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, groupUsers.Created_Date, ParameterDirection.Input),
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
                return _result;

                // return Convert.ToDecimal(_result.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }


        public decimal RemoveListUserFromGroup(List<SH_Group_User_Info> List_groupUsers)
        {
            try
            {
                decimal _result = -1;
                OracleParameter paramReturn = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, 
                    Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_remove_user_from_group", List_groupUsers.Count,
                    new OracleParameter("p_user_id", OracleDbType.Varchar2, List_groupUsers.Select(x=>x.User_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_group_id", OracleDbType.Decimal, List_groupUsers.Select(x=>x.Group_Id).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_created_by", OracleDbType.Varchar2, List_groupUsers.Select(x=>x.Created_By).ToArray(), ParameterDirection.Input),
                    new OracleParameter("p_created_date", OracleDbType.Date, List_groupUsers.Select(x=>x.Created_By).ToArray(), ParameterDirection.Input),
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
                return _result;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal RemoveAllByUser(decimal user_id)
        {
            try
            {
                OracleParameter _result = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_GROUP_USER.proc_remove_all_by_user",
                    new OracleParameter("p_user_id", OracleDbType.Decimal, user_id, ParameterDirection.Input),
                    _result);
                return Convert.ToDecimal(_result.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }
    }
}
