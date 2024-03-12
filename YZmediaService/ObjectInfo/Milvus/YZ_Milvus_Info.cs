using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfo
{
    public class YZ_MilvusCollection_Info
    {
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }
        public decimal Post_Id { get; set; }
    }

    public class YZ_MilvusData_Info
    {
        public decimal File_Id { get; set; }
        public string File_Url { get; set; }
        public string CollectionName { get; set; }
        public float Distance { get; set; }
    }

    public class YZ_MilvusDataSearch_Info
    {
        public List<YZ_MilvusData_Info> dataSearch { get; set; }

        public int length { get; set; }
    }
}
