using CommonLib;
using ObjectInfo;
using RestSharp;

namespace YZmediaService
{
    public class ImageTrainningApi
    {
        public static List<YZ_Fileattach_Info> GetbyCode(string p_code)
        {
            try
            {
                var client = new RestClient(CommonData.ApiClient_Service);
                var request = new RestRequest("api/monitor/system-management/board-trade-param/get-by-code", Method.Get);
                //request.AddHeader("Authorization", $"Bearer {Common.CommonData.c_CurrentUserInfo.Token}");

                request.AddParameter("p_code", p_code);

                RestResponse response = client.Execute(request);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<YZ_Fileattach_Info>>(response?.Content ?? "");
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, ex.Message);
                return new List<YZ_Fileattach_Info>();
            }
        }

        public static decimal Insert(YZ_Fileattach_Info p_info)
        {
            try
            {
                var client = new RestClient(CommonData.ApiClient_Service);
                var request = new RestRequest("api/monitor/system-management/board-trade-param/Insert", Method.Post);
                //request.AddHeader("Authorization", $"Bearer {Common.CommonData.c_CurrentUserInfo.Token}");

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(p_info);

                RestResponse response = client.Execute(request);
                decimal _id = Convert.ToDecimal(Newtonsoft.Json.JsonConvert.DeserializeObject<string>(response?.Content ?? ""));
                
                return _id;
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex, ex.Message);
                return -1;
            }
        }
    }
}
