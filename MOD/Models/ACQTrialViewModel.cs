using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class ACQTrialSaveViewModel
    {
        public int id { get; set; }
        public int aonID { get; set; }
        public string trial_type { get; set; }
        public string date_extension_submission_eut { get; set; }
        public DateTime? date_commencement { get; set; }
        public DateTime? date_completion { get; set; }
        public string remarks { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
    }
}