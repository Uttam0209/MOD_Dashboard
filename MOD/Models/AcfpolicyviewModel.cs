using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class AcfpolicyviewModel
    {
        public long policyid { get; set; }
        public long stagid { get; set; }
        public Nullable<System.DateTime> fdate { get; set; }
        public Nullable<System.DateTime> tdate { get; set; }
        public string Remarks { get; set; }
        public decimal TaskSlno { get; set; }
        public string pdfattachment { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public string Categorisation { get; set; }
        public string item_description { get; set; }
        public int aon_id { get; set; }
        public List<AcfpolicyviewModel> ProjectList { get; set; }
        public List<MODListViewModel> StageList { get; set; }
    }
}