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
using System.Xml.Linq;

namespace DataAccess
{
    public class SH_GroupDA
    {
        public DataSet Group_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.proc_getall",
                 new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public DataSet Group_Get_by_Group_Name(string p_group_name)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.proc_get_by_group_name",
                    new OracleParameter("p_group_name", OracleDbType.Varchar2, p_group_name, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {

                Logger.log.Error(ex.ToString()); 
                return null;
            }
        }

        public List<SH_Group_Info> Group_Search(string p_user_name, string p_keysearch, string p_from, string p_to, string p_orderby, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_SH_GROUP.proc_search",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_keysearch", OracleDbType.Varchar2, p_keysearch, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_orderby", OracleDbType.Varchar2, p_orderby, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return CBO<SH_Group_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<SH_Group_Info>();
            }
        }



        public SH_Group_Info Group_GetById( decimal p_id) 
        {
            try
            {
                DataSet ds= OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.proc_get_by_groupid",
                    new OracleParameter("p_group_id", OracleDbType.Varchar2, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                return CBO<SH_Group_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }

        public decimal Group_Insert(SH_Group_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.Proc_sh_group_Insert",
                   new OracleParameter("p_Group_Id", OracleDbType.Decimal, p_data.Group_Id, ParameterDirection.Input),
                    new OracleParameter("p_Group_Name", OracleDbType.Varchar2, p_data.Group_Name, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Varchar2, p_data.Status, ParameterDirection.Input),
                    new OracleParameter("p_Note", OracleDbType.Varchar2, p_data.Note, ParameterDirection.Input),
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

        public decimal Group_Update(SH_Group_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.proc_sh_group_update", 
                 new OracleParameter("p_Group_Id", OracleDbType.Decimal, p_data.Group_Id, ParameterDirection.Input),
                    new OracleParameter("p_Group_Name", OracleDbType.Varchar2, p_data.Group_Name, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Varchar2, p_data.Status, ParameterDirection.Input),
                    new OracleParameter("p_Note", OracleDbType.Varchar2, p_data.Note, ParameterDirection.Input),
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

        public decimal Group_Delete(decimal groupInfo_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_SH_GROUP.proc_delete",
                  new OracleParameter("p_group_id", OracleDbType.Decimal, groupInfo_id, ParameterDirection.Input),
                  paramReturn);
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
