using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class SH_Group_Rights_Info
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Function_Id { get; set; }
        public string Function_Name { get; set; }
        public string Authcode { get; set; }
        public string Notes { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
        public decimal Group_Id { get; set; }
        public string prid { get; set; }

        public string last { get; set; }
        public decimal Lev { get; set; }
    }
}
