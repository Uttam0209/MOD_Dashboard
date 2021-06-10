using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class WReports
    {
        public long Pkey { get; set; }
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public Nullable<System.DateTime> Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }
        public string IC_percentage { get; set; }
        public string Trials_Required { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> completed_on { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public string dap_timeline { get; set; }
        public decimal TaskSlno { get; set; }
    }

    public class WReports1
    {
        public long Pkey { get; set; }
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }
        public string IC_percentage { get; set; }
        public string Trials_Required { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> completed_on { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public string dap_timeline { get; set; }
        public decimal TaskSlno { get; set; }
    }
    public class StageWiseDelayReport
    {
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Categorisation { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public Nullable<decimal> stage { get; set; }
        public string stage_name { get; set; }
        public int aon_id { get; set; }
    }
    public class StageWiseDelayReport_N
    {
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Categorisation { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public Nullable<decimal> stage { get; set; }
        public string stage_name { get; set; }
        public string aon_id { get; set; }
    }
}