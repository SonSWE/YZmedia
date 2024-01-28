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

namespace DataAccess.User
{
    public class SH_Functions_DA
    {

        public DataSet GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_HRM_FUNCTION.proc_getall",
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                    );
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new DataSet();
            }
        }

        public DataSet Search(string function_name, string function_type, string orderby, int startrecord, int endrecord, ref int totalrows)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_totalrows", OracleDbType.Int32, ParameterDirection.Output);
                paramReturn.Size = 8;

                DataSet _ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_HRM_FUNCTION.proc_search",
                     new OracleParameter("p_function_name", OracleDbType.Varchar2, function_name, ParameterDirection.Input),
                     new OracleParameter("p_function_type", OracleDbType.Varchar2, function_type, ParameterDirection.Input),
                     new OracleParameter("p_startrecord", OracleDbType.Decimal, startrecord, ParameterDirection.Input),
                     new OracleParameter("p_endrecord", OracleDbType.Decimal, endrecord, ParameterDirection.Input),
                     new OracleParameter("p_orderby", OracleDbType.Varchar2, orderby, ParameterDirection.Input),
                     paramReturn,
                     new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                     );

                totalrows = Convert.ToInt32(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new DataSet();
            }
        }
         
        public List<SH_Functions_Info> Function_Search(string p_user_name, string p_keysearch, string p_from, string p_to, string p_orderby, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_HRM_FUNCTION.proc_search_function",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_keysearch", OracleDbType.Varchar2, p_keysearch, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_orderby", OracleDbType.Varchar2, p_orderby, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return CBO<SH_Functions_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<SH_Functions_Info>();
            }
        }

        public decimal Insert_Function(SH_Functions_Info p_data)
        {
            try
            {
                OracleParameter _return = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_HRM_FUNCTION.proc_hrm_functions_insert",
                     new OracleParameter("p_Id", OracleDbType.NVarchar2, p_data.Id, ParameterDirection.Input),
                    new OracleParameter("p_Prid", OracleDbType.NVarchar2, p_data.Prid, ParameterDirection.Input),
                    new OracleParameter("p_Name", OracleDbType.Varchar2, p_data.Name, ParameterDirection.Input),
                    new OracleParameter("p_Name_Eng", OracleDbType.Varchar2, p_data.Name_Eng, ParameterDirection.Input),
                    new OracleParameter("p_Objname", OracleDbType.Varchar2, p_data.Objname, ParameterDirection.Input),
                    new OracleParameter("p_Function_Url", OracleDbType.Varchar2, p_data.Function_Url, ParameterDirection.Input),
                    new OracleParameter("p_Function_Url_Post", OracleDbType.Varchar2, p_data.Function_Url_Post, ParameterDirection.Input),
                    new OracleParameter("p_Lev", OracleDbType.Decimal, p_data.Lev, ParameterDirection.Input),
                    new OracleParameter("p_Last", OracleDbType.Varchar2, p_data.Last, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Varchar2, p_data.Status, ParameterDirection.Input),
                    new OracleParameter("p_System_Type", OracleDbType.Varchar2, p_data.System_Type, ParameterDirection.Input),
                    new OracleParameter("p_Notes", OracleDbType.Varchar2, p_data.Notes, ParameterDirection.Input),
                    new OracleParameter("p_Deleted", OracleDbType.Decimal, p_data.Deleted, ParameterDirection.Input),
                    new OracleParameter("p_Created_By", OracleDbType.Varchar2, p_data.Created_By, ParameterDirection.Input),
                    new OracleParameter("p_Created_Date", OracleDbType.Date, p_data.Created_Date, ParameterDirection.Input),
                    new OracleParameter("p_Modified_By", OracleDbType.Varchar2, p_data.Modified_By, ParameterDirection.Input),
                    new OracleParameter("p_Modified_Date", OracleDbType.Date, p_data.Modified_Date, ParameterDirection.Input),
                    new OracleParameter("p_Position", OracleDbType.Decimal, p_data.Position, ParameterDirection.Input),
                    new OracleParameter("p_Display_On_Menu", OracleDbType.Decimal, p_data.Display_On_Menu, ParameterDirection.Input),
                    new OracleParameter("p_Description", OracleDbType.Varchar2, p_data.Description, ParameterDirection.Input),
                   
                    _return);

                return Convert.ToDecimal(_return.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }

        public decimal Update_function(SH_Functions_Info p_data)
        {
            try
            {
                OracleParameter _return = new OracleParameter("p_result", OracleDbType.Decimal, ParameterDirection.Output);

                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_HRM_FUNCTION.proc_update",
                    new OracleParameter("p_Id", OracleDbType.NVarchar2, p_data.Id, ParameterDirection.Input),
                    new OracleParameter("p_Prid", OracleDbType.NVarchar2, p_data.Prid, ParameterDirection.Input),
                    new OracleParameter("p_Name", OracleDbType.Varchar2, p_data.Name, ParameterDirection.Input),
                    new OracleParameter("p_Name_Eng", OracleDbType.Varchar2, p_data.Name_Eng, ParameterDirection.Input),
                    new OracleParameter("p_Objname", OracleDbType.Varchar2, p_data.Objname, ParameterDirection.Input),
                    new OracleParameter("p_Function_Url", OracleDbType.Varchar2, p_data.Function_Url, ParameterDirection.Input),
                    new OracleParameter("p_Function_Url_Post", OracleDbType.Varchar2, p_data.Function_Url_Post, ParameterDirection.Input),
                    new OracleParameter("p_Lev", OracleDbType.Decimal, p_data.Lev, ParameterDirection.Input),
                    new OracleParameter("p_Last", OracleDbType.Varchar2, p_data.Last, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Varchar2, p_data.Status, ParameterDirection.Input),
                    new OracleParameter("p_System_Type", OracleDbType.Varchar2, p_data.System_Type, ParameterDirection.Input),
                    new OracleParameter("p_Notes", OracleDbType.Varchar2, p_data.Notes, ParameterDirection.Input),
                    new OracleParameter("p_Deleted", OracleDbType.Decimal, p_data.Deleted, ParameterDirection.Input),
                    new OracleParameter("p_Created_By", OracleDbType.Varchar2, p_data.Created_By, ParameterDirection.Input),
                    new OracleParameter("p_Created_Date", OracleDbType.Date, p_data.Created_Date, ParameterDirection.Input),
                    new OracleParameter("p_Modified_By", OracleDbType.Varchar2, p_data.Modified_By, ParameterDirection.Input),
                    new OracleParameter("p_Modified_Date", OracleDbType.Date, p_data.Modified_Date, ParameterDirection.Input),
                    new OracleParameter("p_Position", OracleDbType.Decimal, p_data.Position, ParameterDirection.Input),
                    new OracleParameter("p_Display_On_Menu", OracleDbType.Decimal, p_data.Display_On_Menu, ParameterDirection.Input),
                    new OracleParameter("p_Description", OracleDbType.Varchar2, p_data.Description, ParameterDirection.Input),
                    
                    _return);

                return Convert.ToDecimal(_return.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return -1;
            }
        }


        public decimal delete_function(decimal p_function_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_HRM_FUNCTION.pro_delete_function",
                  new OracleParameter("p_function_id", OracleDbType.Decimal, p_function_id, ParameterDirection.Input),
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
