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
    
    public partial class acq_trials
    {
        public int id { get; set; }
        public int aonID { get; set; }
        public string trial_type { get; set; }
        public string date_extension_submission_eut { get; set; }
        public Nullable<System.DateTime> date_commencement { get; set; }
        public Nullable<System.DateTime> date_completion { get; set; }
        public string remarks { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
