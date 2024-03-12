using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class YZ_FileAttach_Info
    {
        public decimal File_Id { get; set; }
        public string File_Name { get; set; }
        public string File_Url { get; set; }
        public string File_Size { get; set; }
        public string File_Type { get; set; }
        public decimal Status { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
    }
}
