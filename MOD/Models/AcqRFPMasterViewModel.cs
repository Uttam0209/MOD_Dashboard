using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOD.Models;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class SaveAcqRFPMasterViewModel
    {
        public int rfp_id { get; set; }

        
        public int aon_id { get; set; }
        public string item_description { get; set; }
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
        public string date_tec_constitution { get; set; }
        public string date_tec_commencement { get; set; }
        public string date_tec_completion { get; set; }
        public string date_tec_reportsubmission { get; set; }
        public string date_tec_approval { get; set; }
        //public string date_submission_eut { get; set; }
        //public string date_issue_trials_directive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public int vendorID { get; set; }
        public string VendorName { get; set; }
        public string SelectedVendorIds { get; set; }
        public string[] SelectedIDArray { get; set; }
        public int[] SelectedVendorsID { get; set; }
        public int[] VendorSelectedArray { get; set; }
        public int VendorRfpId { get; set; }
        public string ExtendedLastDate_BidSubmission { get; set; }
        public string Categorisation { get; set; }

        // TOC 
        public string Date_toc_Constitution { get; set; }
        public string Date_toc_report { get; set; }
        public string Date_toc_acceptance_CompetantAuth { get; set; }

        public IEnumerable<SaveAcqRFPMasterViewModel> VendorList { get; set; }
        public List<SaveAcqRFPMasterViewModel> ProjectList { get; set; }
        public virtual acq_project_master acq_project_master { get; set; }
        public List<acq_rfp_master> RFPMasterList { get; set; }
        public List<acq_rfp_vendors> AcqVendorMaster { get; set; }
        public string[] VendorMasterArray { get; set; }

        // acq_rfp_VendorsDetails Data
        public int ID { get; set; }
        public int rfp_ids { get; set; }
        public int vendor_id { get; set; }
        public Nullable<byte> type_ID { get; set; }

        //Fet Dates
        public string Date_received_Fet { get; set; }
        public string Date_accepted_Fet { get; set; }

        // CNC Dates
        public string Date_cnc_constitution { get; set; }
        public string Date_cnc_benchmarking { get; set; }
        public string Date_commercial_bid_opening { get; set; }
        public string Date_cnc_conclusion { get; set; }
        public string Date_cnc_report { get; set; }

        //CFA
        public string date_Submission_CFANote_DGacq { get; set; }
        public string date_Acq_approvalforsending_to_MoDFin { get; set; }
        public string date_submissionto_CFA { get; set; }
        public string date_Approval_CFAnote { get; set; }

        //DCN
        public string date_DCN_Sentfor_InterMinisterialConsultations { get; set; }
        public string date_MoF_Concurrence { get; set; }
        public string date_DCN_submissionto_CCS { get; set; }
        public string ApproveToCSS_CFA { get; set; }
        public string DateContractSigning { get; set; }


        //Contract
        public string date_approval_CFA_CCS { get; set; }
        public string date_ContractSigning { get; set; }


        // Make-1
        public string Eoi_DPRDateIssueMakeI { get; set; }
        public string Eoi_DPRReceiptDateMakeI { get; set; }
        public string Eoi_DPRDateCompletionandResponseMakeI { get; set; }
        public string dtforwardingshortlistedDPMakeI { get; set; }
        public string dateapprovalshortlistedvendorsMakeI { get; set; }

        public string dateselectionDasbySHQ_dprbMakeI { get; set; }
        public string dateapprovalCFAfundingarrangementissueProjectMakeI { get; set; }

        public string dateselectionDasbySHQ_dprbMakeII { get; set; }
        public string dateapprovalCFAfundingarrangementissueProjectMakeII { get; set; }

        public string datecommencementpreliminaryMakeI { get; set; }
        public string datefinalisationdetaileddesign { get; set; }
        public string datecommencementfabrication_devProMakeI { get; set; }
        public string datetechnicallimitedfieldstrialsMakeI { get; set; }
        public string datefreezingPSQRStoSQRSMakeI { get; set; }
        public string dateintimationvendorssubmissionrevisedMakeI { get; set; }
        public string datesubmissionrevisedcommercialoffersvendorsMakeI { get; set; }

        // Make-II
        public string DateIssueMake_II { get; set; }
        public string EoiReceiptDateMake_II { get; set; }
        public string EoiCompletionResponseMake_II { get; set; }
        public string dateApprovalEoiMake_II { get; set; }
        public string dateissueProjectSenctionMake_II { get; set; }

        public string datecommencementDevPrototypeMakeII { get; set; }
        public string datecompletionDevPrototypeMakeII { get; set; }
        public string datecommencementUserTrialsMakeII { get; set; }
        public string datecompletionUserTrialsRedinessReviewMakeII { get; set; }
        public string dateFreezingPSQRsMakeII { get; set; }

        //DD
        public string dateFormulationJPMT_DD { get; set; }
        public string dateFormulationPMT_DD { get; set; }
        public string dateIdentificationDCPP_DD { get; set; }
        public string datedetaildesignbySHQ_DD { get; set; }
        public string dateissuetrialdeveiation_DD { get; set; }
        public string dateprojectReview_DD { get; set; }
        public string daterealisationPrototype_DD { get; set; }
        public string dateCommencementPSQR { get; set; }
        public string dateCulminationPSQRValid_DD { get; set; }


        public List<SaveAcqRFPMasterViewModel> BidVendorList { get; set; }
        public List<SaveAcqRFPMasterViewModel> TcpVendorList { get; set; }
        public List<SaveAcqRFPMasterViewModel> FetVendorList { get; set; }
        public List<SaveAcqRFPMasterViewModel> ContractVendorList { get; set; }
        public List<acq_trials> GetTrials { get; set; }

        public List<SaveAcqRFPMasterViewModel> SelectedVendorsType1 { get; set; }
        public List<SaveAcqRFPMasterViewModel> SelectedBidType1 { get; set; }
        public List<SaveAcqRFPMasterViewModel> SelectedTcpType1 { get; set; }
        public List<SaveAcqRFPMasterViewModel> SelectedFetType1 { get; set; }
        public List<SaveAcqRFPMasterViewModel> VendorMaster { get; set; }
    }
}
