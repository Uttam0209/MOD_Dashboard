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
    
    public partial class tbl_tblTimelineForProcurement
    {
        public int ID { get; set; }
        public Nullable<int> AoNID { get; set; }
        public string Stages { get; set; }
        public string TimePlanned { get; set; }
        public string UnitTime { get; set; }
        public Nullable<System.DateTime> PlannedStartDate { get; set; }
        public Nullable<System.DateTime> plannedEndDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        public virtual tbl_tblAON tbl_tblAON { get; set; }
    }
}
