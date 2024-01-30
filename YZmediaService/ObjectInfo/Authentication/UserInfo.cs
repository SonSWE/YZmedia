﻿using System.Security.Cryptography.Xml;

namespace ObjectInfo
{
    public class UserInfo
    {
        public decimal STT { get; set; }
        public string? User_Name { get; set; }
        public string Full_Name { get; set; }

        public string? Password { get; set; }
        public string? Status { get; set; }
        public string? Status_Text { get; set; }
        public decimal User_Type { get; set; }
        public string? User_Type_Text { get; set; }


        public decimal User_Id { get; set; }
        public decimal Reference_Id { get; set; }

        public string? Lst_Com_Id { get; set; }
        public decimal Com_Id { get; set; }
        public string? Com_Name { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Token { get; set; }

        public List<UserFunction> FunctionSettings = new List<UserFunction>();
    }

    public class SAUser
    {
        public decimal User_Id { get; set; }
        public string Full_Name { get; set; }
        public decimal User_Type { get; set; }
        public string User_Name { get; set; }
        public decimal Brid { get; set; }
        public decimal DPId { get; set; }
        public DateTime timeoutToken { get; set; }
    }

}
