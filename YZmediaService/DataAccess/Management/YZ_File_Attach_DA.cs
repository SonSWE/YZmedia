using CommonLib;
using ObjectInfo;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class YZ_File_Attach_DA
    {
        public YZ_Fileattach_Info Get_Lst_getByID(decimal p_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.proc_getbyid",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_Fileattach_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new YZ_Fileattach_Info();
            }
        }


        public decimal Insert_FileAttach(YZ_Fileattach_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.Proc_Insert",
                 new OracleParameter("p_File_Id", OracleDbType.Decimal, p_data.File_Id, ParameterDirection.Input),
                 new OracleParameter("p_File_Name", OracleDbType.Varchar2, p_data.File_Name, ParameterDirection.Input),
                 new OracleParameter("p_File_Url", OracleDbType.Varchar2, p_data.File_Url, ParameterDirection.Input),
                 new OracleParameter("p_File_Size", OracleDbType.Varchar2, p_data.File_Size, ParameterDirection.Input),
                 new OracleParameter("p_File_Type", OracleDbType.Varchar2, p_data.File_Type, ParameterDirection.Input),
                 new OracleParameter("p_Deleted", OracleDbType.Decimal, p_data.Deleted, ParameterDirection.Input),
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

        public decimal Update_FileAttach(YZ_Fileattach_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.proc_update",
                 new OracleParameter("p_File_Id", OracleDbType.Decimal, p_data.File_Id, ParameterDirection.Input),
                 new OracleParameter("p_File_Name", OracleDbType.Varchar2, p_data.File_Name, ParameterDirection.Input),
                 new OracleParameter("p_File_Url", OracleDbType.Varchar2, p_data.File_Url, ParameterDirection.Input),
                 new OracleParameter("p_File_Size", OracleDbType.Varchar2, p_data.File_Size, ParameterDirection.Input),
                 new OracleParameter("p_File_Type", OracleDbType.Varchar2, p_data.File_Type, ParameterDirection.Input),
                 new OracleParameter("p_Deleted", OracleDbType.Decimal, p_data.Deleted, ParameterDirection.Input),
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


        public decimal Insert_FileAttach_Batch(List<YZ_Fileattach_Info> lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paraResult = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.Proc_Insert", lst_data.Count,
                 new OracleParameter("p_File_Id", OracleDbType.Decimal, lst_data.Select(x => x.File_Id).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Name", OracleDbType.Varchar2, lst_data.Select(x => x.File_Name).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Url", OracleDbType.Varchar2, lst_data.Select(x => x.File_Url).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Size", OracleDbType.Varchar2, lst_data.Select(x => x.File_Size).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Type", OracleDbType.Varchar2, lst_data.Select(x => x.File_Type).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Deleted", OracleDbType.Decimal, lst_data.Select(x => x.Deleted).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Created_By", OracleDbType.Varchar2, lst_data.Select(x => x.Created_By).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Created_Date", OracleDbType.Date, lst_data.Select(x => x.Created_Date).ToArray(), ParameterDirection.Input),
                 paraResult);

                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paraResult.Value;
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
            }

            return _result;
        }


        public decimal Update_FileAttach_Batch(List<YZ_Fileattach_Info> lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paraResult = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.proc_update", lst_data.Count,
                 new OracleParameter("p_file_id", OracleDbType.Decimal, lst_data.Select(x => x.File_Id).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_file_name", OracleDbType.Varchar2, lst_data.Select(x => x.File_Name).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_file_url", OracleDbType.Varchar2, lst_data.Select(x => x.File_Url).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Size", OracleDbType.Varchar2, lst_data.Select(x => x.File_Size).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_File_Type", OracleDbType.Varchar2, lst_data.Select(x => x.File_Type).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Deleted", OracleDbType.Decimal, lst_data.Select(x => x.Deleted).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Created_By", OracleDbType.Varchar2, lst_data.Select(x => x.Created_By).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_Created_Date", OracleDbType.Date, lst_data.Select(x => x.Created_Date).ToArray(), ParameterDirection.Input),
                 paraResult);

                Oracle.ManagedDataAccess.Types.OracleDecimal[] totalReturn = (Oracle.ManagedDataAccess.Types.OracleDecimal[])paraResult.Value;
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
            }

            return _result;
        }

        




        public decimal Delete_FileAttach( decimal p_file_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "pkg_YZ_FILEATTACH.pro_delete_attach_file",
                    new OracleParameter("p_file_id", OracleDbType.Decimal, p_file_id, ParameterDirection.Input),
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
