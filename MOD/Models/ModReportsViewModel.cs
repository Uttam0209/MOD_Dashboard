using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class ModReportsListViewModel
    {
        public List<MODListViewModel> AonList { get; set; }
        public List<acq_project_master> AonFilterList { get; set; }
        public List<acq_meeting_master> MeetingMaster { get; set; }
        public int aon_id { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string item_description { get; set; }
        public string Quantity { get; set; }
        public string Service_Lead_Service { get; set; }
        public string Categorisation { get; set; }
        public DateTime? Date_of_Accord_of_AoN { get; set; }
        public string DPP_DAP { get; set; }
        public string Cost { get; set; }
        public string Currency { get; set; }
        public string Type_of_Acquisition { get; set; }
        public string AoNaccordedbyDACDPB { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Tax_Duties { get; set; }
        public string Trials_Required { get; set; }
        public string Essential_parameters { get; set; }
        public string Any_other_aspect { get; set; }
        public string IC_percentage { get; set; }
        public string Option_clause_applicable { get; set; }
        public string Offset_applicable { get; set; }
        public string AMC_applicable { get; set; }
        public string AMCRemarks { get; set; }
        public string Warrenty_applicable { get; set; }
        public string Warrenty_Remarks { get; set; }
        public string AoN_validity { get; set; }
        public string AoN_validity_unit { get; set; }
        public string AoN_revalidation_meeting_id { get; set; }
        public string AoN_validity_extended_till { get; set; }
        public string AoN_Foreclosure_meeting_id { get; set; }
        public string Remarks { get; set; }

        //
    }
}