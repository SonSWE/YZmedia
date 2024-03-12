using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class YZ_Post_Info
    {
        public decimal Post_Id { get; set; }
        public string Post_Name { get; set; }
        public string Post_Description { get; set; }
        public decimal Thumbnail_File_Id { get; set; }
        public decimal Is_Private { get; set; }
        public string Password { get; set; }
        public decimal Status { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }

        public List<YZ_FileAttach_Info> list_file_attach  = new List<YZ_FileAttach_Info>();
    }
}
