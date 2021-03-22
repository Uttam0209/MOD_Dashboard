using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class MODListViewModel
    {

        public List<MODListViewModel> AonList { get; set; }
        public List<acq_project_master> AonFilterList { get; set; }
        public List<acq_meeting_master> MeetingMaster { get; set; }
        public int aon_id { get; set; }
        public DateTime? MeetingDate { get; set; }
        public string item_description { get; set; }
        public decimal TaskSlno { get; set; }
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
        public string pending_in_stage { get; set; }
        public DateTime? date_of_entering_this_stage { get; set; }
        public DateTime? AoNClosureDate { get; set; }
        public string System_case { get; set; }


        // Acq_rfpMaster

        public string type_rfp { get; set; }
        public string draft_rfp_date { get; set; }
        public string date_submissionto_adgacq { get; set; }
        public string date_acceptanceby_adgacq { get; set; }
        public string date_collegiate_vetting { get; set; }
        public string date_approval_rfp { get; set; }
        public string date_issue_rfp { get; set; }
        public string date_prebid_meeting { get; set; }
        public string date_prebid_replies { get; set; }
        public string lastdate_bidsubmission { get; set; }
        public string date_bid_opening { get; set; }
        public string ExtendedLastDate_BidSubmission { get; set; }
        public string date_tec_constitution { get; set; }
        public string date_tec_commencement { get; set; }
        public string date_tec_completion { get; set; }
        public string date_tec_reportsubmission { get; set; }
        public string date_tec_approval { get; set; }
        public string date_submission_eut { get; set; }
        public string date_issue_trials_directive { get; set; }
        public string Date_received_Fet { get; set; }
        public string Date_accepted_Fet { get; set; }
        public string Date_toc_Constitution { get; set; }
        public string Date_toc_report { get; set; }
        public string Date_toc_acceptance_CompetantAuth { get; set; }
        public string Date_cnc_constitution { get; set; }
        public string Date_cnc_benchmarking { get; set; }
        public string Date_commercial_bid_opening { get; set; }
        public string Date_cnc_conclusion { get; set; }
        public string Date_cnc_report { get; set; }
        public string date_Submission_CFANote_DGacq { get; set; }
        public string date_Acq_approvalforsending_to_MoDFin { get; set; }
        public string date_submissionto_CFA { get; set; }
        public string date_Approval_CFAnote { get; set; }
        public string date_DCN_Sentfor_InterMinisterialConsultations { get; set; }
        public string date_MoF_Concurrence { get; set; }
        public string date_DCN_submissionto_CCS { get; set; }
        public string ApproveToCSS_CFA { get; set; }
        public string DateContractSigning { get; set; }
        public string date_approval_CFA_CCS { get; set; }
        public string date_ContractSigning { get; set; }
        public virtual acq_project_master acq_project_master { get; set; }
    }

    public class MODSaveViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter AoNDate")]
        public DateTime? dateofAon { get; set; }
        [Required(ErrorMessage = "Select Categorization")]
        public string categorization { get; set; }
        [Required(ErrorMessage = "Enter Item Name")]
        public string itemname { get; set; }
        [Required(ErrorMessage = "Enter Quantity")]
        public string quantity { get; set; }
        [Required(ErrorMessage = "Enter Cost")]
        public int? cost { get; set; }
        [Required(ErrorMessage = "Select Services")]
        public string leadservice { get; set; }
        [Required(ErrorMessage = "Select Nature")]
        public string naturetype { get; set; }
        [Required(ErrorMessage = "Select Meeting Type")]
        public string dac_dpb { get; set; }
        public string draftRFPbySHQ { get; set; }
        public string collegiatevetting { get; set; }
        public string approval { get; set; }
        public string issuancevendor { get; set; }
        public string bidsubmissiondate { get; set; }
        public DateTime? dateofsubmission { get; set; }
        public DateTime? extendedDate { get; set; }
        public string vendors { get; set; }
        public DateTime? TECconstitutiondate { get; set; }
        public string TECResult { get; set; }
        public DateTime? TECReportApproval { get; set; }
        public DateTime? commencementdate { get; set; }
        public DateTime? completiondate { get; set; }
        public string participatingvendors { get; set; }
        public DateTime? Reportrecievedate { get; set; }
        public DateTime? reportacceptedDate { get; set; }
        public string qualifiedvendors { get; set; }
        public DateTime? TOCConstitutiondate { get; set; }
        public DateTime? TOCReportDate { get; set; }
        public DateTime? TOCAcceptanceDate { get; set; }
        public DateTime? CNCConstitutiondate { get; set; }
        public DateTime? CNCBenchmarkdate { get; set; }
        public DateTime? CNCBidopeningdate { get; set; }
        public DateTime? CNCConclusionDate { get; set; }
        public DateTime? CNCReportdate { get; set; }
        public DateTime? CFA_MOD_Approval { get; set; }
        public DateTime? CFA_MOD_Concurrence { get; set; }
        public string UniqueId { get; set; }
        public List<tbl_tblAON> AonList { get; set; }


    }

    public class SaveAcqProjectMasterViewModel
    {
        public int aon_id { get; set; }

        [Required(ErrorMessage = "Enter Item Description")]
        public string item_description { get; set; }

        public string DPP_DAP { get; set; }
        public string AoN_Accorded_By { get; set; }
        [Required(ErrorMessage = "Enter AoN Date")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]
        public DateTime? Date_of_Accord_of_AoN { get; set; }
        [Required(ErrorMessage = "Select Meeting Date")]
        public int? meeting_id { get; set; }
        public string Categorisation { get; set; }
        public string Service_Lead_Service { get; set; }
        public string Quantity { get; set; }
        public string Cost { get; set; }
        public string Type_of_Acquisition { get; set; }
        public string Trials_Required { get; set; }
        public string Essential_parameters { get; set; }
        public string Any_other_aspect { get; set; }
        public string IC_percentage { get; set; }
        public string Option_clause_applicable { get; set; }
        public string Offset_applicable { get; set; }
        public string AMC_applicable { get; set; }
        public string AoN_validity { get; set; }
        public string AoN_validity_unit { get; set; }
        public Nullable<decimal> AoN_revalidation_meeting_id { get; set; }
        public Nullable<System.DateTime> AoN_validity_extended_till { get; set; }
        public Nullable<decimal> AoN_Foreclosure_meeting_id { get; set; }
        public string Remarks { get; set; }
        public List<acq_meeting_master> MeetingMaster { get; set; }
        public string Tax_Duties { get; set; }
        public string AMCRemarks { get; set; }
        public string Warrenty_applicable { get; set; }
        public string Warrenty_Remarks { get; set; }
        public string Currency { get; set; }
        public DateTime? AoNClosureDate { get; set; }
        public string AoNClosureRemarks { get; set; }
        public Nullable<int> AoNForeClosureCreatedBy { get; set; }
        public List<SaveAcqProjectMasterViewModel> AoNList { get; set; }
        public string System_case { get; set; }
    }
}