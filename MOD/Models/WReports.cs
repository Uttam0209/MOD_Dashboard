using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class WReports
    {
        public long Pkey { get; set; }
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public Nullable<System.DateTime> Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }
        public string IC_percentage { get; set; }
        public string Trials_Required { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> completed_on { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public string dap_timeline { get; set; }
        public decimal TaskSlno { get; set; }
    }

    public class WReports1
    {
        public long Pkey { get; set; }
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }
        public string IC_percentage { get; set; }
        public string Trials_Required { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> completed_on { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public string dap_timeline { get; set; }
        public decimal TaskSlno { get; set; }
    }
    public class StageWiseDelayReport
    {
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Categorisation { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public Nullable<decimal> stage { get; set; }
        public string stage_name { get; set; }
        public int aon_id { get; set; }
    }
    public class StageWiseDelayReport_N
    {
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        public string Categorisation { get; set; }
        public Nullable<int> no_of_weeks { get; set; }
        public Nullable<decimal> stage { get; set; }
        public string stage_name { get; set; }
        public string aon_id { get; set; }
    }


    public class StageWisePendingReport_N
    {
        public string Service_Lead_Service { get; set; }
        public string item_description { get; set; }
        
        public string Date_of_Accord_of_AoN { get; set; }
        public string Cost { get; set; }
        public string Categorisation { get; set; }

        public string IC_percentage { get; set; }

        public string Trials_Required { get; set; }
        public decimal TaskSlno { get; set; }

        public string System_case { get; set; }
        public string pending_in_stage { get; set; }
        public DateTime date_of_entering_this_stage { get; set; }

        public string Pkey { get; set; }
    }


    public class PeriodWiseCategoryReport
    {
        public string item_description { get; set; }
        public string DPP_DAP { get; set; }

        public string AoN_Accorded_By { get; set; }
        public string Date_of_Accord_of_AoN { get; set; }
        public string meeting_id { get; set; }

        public string Categorisation { get; set; }

        public string Service_Lead_Service { get; set; }
        public string Quantity { get; set; }

        public string Cost { get; set; }
        public string Currency { get; set; }
        public string Tax_Duties { get; set; }

        public string Type_of_Acquisition { get; set; }



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
        public string AoNClosureDate { get; set; }

        public string AoNClosureRemarks { get; set; }

        public string AoNForeClosureCreatedBy { get; set; }
        public string CreatedBy { get; set; }

        public string CreatedOn { get; set; }
        public string DeletedBy { get; set; }
        public string DeletedOn { get; set; }

        public string IsDeleted { get; set; }


        public string System_case { get; set; }
        public string VendorsIDs { get; set; }

        public string DirectorateId { get; set; }
        public string ResponsiblePersonLeve1 { get; set; }
        public string ResponsiblePersonLeve2 { get; set; }

        public string ResponsiblePersonLeve3 { get; set; }

        public string ResponsiblePersonLeve4 { get; set; }
        public string AovType { get; set; }

        public string ref_socId { get; set; }
        public string AoNNumber { get; set; }
       

        public string aon_id { get; set; }
    }
}