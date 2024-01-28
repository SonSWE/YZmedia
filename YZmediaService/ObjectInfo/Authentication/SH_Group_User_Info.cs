using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo.Users
{
    public class SH_Group_User_Info
    {
        public decimal STT { get; set; }
        public decimal Group_Id { get; set; }
        public decimal User_Id { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public decimal Deleted { get; set; }
    }
}
