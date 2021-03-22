using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOD.Models;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class AcquisitionMeetingMasterListViewModel
    {
        public List<acq_meeting_master> Meeting_MasterList { get; set; }
    }
    public class AcquisitionCreateMasterViewModel
    {

        public int meeting_id { get; set; }
        [Required (ErrorMessage ="Select Meeting Type")]
        public string dac_dpb { get; set; }
        [Required (ErrorMessage ="Select Meeting Date")]

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? meeting_date { get; set; }
        public DateTime? Date_of_Issue_of_Minutes { get; set; }
        public bool? Deleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string Remarks { get; set; }
    }
}