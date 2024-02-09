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

    public class YZ_AllCode_DA
    {
        public List<YZ_Allcode_Info> getAllCode_cdname_cdtype(string p_cdname, string p_cdtype)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "pkg_allcode.proc_get_By_cdname_cdtype",
                    new OracleParameter("p_cdname", OracleDbType.Varchar2, p_cdname, ParameterDirection.Input),
                    new OracleParameter("p_cdtype", OracleDbType.Varchar2, p_cdtype, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_Allcode_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_Allcode_Info>();
            }
        }


        public List<YZ_Allcode_Info> getAllCode()
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Config_Info.gConnectionString, CommandType.StoredProcedure,
                    Config_Info.c_user_connect + "pkg_allcode.proc_get_allCode",
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return CBO<YZ_Allcode_Info>.FillCollection_FromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_Allcode_Info>();
            }
        }
    }

}
