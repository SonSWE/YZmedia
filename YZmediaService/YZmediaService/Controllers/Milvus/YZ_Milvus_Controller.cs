using CommonLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObjectInfo;
using RestSharp;

namespace YZmediaService.Controllers.Milvus
{
    public class YZ_Milvus_Controller : Controller
    {
        private readonly RestClient _client;

        public YZ_Milvus_Controller()
        {
            _client = new RestClient(Config_Info.FlaskApiAddress);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        //[Authorize]
        [Route("api/milvus/collection/create")]
        public async Task<IActionResult> Create([FromForm] YZ_MilvusCollection_Info info)
        {
            decimal _success = -1;
            string _responseMessage = "Thêm mới collection thất bại!";

            try
            {
                var request = new RestRequest("api/CreateCollection").AddJsonBody(info);

                var response = await _client.ExecutePostAsync<BaseInfo>(request);

                if (response != null)
                {
                    _success = response.Data.Code;
                    _responseMessage = response.Data.Message;
                }

                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/milvus/collection/insert")]
        public async Task<IActionResult> Insert([FromForm] YZ_MilvusData_Info info)
        {
            decimal _success = -1;
            string _responseMessage = "Insert data cho collection thất bại!";

            try
            {
                var request = new RestRequest("api/InsertDataCollection").AddJsonBody(info);

                var response = await _client.ExecutePostAsync<BaseInfo>(request);

                if (response != null)
                {
                    _success = response.Data.Code;
                    _responseMessage = response.Data.Message;
                }

                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/milvus/collection/search-image-by-image")]
        public async Task<IActionResult> SearchImageByImage([FromForm] YZ_MilvusData_Info info)
        {
            decimal _success = -1;
            string _responseMessage = "Search data thất bại!";

            try
            {
                var request = new RestRequest("api/SearchImageByImage").AddJsonBody(info);

                var response = await _client.ExecutePostAsync<BaseInfo>(request);

                if (response != null)
                {
                    _success = response.Data.Code;
                    _responseMessage = response.Data.Message;
                }

                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = _success.ToString(), responseMessage = _responseMessage });
            }
        }
    }
}
