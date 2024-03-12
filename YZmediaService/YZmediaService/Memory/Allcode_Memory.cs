using CommonLib;
using DataAccess;
using ObjectInfo;

namespace YZmediaService.Memory
{
    public class Allcode_Memory
    {
        static List<YZ_AllCode_Info> _lstAllcode = new();

        public static void Reload()
        {
            try
            {
                List<YZ_AllCode_Info> _lst = new YZ_AllCode_DA().getAllCode();
                _lstAllcode = _lst;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
        }

        public static List<YZ_AllCode_Info> Allcode_GetAll()
        {
            try
            {
                return _lstAllcode
                    .OrderBy(x => x.Cdname)
                    .ThenBy(x => x.Cdtype)
                    .ThenBy(x => x.Lstodr).ToList();
                //return _lstAllcode.OrderBy(x => new { x.Cdname, x.Cdtype, x.Lstodr }).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }

        public static List<YZ_AllCode_Info> Allcode_GetByCdnameCdtype(string cdname, string cdtype)
        {
            try
            {
                return _lstAllcode.Where(x => x.Cdname.Equals(cdname) && x.Cdtype.Equals(cdtype)).OrderBy(x => x.Lstodr).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }

        public static List<YZ_AllCode_Info> Allcode_GetByCdname(string cdname)
        {
            try
            {
                return _lstAllcode.Where(x => x.Cdname.Equals(cdname)).OrderBy(x => x.Lstodr).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }

        public static List<YZ_AllCode_Info> Allcode_GetByCdtype(string cdtype)
        {
            try
            {
                return _lstAllcode.Where(x => x.Cdtype.Equals(cdtype)).OrderBy(x => x.Lstodr).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }

        public static string Allcode_GetContent(string cdname, string cdtype, string cdval)
        {
            try
            {
                YZ_AllCode_Info? _allcode = _lstAllcode.FirstOrDefault(x => x.Cdname.Equals(cdname) && x.Cdtype.Equals(cdtype) && x.Cdval.Equals(cdval));
                if (_allcode != null)
                {
                    return _allcode.Content;
                }

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
            return string.Empty;
        }

        public static List<YZ_AllCode_Info> Allcode_GetByCdtypeCdVal(string cdtype, string vdval)
        {
            try
            {
                return _lstAllcode.Where(x => x.Cdtype.Equals(cdtype) && x.Cdval.Equals(vdval)).OrderBy(x => x.Lstodr).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }

        public static string Allcode_GetContent_GetByCdtypeCdVal(string cdtype, string cdval)
        {
            try
            {
                YZ_AllCode_Info? result = _lstAllcode.FirstOrDefault(x => x.Cdtype == cdtype && x.Cdval == cdval);
                if (result != null)
                {
                    return result.Content;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
            }
            return string.Empty;
        }

        public static List<YZ_AllCode_Info> Allcode_Get_Like_CDVal(string cdname, string cdval)
        {
            try
            {
                return _lstAllcode.Where(x => x.Cdname.Equals(cdname) && x.Cdval.StartsWith(cdval)).OrderBy(x => x.Lstodr).ToList();
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return new List<YZ_AllCode_Info>();
            }
        }
    }
}
