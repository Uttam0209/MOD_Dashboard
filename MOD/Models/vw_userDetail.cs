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
    
    public partial class vw_userDetail
    {
        public int deptt_id { get; set; }
        public string deptt_description { get; set; }
        public Nullable<int> section_id { get; set; }
        public string section_descr { get; set; }
        public Nullable<int> UserId { get; set; }
        public string UserName { get; set; }
        public string InternalEmailID { get; set; }
        public string ExternalEmailID { get; set; }
        public string Password { get; set; }
        public string Pswd_Salt { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Flag { get; set; }
        public Nullable<int> SrNo { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string IsActive { get; set; }
        public string IPAddress { get; set; }
        public string MacAddress { get; set; }
        public string DSCSerialno { get; set; }
        public string Temp_Number { get; set; }
    }
}
