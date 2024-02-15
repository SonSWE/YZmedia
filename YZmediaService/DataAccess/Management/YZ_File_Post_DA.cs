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
    public class YZ_File_Post_DA
    {
        public YZ_File_Post_Info Get_Lst_getByPostID(decimal p_id)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "PKG_YZ_FILE_POST.PROC_GETBYPOSTID",
                    new OracleParameter("p_post_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_File_Post_Info>.FillObjectFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new YZ_File_Post_Info();
            }
        }


        public decimal Insert(YZ_File_Post_Info p_data)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_FILE_POST.PROC_INSERT",
                 new OracleParameter("p_file_id", OracleDbType.Decimal, p_data.File_Id, ParameterDirection.Input),
                 new OracleParameter("p_post_id", OracleDbType.Decimal, p_data.Post_Id, ParameterDirection.Input),
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

        public decimal Insert_FileAttach_Batch(List<YZ_File_Post_Info> lst_data)
        {
            decimal _result = 1;
            try
            {
                OracleParameter paraResult = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_FILE_POST.PROC_INSERT", lst_data.Count,
                 new OracleParameter("p_file_id", OracleDbType.Decimal, lst_data.Select(x => x.File_Id).ToArray(), ParameterDirection.Input),
                 new OracleParameter("p_post_id", OracleDbType.Decimal, lst_data.Select(x => x.Post_Id).ToArray(), ParameterDirection.Input),
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

        public decimal Delete_ByPostId(decimal p_id)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Config_Info.gConnectionString, CommandType.StoredProcedure, Config_Info.c_user_connect + "PKG_YZ_FILE_POST.PROC_DELETE_BYPOSTID",
                    new OracleParameter("p_post_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
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
