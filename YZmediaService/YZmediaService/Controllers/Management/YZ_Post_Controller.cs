using CommonLib;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ObjectInfo;


namespace YZmediaService.Controllers.Management
{
    public class YZ_Post_Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //[Authorize]
        [Route("api/manager/posts/get-all")]
        public IActionResult Get_all()
        {
            try
            {
                List<YZ_Post_Info> _lst = YZ_Post_DA.GetInstance().GetAll();
                return Ok(new { jsondata = JsonConvert.SerializeObject(_lst) });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Ok(new { jsondata = JsonConvert.SerializeObject(new List<YZ_Post_Info>()) });
            }
        }

        [HttpGet]
        //[Authorize]
        [Route("api/manager/posts/get-ByID")]
        public IActionResult Get_getByID(string p_user_name, decimal p_id)
        {
            try
            {
                YZ_Post_Info _lst = YZ_Post_DA.GetInstance().Get_getByID(p_id);
                return Json(new { jsondata = JsonConvert.SerializeObject(_lst) });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { jsondata = JsonConvert.SerializeObject(new YZ_Post_Info()) });
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/manager/posts/insert")]
        public IActionResult Insert([FromForm] YZ_Post_Info info)
        {
            decimal _success = -1; string responseMessage = "Thêm mới thất bại!";
            string idFocus = string.Empty;
            List<YZ_FileAttach_Info> _lst_fileAttach = new List<YZ_FileAttach_Info>();
            try
            {
                int numFIle = 0;
                numFIle = UploadFile(ref _lst_fileAttach);
                if (numFIle < 0)
                {
                    responseMessage = "Upload file thất bại!";
                    _success = -4;
                    return Json(new { code = _success.ToString(), _responseMessage = responseMessage });
                }
                else
                {
                    info.list_file_attach = _lst_fileAttach;

                    _success = YZ_Post_DA.GetInstance().Insert(info);

                    if (_success > 0)
                    {
                        responseMessage = "Thêm mới thành công!";
                    }

                    return Json(new { code = _success.ToString(), _responseMessage = responseMessage });
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = _success.ToString(), _responseMessage = responseMessage });
            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/manager/posts/update")]
        public IActionResult Human_update([FromForm] YZ_Post_Info info)
        {

            decimal _success = -1; string responseMessage = "Cập nhập thất bại"; string idFocus = string.Empty;
            List<YZ_FileAttach_Info> _lst_fileAttach = new List<YZ_FileAttach_Info>();
            try
            {
                int numFIle = 0;
                numFIle = UploadFile(ref _lst_fileAttach);
                info.list_file_attach = _lst_fileAttach;

                if (numFIle < 0)
                {
                    responseMessage = "Upload file thất bại!";
                    _success = -4;
                    return Json(new { code = _success.ToString(), _responseMessage = responseMessage });
                }
                _success = YZ_Post_DA.GetInstance().Update(info);
                if (_success > 0)
                {
                    responseMessage = "Cập nhật thành công!";
                }
                return Json(new { code = _success.ToString(), _responseMessage = responseMessage });

            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = -1 });

            }
        }

        [HttpPost]
        //[Authorize]
        [Route("api/manager/posts/delete")]
        public IActionResult Delete(decimal p_id, string p_user_name)
        {
            try
            {
                decimal _result = YZ_Post_DA.GetInstance().Delete(p_id, p_user_name);
                return Json(new { code = _result.ToString() });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Json(new { code = -1 });

            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/manager/posts/search")]
        public IActionResult Search_User(string p_user_name, string keysearch, string from = "0", string to = "0", string p_order_by = "")
        {
            try
            {
                decimal p_total_record = 0;
                List<YZ_Post_Info> _lst = YZ_Post_DA.GetInstance().Search(p_user_name, keysearch, from, to, p_order_by, ref p_total_record);

                return Ok(new { totalrows = p_total_record, jsondata = JsonConvert.SerializeObject(_lst) });
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());
                return Ok(new { totalrows = "0", jsondata = JsonConvert.SerializeObject(new List<YZ_Post_Info>()) });

            }
        }

        private int UploadFile(ref List<YZ_FileAttach_Info> lstFileAttach)
        {
            try
            {
                var formFiles = this.HttpContext.Request.Form.Files;
                if (formFiles != null && formFiles.Count > 0)
                {
                    for (int i = 0; i < formFiles.Count; i++)
                    {
                        IFormFile file = formFiles[i];
                        if (file != null)
                        {
                            string fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmssFFF")}_cg{Path.GetExtension(file.FileName)}";
                            string url_full_url_file = Path.Combine(CommonData.FileAttach, fileName);

                            decimal status_upfile = UploadHelpers.upload_file(file.OpenReadStream(), fileName);
                            if (status_upfile == -1)
                            {
                                return -1;
                            }
                            else
                            {
                                var fileExtension = Path.GetExtension(file.FileName);
                                long fileSizeInBytes = file.Length;
                                double fileSizeInKB = (double)fileSizeInBytes / 1024; // KB
                                double fileSizeInMB = fileSizeInKB / 1024; // MB

                                lstFileAttach.Add(new YZ_FileAttach_Info()
                                {
                                    File_Name = fileName,
                                    File_Url = url_full_url_file,
                                    File_Type = fileExtension,
                                    File_Size = fileSizeInKB.ToString(),
                                    Deleted = 0,
                                });


                            }

                        }
                    }
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Logger.log.Error(ex.ToString());

            }
            return -1;

        }

    }
}
