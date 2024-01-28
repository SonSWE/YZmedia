using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class SH_UserFunction_Info
    {
        public decimal STT { get; set; }
       
        public decimal User_Id { get; set; }
        public string Username { get; set; }
        public string Function_Id { get; set; }
        public string Function_Pri_Id { get; set; }
        public string Function_Name { get; set; }
        public string Authcode { get; set; }
        public decimal Group_Id { get; set; }
        public string Notes { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }

        public string AllowAccess { get; set; }
    }

   

    




    

}
