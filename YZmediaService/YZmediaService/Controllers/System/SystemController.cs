using CommonLib;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObjectInfo;
using YZmediaService.Memory;
using System.Data;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace YZmediaService.Controllers.System
{
    [ApiController]
    public class SystemController : Controller
    {
        [Route("api/sh/allcode/get-all"), HttpGet]
        public IActionResult Allcode_GetAll()
        {
            try
            {
                List<YZ_Allcode_Info> data = Allcode_Memory.Allcode_GetAll();
                return Ok(new { jsondata = JsonConvert.SerializeObject(data) });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { jsondata = JsonConvert.SerializeObject(new List<YZ_Allcode_Info>()) });
            }
        }

        [Route("api/sh/allcode/get-by-cdname-cdtype"), HttpGet]
        public IActionResult Allcode_GetByCdNameCdType(string cdName, string cdType)
        {
            try
            {
                List<YZ_Allcode_Info> data = Allcode_Memory.Allcode_GetByCdnameCdtype(cdName, cdType);
                return Ok(new { jsondata = JsonConvert.SerializeObject(data) });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return StatusCode((int)HttpStatusCode.InternalServerError, new { jsondata = JsonConvert.SerializeObject(new List<YZ_Allcode_Info>()) });
            }
        }
    }
}
