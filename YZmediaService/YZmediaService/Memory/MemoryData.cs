using CommonLib;
using ObjectInfo;
using System.Data;

namespace YZmediaService
{
    public class MemoryData
    {
        private static Dictionary<string, SystemParamInfo> _dic_SysParam = new Dictionary<string, SystemParamInfo>(); //tham số hện thống

        public static void Load_sysvar()
        {
            try
            {
                //SystemParamDA _SystemParaDA = new SystemParamDA();
                //DataSet _ds = _SystemParaDA.SystemParam_GetAll();
                //List<SystemParamInfo> _lst1 = CBO<SystemParamInfo>.FillCollection_FromDataSet(_ds);

                //foreach (var item in _lst1)
                //{
                //    _dic_SysParam[item.Parakey.ToUpper()] = item;
                //}

                //CommonData.Address_Server_Save_file = Get_Sysvar_By_Key("ADDRESS_SERVER_SAVE_FILE")?.Paravalue;
                //CommonData.FileAttach = Get_Sysvar_By_Key("FILE_ATTACH")?.Paravalue;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
        }

        public static void LoadMemory()
        {
            try
            {
                MemoryData.Load_sysvar();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
        }

        public static SystemParamInfo Get_Sysvar_By_Key(string p_key, string p_token = "")
        {
            try
            {
                if (_dic_SysParam.ContainsKey(p_key.ToUpper()))
                {
                    return _dic_SysParam[p_key.ToUpper()];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return null;
            }
        }
    }
}
