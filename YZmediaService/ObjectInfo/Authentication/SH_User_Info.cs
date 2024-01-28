using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class SH_User_Info
    {
        public decimal User_Id { get; set; }
        public string User_Name { get; set; }
        public string Full_Name { get; set; }
        public decimal Reference_Id { get; set; }
        public decimal User_Type { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Status_Text { get; set; }

        public decimal Is_Reset_Pass { get; set; }
        public DateTime Last_Update_Pass { get; set; }
        public decimal Number_Set_Right { get; set; }
        public decimal Authen_Type { get; set; }
        public decimal Register_Type { get; set; }
        public decimal First_Time_Change_Pass { get; set; }
        public decimal Is_Logout_Change_Pass { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
    }
}
