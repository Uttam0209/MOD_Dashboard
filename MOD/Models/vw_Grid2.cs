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
    
    public partial class vw_Grid2
    {
        public long IdPrimaryKey { get; set; }
        public Nullable<decimal> stage { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<int> army_projects { get; set; }
        public Nullable<int> iaf_projects { get; set; }
        public Nullable<int> navy_projects { get; set; }
        public Nullable<int> other_projects { get; set; }
    }
}
