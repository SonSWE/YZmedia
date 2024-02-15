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
    public class YZ_Post_DA
    {
        public List<YZ_Post_Info> GetAll()
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_YZ_POSTS.PROC_GETALL",
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_Post_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_Post_Info>();
            }
        }
        public YZ_Post_Info Get_getByID(decimal p_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_YZ_POSTS.PROC_GETBYID",
                    new OracleParameter("p_post_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_Post_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new YZ_Post_Info();
            }
        }
        public decimal Insert(YZ_Post_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_POSTS.PROC_INSERT",
                 new OracleParameter("p_post_id", OracleDbType.Decimal, p_data.Post_Id, ParameterDirection.Input),
                 new OracleParameter("p_post_name", OracleDbType.NVarchar2, p_data.Post_Name, ParameterDirection.Input),
                 new OracleParameter("p_post_description", OracleDbType.NVarchar2, p_data.Post_Description, ParameterDirection.Input),
                 new OracleParameter("p_thumbnail_file_id", OracleDbType.Decimal, p_data.Thumbnail_File_Id, ParameterDirection.Input),
                 new OracleParameter("p_is_private", OracleDbType.Decimal, p_data.Is_Private, ParameterDirection.Input),
                 new OracleParameter("p_password", OracleDbType.Varchar2, p_data.Password, ParameterDirection.Input),
                 new OracleParameter("p_status", OracleDbType.Decimal, p_data.Status, ParameterDirection.Input),

                 new OracleParameter("p_Created_By", OracleDbType.Varchar2, p_data.Created_By, ParameterDirection.Input),
                 new OracleParameter("p_Created_Date", OracleDbType.Date, p_data.Created_Date, ParameterDirection.Input),
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
        public decimal Update(YZ_Post_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_POSTS.PROC_UPDATE",
                 new OracleParameter("p_post_id", OracleDbType.Decimal, p_data.Post_Id, ParameterDirection.Input),
                 new OracleParameter("p_post_name", OracleDbType.NVarchar2, p_data.Post_Name, ParameterDirection.Input),
                 new OracleParameter("p_post_description", OracleDbType.NVarchar2, p_data.Post_Description, ParameterDirection.Input),
                 new OracleParameter("p_thumbnail_file_id", OracleDbType.Decimal, p_data.Thumbnail_File_Id, ParameterDirection.Input),
                 new OracleParameter("p_is_private", OracleDbType.Decimal, p_data.Is_Private, ParameterDirection.Input),
                 new OracleParameter("p_password", OracleDbType.Varchar2, p_data.Password, ParameterDirection.Input),
                 new OracleParameter("p_status", OracleDbType.Decimal, p_data.Status, ParameterDirection.Input),

                 new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_data.Modified_By, ParameterDirection.Input),
                 new OracleParameter("p_modified_date", OracleDbType.Date, p_data.Modified_Date, ParameterDirection.Input),
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
        public decimal Delete(decimal p_post_id, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_POSTS.PROC_DELETE",
                    new OracleParameter("p_file_id", OracleDbType.Decimal, p_post_id, ParameterDirection.Input),
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
        public List<YZ_Post_Info> Search(string p_user_name, string p_keysearch, string p_from, string p_to, string p_orderby, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_YZ_POSTS.proc_search",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_keysearch, ParameterDirection.Input),
                    new OracleParameter("p_start_row", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_end_row", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_orderby", OracleDbType.Varchar2, p_orderby, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return CBO<YZ_Post_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_Post_Info>();
            }
        }

    }
}
