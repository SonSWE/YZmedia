using CommonLib;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Net;

namespace YZmediaService
{
    public class UploadHelpers
    {
        /// upload file  thẳng nên máy chứa code, cần fpt
        /// 
        public static decimal upload_file(Stream fileStream, string fileName)
        {
            decimal status_upfile = -1;
            try
            {
                if (fileStream != null && fileStream.Length > 0)
                {
                    var filePath = Path.Combine(CommonData.Link_Server_file, fileName); // Thay đổi đường dẫn và tên tệp cần lưu
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                         fileStream.CopyTo(stream);
                    }
                    status_upfile = 1;
                }
                return status_upfile;
            }
            catch (Exception ex)
            {

                Logger.log.Error(ex.ToString());
                return status_upfile;
            }

        }
    }
}
