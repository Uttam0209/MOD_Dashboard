//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MOD.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class acq_project_status_timelines
    {
        public int aon_id { get; set; }
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public Nullable<System.DateTime> Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }
        public string IC_percentage { get; set; }
        public string Trials_Required { get; set; }
        public Nullable<decimal> TaskSlno { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> completed_on { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public string dap_timeline { get; set; }
    }
}
