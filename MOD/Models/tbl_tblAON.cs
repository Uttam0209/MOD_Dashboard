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
    
    public partial class tbl_tblAON
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbl_tblAON()
        {
            this.tbl_tblTimelineForProcurement = new HashSet<tbl_tblTimelineForProcurement>();
        }
    
        public int Id { get; set; }
        public Nullable<System.DateTime> dateofAon { get; set; }
        public string categorization { get; set; }
        public string itemname { get; set; }
        public string quantity { get; set; }
        public Nullable<int> cost { get; set; }
        public string leadservice { get; set; }
        public string naturetype { get; set; }
        public string draftRFPbySHQ { get; set; }
        public string collegiatevetting { get; set; }
        public string approval { get; set; }
        public string issuancevendor { get; set; }
        public string bidsubmissiondate { get; set; }
        public Nullable<System.DateTime> dateofsubmission { get; set; }
        public Nullable<System.DateTime> extendedDate { get; set; }
        public string vendors { get; set; }
        public Nullable<System.DateTime> TECconstitutiondate { get; set; }
        public string TECResult { get; set; }
        public Nullable<System.DateTime> TECReportApproval { get; set; }
        public Nullable<System.DateTime> commencementdate { get; set; }
        public Nullable<System.DateTime> completiondate { get; set; }
        public string participatingvendors { get; set; }
        public Nullable<System.DateTime> Reportrecievedate { get; set; }
        public Nullable<System.DateTime> reportacceptedDate { get; set; }
        public string qualifiedvendors { get; set; }
        public Nullable<System.DateTime> TOCConstitutiondate { get; set; }
        public Nullable<System.DateTime> TOCReportDate { get; set; }
        public Nullable<System.DateTime> TOCAcceptanceDate { get; set; }
        public Nullable<System.DateTime> CNCConstitutiondate { get; set; }
        public Nullable<System.DateTime> CNCBenchmarkdate { get; set; }
        public Nullable<System.DateTime> CNCBidopeningdate { get; set; }
        public Nullable<System.DateTime> CNCConclusionDate { get; set; }
        public Nullable<System.DateTime> CNCReportdate { get; set; }
        public Nullable<System.DateTime> CFA_MOD_Approval { get; set; }
        public Nullable<System.DateTime> CFA_MOD_Concurrence { get; set; }
        public string UniqueId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbl_tblTimelineForProcurement> tbl_tblTimelineForProcurement { get; set; }
    }
}
