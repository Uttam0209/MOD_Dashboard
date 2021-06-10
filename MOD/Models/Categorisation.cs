using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class Categorisation
    {
        public String Text { get; set; }
        public String Value { get; set; }
    }

    public class CatWiseList
    {
        public String stage { get; set; }
        public String TaskDescription { get; set; }
        public Int32  PSU { get; set; }
        public Int32 Foreign_OEM { get; set; }
        public  Int32 Indian_Private_MSME { get; set; }
        public Int32 Indian_Private__Non_MSME { get; set; }
        public string Other { get; set; }
    }
    public class CatWiseListUnion
    {
        public String stage { get; set; }
        public String TaskDescription { get; set; }
        public Int32 PSU { get; set; }
        public Int32 Foreign_OEM { get; set; }
        public Int32 Indian_Private_MSME { get; set; }
        public Int32 Indian_Private__Non_MSME { get; set; }
        public Int32 Other { get; set; }
        public Int32 Undecided { get; set; }
    }

    public class CatWiseDetailList
    {
        public int aon_id { get; set; }
        public string Aon_description { get; set; }
        public string Description { get; set; }
        public string Categorisation { get; set; }
        public string Service_Lead_Service { get; set; }
        public Int32 total_aon_cost { get; set; }
        public Int32 stage { get; set; }
        public string vendorcategory { get; set; }
    }
}