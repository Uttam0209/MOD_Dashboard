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
    
    public partial class tbl_trn_tryProject
    {
        public Nullable<int> TryPojectID { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Template_type { get; set; }
        public Nullable<int> TaskSlno { get; set; }
        public string TaskDescription { get; set; }
        public string ShortName { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Dependencies { get; set; }
        public int super_task_id { get; set; }
        public string ResponsibilityDepart { get; set; }
        public string Responsibility { get; set; }
        public string IsActive { get; set; }
        public int Progress { get; set; }
        public Nullable<System.DateTime> planned_start_date { get; set; }
        public Nullable<System.DateTime> planned_end_date { get; set; }
    }
}
