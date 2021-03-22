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
    
    public partial class tbl_mst_Template
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_mst_Template()
        {
            this.acq_policy = new HashSet<acq_policy>();
        }
    
        public long TemplateID { get; set; }
        public Nullable<decimal> TaskSlno { get; set; }
        public string TaskDescription { get; set; }
        public string ShortName { get; set; }
        public string Duration { get; set; }
        public string Unit_of_time { get; set; }
        public string Dependencies { get; set; }
        public string super_task_id { get; set; }
        public Nullable<long> Responsibility { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> RecInsTime { get; set; }
        public string color { get; set; }
        public string dap_timeline { get; set; }
        public string Categorisation { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<acq_policy> acq_policy { get; set; }
    }
}
