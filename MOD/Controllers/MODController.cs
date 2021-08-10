using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDPAdmin.Services.Master;
using Ganss.XSS;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using MOD.Service;
using Newtonsoft.Json;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
   // [Authorize]
    [SessionExpire]
    [SessionExpireRefNo]
    public class MODController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        MODEntities entities = new MODEntities();
        masterService mService = new masterService();
        string password = "p@SSword";
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        public MODController()
        {
            if (System.Web.HttpContext.Current.Session["EmailID"] != null)
            {
                AccountController account = new AccountController();
                string message = account.Blockuser(System.Web.HttpContext.Current.Session["EmailID"].ToString());
                if (message == "Blocked")
                {
                    System.Web.HttpContext.Current.Response.Redirect("~/LoginBlockMsg");
                }
            }
            BruteForce bruteForce = new BruteForce();
            BruteForceAttackss.bcontroller = "MOD";
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "MOD")
                {
                    if (BruteForceAttackss.refreshcount == 0 && BruteForceAttackss.date == null)
                    {
                        BruteForceAttackss.date = System.DateTime.Now;
                        BruteForceAttackss.refreshcount = 1;
                    }
                    else
                    {
                        TimeSpan tt = System.DateTime.Now - BruteForceAttackss.date.Value;
                        if (tt.TotalSeconds <= 30 && BruteForceAttackss.refreshcount > 20)
                        {
                            if (System.Web.HttpContext.Current.Session["EmailID"] != null)
                            {
                                List<UserViewModel> model = new List<UserViewModel>();
                                model = bruteForce.GetUserLoginBlock(System.Web.HttpContext.Current.Session["EmailID"].ToString());
                                if (model != null)
                                {
                                    BruteForceAttackss.refreshcount = 0;
                                    BruteForceAttackss.date = null;
                                    BruteForceAttackss.bcontroller = "";
                                    System.Web.HttpContext.Current.Response.Redirect(WebPortalUrl);
                                }
                            }

                        }
                        else
                        {
                            BruteForceAttackss.refreshcount = BruteForceAttackss.refreshcount + 1;
                        }
                    }


                }
            }
            else
            {
                BruteForceAttackss.bcontroller = "MOD";
            }
        }
        public static DataTable return_datatable(String query)
        {
            OleDbDataAdapter adap = new OleDbDataAdapter();
            OleDbCommand cmd = new OleDbCommand();
            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                cmd.CommandText = query;
                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }
            return dt;
        }
        public static int execute_query(String query)
        {

            OleDbDataAdapter adap = new OleDbDataAdapter();
            OleDbCommand cmd = new OleDbCommand();
            using (OleDbConnection conn = masterService.DB())
            {
                try
                {
                    cmd.CommandText = query;
                    cmd.Connection = conn; adap.SelectCommand = cmd;
                    cmd.ExecuteNonQuery();
                    return 1;
                }
                catch (Exception ex) { return 0; }
                finally { conn.Close(); }
            }
        }

        //[Route("MOD")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Stage Progress (Technical Manager)".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
                        {
                            isAccessible = true;
                        }
                    }
                }

                if (!isAccessible)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            List<SaveAcqRFPMasterViewModel> list = new List<SaveAcqRFPMasterViewModel>();
            SaveAcqRFPMasterViewModel model = new SaveAcqRFPMasterViewModel();
            var SectionID = Session["SectionID"];

            if ((int)SectionID == 2 || (int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 3 || (int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 4 || (int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG") || x.System_case == "Y").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }


            // Vendor List Bind

            List<SaveAcqRFPMasterViewModel> venList = new List<SaveAcqRFPMasterViewModel>();
            var vendorList = entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
            if (vendorList != null)
            {
                foreach (var item in vendorList)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.VendorId;
                    ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                    venList.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> BidVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var bidVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 1).ToList();
            if (bidVendor != null)
            {
                foreach (var item in bidVendor)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.vendor_id;
                    ObjVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    BidVendorListData.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> TcpVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var tcpVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2).ToList();
            if (tcpVendor != null)
            {
                foreach (var item in tcpVendor)
                {
                    SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                    ObjTCPVendor.vendorID = item.vendor_id;
                    ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    TcpVendorListData.Add(ObjTCPVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> FetVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var FetVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3).ToList();
            if (FetVendor != null)
            {
                foreach (var item in FetVendor)
                {
                    SaveAcqRFPMasterViewModel ObjFETVendor = new SaveAcqRFPMasterViewModel();
                    ObjFETVendor.vendorID = item.vendor_id;
                    ObjFETVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    FetVendorListData.Add(ObjFETVendor);
                }
            }

            // Get All Trials
            var struid = Convert.ToInt32(Session["UserID"].ToString());
            model.GetTrials = entities.acq_trials.Where(x => x.IsDeleted == false && x.CreatedBy == struid).ToList();


            model.FetVendorList = FetVendorListData;
            model.TcpVendorList = TcpVendorListData;
            model.ProjectList = list;
            model.VendorList = venList;
            model.BidVendorList = BidVendorListData;
            return View(model);
        }

       // [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Index(SaveAcqRFPMasterViewModel model)
        {
            List<SaveAcqRFPMasterViewModel> list = new List<SaveAcqRFPMasterViewModel>();
            SaveAcqRFPMasterViewModel model1 = new SaveAcqRFPMasterViewModel();
            //var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false).OrderBy(x => x.item_description).ToList();
            //if (projectList != null)
            //{
            //    foreach (var item in projectList)
            //    {
            //        SaveAcqRFPMasterViewModel obj1 = new SaveAcqRFPMasterViewModel();
            //        obj1.aon_id = item.aon_id;
            //        obj1.item_description = Cipher.Decrypt(item.item_description, password);
            //        list.Add(obj1);
            //    }
            //}

            // Get All Trials
            var struid = Convert.ToInt32(Session["UserID"].ToString());

            model.GetTrials = entities.acq_trials.Where(x => x.IsDeleted == false && x.CreatedBy == struid && x.aonID == model.aon_id).ToList();

            var SectionID = Session["SectionID"];
          

            if ((int)SectionID == 2 || (int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 3 || (int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 4 || (int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG") || x.System_case == "Y").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }

            // Vendor DropDown List 

            List<SaveAcqRFPMasterViewModel> venList = new List<SaveAcqRFPMasterViewModel>();
            var vendorList = entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
            if (vendorList != null)
            {
                foreach (var item in vendorList)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.VendorId;
                    ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                    venList.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> BidVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var bidVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 1).ToList();
            if (bidVendor != null)
            {
                foreach (var item in bidVendor)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.vendor_id;
                    ObjVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    BidVendorListData.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> TcpVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var tcpVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2).ToList();
            if (tcpVendor != null)
            {
                foreach (var item in tcpVendor)
                {
                    SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                    ObjTCPVendor.vendorID = item.vendor_id;
                    ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    TcpVendorListData.Add(ObjTCPVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> FetVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var FetVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3).ToList();
            if (tcpVendor != null)
            {
                foreach (var item in tcpVendor)
                {
                    SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                    ObjTCPVendor.vendorID = item.vendor_id;
                    ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    TcpVendorListData.Add(ObjTCPVendor);
                }
            }
            model1.FetVendorList = FetVendorListData;
            model1.TcpVendorList = TcpVendorListData;
            model1.ProjectList = list;
            model1.VendorList = venList;
            model1.BidVendorList = BidVendorListData;

            if (model.rfp_id == 0)
            {
                acq_rfp_master obj = new acq_rfp_master();
                obj.type_rfp = sanitizer.Sanitize(model.type_rfp);
                obj.aon_id = model.aon_id;
                obj.draft_rfp_date = sanitizer.Sanitize(model.draft_rfp_date);
                obj.date_submissionto_adgacq = sanitizer.Sanitize(model.date_submissionto_adgacq);
                obj.date_acceptanceby_adgacq = sanitizer.Sanitize(model.date_acceptanceby_adgacq);
                obj.date_collegiate_vetting = sanitizer.Sanitize(model.date_collegiate_vetting);
                obj.date_approval_rfp = sanitizer.Sanitize(model.date_approval_rfp);
                obj.date_issue_rfp = sanitizer.Sanitize(model.date_issue_rfp);
                obj.date_prebid_meeting = sanitizer.Sanitize(model.date_prebid_meeting);
                obj.date_prebid_replies = sanitizer.Sanitize(model.date_prebid_replies);
                obj.lastdate_bidsubmission = sanitizer.Sanitize(model.lastdate_bidsubmission);
                obj.date_bid_opening = sanitizer.Sanitize(model.date_bid_opening);
                obj.ExtendedLastDate_BidSubmission = sanitizer.Sanitize(model.ExtendedLastDate_BidSubmission);
                obj.Date_cnc_constitution = sanitizer.Sanitize(model.Date_cnc_constitution);
                obj.Date_cnc_benchmarking = sanitizer.Sanitize(model.Date_cnc_benchmarking);
                obj.Date_commercial_bid_opening = sanitizer.Sanitize(model.Date_commercial_bid_opening);
                obj.Date_cnc_conclusion = sanitizer.Sanitize(model.Date_cnc_conclusion);
                obj.Date_cnc_report = sanitizer.Sanitize(model.Date_cnc_report);

                obj.date_Submission_CFANote_DGacq = sanitizer.Sanitize(model.date_Submission_CFANote_DGacq);
                obj.date_Acq_approvalforsending_to_MoDFin = sanitizer.Sanitize(model.date_Submission_CFANote_DGacq);
                obj.date_submissionto_CFA = sanitizer.Sanitize(model.date_submissionto_CFA);
                obj.date_Approval_CFAnote = sanitizer.Sanitize(model.date_Approval_CFAnote);

                obj.date_DCN_Sentfor_InterMinisterialConsultations = sanitizer.Sanitize(model.date_DCN_Sentfor_InterMinisterialConsultations);
                obj.date_MoF_Concurrence = sanitizer.Sanitize(model.date_MoF_Concurrence);
                obj.date_DCN_submissionto_CCS = sanitizer.Sanitize(model.date_DCN_submissionto_CCS);
                obj.ApproveToCSS_CFA = sanitizer.Sanitize(model.ApproveToCSS_CFA);
                obj.DateContractSigning = sanitizer.Sanitize(model.DateContractSigning);

                obj.date_approval_CFA_CCS = sanitizer.Sanitize(model.date_approval_CFA_CCS);
                obj.date_ContractSigning = sanitizer.Sanitize(model.date_ContractSigning);

                obj.Eoi_DPRDateIssueMakeI = sanitizer.Sanitize(model.Eoi_DPRDateIssueMakeI);
                obj.Eoi_DPRReceiptDateMakeI = sanitizer.Sanitize(model.Eoi_DPRReceiptDateMakeI);
                obj.Eoi_DPRDateCompletionandResponseMakeI = sanitizer.Sanitize(model.Eoi_DPRDateCompletionandResponseMakeI);
                obj.dtforwardingshortlistedDPMakeI = sanitizer.Sanitize(model.dtforwardingshortlistedDPMakeI);
                obj.dateapprovalshortlistedvendorsMakeI = sanitizer.Sanitize(model.dateapprovalshortlistedvendorsMakeI);

                obj.dateselectionDasbySHQ_dprbMakeI = sanitizer.Sanitize(model.dateselectionDasbySHQ_dprbMakeI);
                obj.dateapprovalCFAfundingarrangementissueProjectMakeI = sanitizer.Sanitize(model.dateapprovalCFAfundingarrangementissueProjectMakeI);

                obj.dateselectionDasbySHQ_dprbMakeII = sanitizer.Sanitize(model.dateselectionDasbySHQ_dprbMakeII);
                obj.dateapprovalCFAfundingarrangementissueProjectMakeII = sanitizer.Sanitize(model.dateapprovalCFAfundingarrangementissueProjectMakeII);

                obj.datecommencementpreliminaryMakeI = sanitizer.Sanitize(model.datecommencementpreliminaryMakeI);
                obj.datefinalisationdetaileddesign = sanitizer.Sanitize(model.datefinalisationdetaileddesign);
                obj.datecommencementfabrication_devProMakeI = sanitizer.Sanitize(model.datecommencementfabrication_devProMakeI);
                obj.datetechnicallimitedfieldstrialsMakeI = sanitizer.Sanitize(model.datetechnicallimitedfieldstrialsMakeI);
                obj.datefreezingPSQRStoSQRSMakeI = sanitizer.Sanitize(model.datefreezingPSQRStoSQRSMakeI);
                obj.dateintimationvendorssubmissionrevisedMakeI = sanitizer.Sanitize(model.dateintimationvendorssubmissionrevisedMakeI);
                obj.datesubmissionrevisedcommercialoffersvendorsMakeI = sanitizer.Sanitize(model.datesubmissionrevisedcommercialoffersvendorsMakeI);

                obj.DateIssueMake_II = sanitizer.Sanitize(model.DateIssueMake_II);
                obj.EoiReceiptDateMake_II = sanitizer.Sanitize(model.EoiReceiptDateMake_II);
                obj.EoiCompletionResponseMake_II = sanitizer.Sanitize(model.EoiCompletionResponseMake_II);
                obj.dateApprovalEoiMake_II = sanitizer.Sanitize(model.dateApprovalEoiMake_II);
                obj.dateissueProjectSenctionMake_II = sanitizer.Sanitize(model.dateissueProjectSenctionMake_II);

                obj.datecommencementDevPrototypeMakeII = sanitizer.Sanitize(model.datecommencementDevPrototypeMakeII);
                obj.datecompletionDevPrototypeMakeII = sanitizer.Sanitize(model.datecompletionDevPrototypeMakeII);
                obj.datecommencementUserTrialsMakeII = sanitizer.Sanitize(model.datecommencementUserTrialsMakeII);
                obj.datecompletionUserTrialsRedinessReviewMakeII = sanitizer.Sanitize(model.datecompletionUserTrialsRedinessReviewMakeII);
                obj.dateFreezingPSQRsMakeII = sanitizer.Sanitize(model.dateFreezingPSQRsMakeII);

                obj.dateFormulationJPMT_DD = sanitizer.Sanitize(model.dateFormulationJPMT_DD);
                obj.dateFormulationPMT_DD = sanitizer.Sanitize(model.dateFormulationPMT_DD);
                obj.dateIdentificationDCPP_DD = sanitizer.Sanitize(model.dateIdentificationDCPP_DD);
                obj.datedetaildesignbySHQ_DD = sanitizer.Sanitize(model.datedetaildesignbySHQ_DD);
                obj.dateissuetrialdeveiation_DD = sanitizer.Sanitize(model.dateissuetrialdeveiation_DD);
                obj.dateprojectReview_DD = sanitizer.Sanitize(model.dateprojectReview_DD);
                obj.daterealisationPrototype_DD = sanitizer.Sanitize(model.daterealisationPrototype_DD);
                obj.dateCommencementPSQR = sanitizer.Sanitize(model.dateCommencementPSQR);
                obj.dateCulminationPSQRValid_DD = sanitizer.Sanitize(model.dateCulminationPSQRValid_DD);

                obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                obj.CreatedOn = System.DateTime.Now;
                entities.acq_rfp_master.Add(obj);
                entities.SaveChanges();

                var LastInsertrPFId = obj.rfp_id;

                acq_rfp_vendors rfpVendor = new acq_rfp_vendors();
                rfpVendor.Rfp_fk_Id = LastInsertrPFId;
                entities.acq_rfp_vendors.Add(rfpVendor);
                entities.SaveChanges();

                var LastInsertId = rfpVendor.rfp_id;

                acq_rfp_VendorsDetails objdeta = new acq_rfp_VendorsDetails();
                if (model.SelectedIDArray != null)
                {
                    foreach (var item in model.SelectedIDArray)
                    {
                        objdeta.rfpID = LastInsertId;
                        objdeta.vendor_id = Convert.ToInt32(item);
                        objdeta.type_ID = model.type_ID;
                        entities.acq_rfp_VendorsDetails.Add(objdeta);
                        entities.SaveChanges();
                    }
                }
                ViewBag.Message = "Data Saved Successfully";
                //TempData["Message"] = "Data Saved Successfully..!!";
                //return RedirectToAction("Index");
                //return View();
            }

            else if (model.rfp_id > 0)
            {
                var _updateAon = entities.acq_rfp_master.Where(x => x.rfp_id == model.rfp_id).FirstOrDefault();
                if (_updateAon != null)
                {
                    _updateAon.type_rfp = sanitizer.Sanitize(model.type_rfp);
                    _updateAon.aon_id = model.aon_id;
                    _updateAon.draft_rfp_date = sanitizer.Sanitize(model.draft_rfp_date);
                    _updateAon.date_submissionto_adgacq = sanitizer.Sanitize(model.date_submissionto_adgacq);
                    _updateAon.date_acceptanceby_adgacq = sanitizer.Sanitize(model.date_acceptanceby_adgacq);
                    _updateAon.date_collegiate_vetting = sanitizer.Sanitize(model.date_collegiate_vetting);
                    _updateAon.date_approval_rfp = sanitizer.Sanitize(model.date_approval_rfp);
                    _updateAon.date_issue_rfp = sanitizer.Sanitize(model.date_issue_rfp);
                    _updateAon.date_prebid_meeting = sanitizer.Sanitize(model.date_prebid_meeting);
                    _updateAon.date_prebid_replies = sanitizer.Sanitize(model.date_prebid_replies);
                    _updateAon.lastdate_bidsubmission = sanitizer.Sanitize(model.lastdate_bidsubmission);
                    _updateAon.date_bid_opening = sanitizer.Sanitize(model.date_bid_opening);
                    _updateAon.ExtendedLastDate_BidSubmission = sanitizer.Sanitize(model.ExtendedLastDate_BidSubmission);
                    _updateAon.date_tec_constitution = sanitizer.Sanitize(model.date_tec_constitution);
                    _updateAon.date_tec_commencement = sanitizer.Sanitize(model.date_tec_commencement);
                    _updateAon.date_tec_completion = sanitizer.Sanitize(model.date_tec_completion);
                    _updateAon.date_tec_reportsubmission = sanitizer.Sanitize(model.date_tec_reportsubmission);
                    _updateAon.date_tec_approval = sanitizer.Sanitize(model.date_tec_approval);
                    _updateAon.Date_toc_Constitution = sanitizer.Sanitize(model.Date_toc_Constitution);
                    _updateAon.Date_toc_report = sanitizer.Sanitize(model.Date_toc_report);
                    _updateAon.Date_toc_acceptance_CompetantAuth = sanitizer.Sanitize(model.Date_toc_acceptance_CompetantAuth);
                    _updateAon.Date_received_Fet = sanitizer.Sanitize(model.Date_received_Fet);
                    _updateAon.Date_accepted_Fet = sanitizer.Sanitize(model.Date_accepted_Fet);
                    _updateAon.Date_cnc_constitution = sanitizer.Sanitize(model.Date_cnc_constitution);
                    _updateAon.Date_cnc_benchmarking = sanitizer.Sanitize(model.Date_cnc_benchmarking);
                    _updateAon.Date_commercial_bid_opening = sanitizer.Sanitize(model.Date_commercial_bid_opening);
                    _updateAon.Date_cnc_conclusion = sanitizer.Sanitize(model.Date_cnc_conclusion);
                    _updateAon.Date_cnc_report = sanitizer.Sanitize(model.Date_cnc_report);

                    _updateAon.date_Submission_CFANote_DGacq = sanitizer.Sanitize(model.date_Submission_CFANote_DGacq);
                    _updateAon.date_Acq_approvalforsending_to_MoDFin = sanitizer.Sanitize(model.date_Submission_CFANote_DGacq);
                    _updateAon.date_submissionto_CFA = sanitizer.Sanitize(model.date_submissionto_CFA);
                    _updateAon.date_Approval_CFAnote = sanitizer.Sanitize(model.date_Approval_CFAnote);

                    _updateAon.date_DCN_Sentfor_InterMinisterialConsultations = sanitizer.Sanitize(model.date_DCN_Sentfor_InterMinisterialConsultations);
                    _updateAon.date_MoF_Concurrence = sanitizer.Sanitize(model.date_MoF_Concurrence);
                    _updateAon.date_DCN_submissionto_CCS = sanitizer.Sanitize(model.date_DCN_submissionto_CCS);
                    _updateAon.ApproveToCSS_CFA = sanitizer.Sanitize(model.ApproveToCSS_CFA);
                    _updateAon.DateContractSigning = sanitizer.Sanitize(model.DateContractSigning);

                    _updateAon.date_approval_CFA_CCS = sanitizer.Sanitize(model.date_approval_CFA_CCS);
                    _updateAon.date_ContractSigning = sanitizer.Sanitize(model.date_ContractSigning);

                    _updateAon.Eoi_DPRDateIssueMakeI = sanitizer.Sanitize(model.Eoi_DPRDateIssueMakeI);
                    _updateAon.Eoi_DPRReceiptDateMakeI = sanitizer.Sanitize(model.Eoi_DPRReceiptDateMakeI);
                    _updateAon.Eoi_DPRDateCompletionandResponseMakeI = sanitizer.Sanitize(model.Eoi_DPRDateCompletionandResponseMakeI);
                    _updateAon.dtforwardingshortlistedDPMakeI = sanitizer.Sanitize(model.dtforwardingshortlistedDPMakeI);
                    _updateAon.dateapprovalshortlistedvendorsMakeI = sanitizer.Sanitize(model.dateapprovalshortlistedvendorsMakeI);

                    _updateAon.dateselectionDasbySHQ_dprbMakeI = sanitizer.Sanitize(model.dateselectionDasbySHQ_dprbMakeI);
                    _updateAon.dateapprovalCFAfundingarrangementissueProjectMakeI = sanitizer.Sanitize(model.dateapprovalCFAfundingarrangementissueProjectMakeI);

                    _updateAon.dateselectionDasbySHQ_dprbMakeII = sanitizer.Sanitize(model.dateselectionDasbySHQ_dprbMakeII);
                    _updateAon.dateapprovalCFAfundingarrangementissueProjectMakeII = sanitizer.Sanitize(model.dateapprovalCFAfundingarrangementissueProjectMakeII);

                    _updateAon.datecommencementpreliminaryMakeI = sanitizer.Sanitize(model.datecommencementpreliminaryMakeI);
                    _updateAon.datefinalisationdetaileddesign = sanitizer.Sanitize(model.datefinalisationdetaileddesign);
                    _updateAon.datecommencementfabrication_devProMakeI = sanitizer.Sanitize(model.datecommencementfabrication_devProMakeI);
                    _updateAon.datetechnicallimitedfieldstrialsMakeI = sanitizer.Sanitize(model.datetechnicallimitedfieldstrialsMakeI);
                    _updateAon.datefreezingPSQRStoSQRSMakeI = sanitizer.Sanitize(model.datefreezingPSQRStoSQRSMakeI);
                    _updateAon.dateintimationvendorssubmissionrevisedMakeI = sanitizer.Sanitize(model.dateintimationvendorssubmissionrevisedMakeI);
                    _updateAon.datesubmissionrevisedcommercialoffersvendorsMakeI = sanitizer.Sanitize(model.datesubmissionrevisedcommercialoffersvendorsMakeI);

                    _updateAon.DateIssueMake_II = sanitizer.Sanitize(model.DateIssueMake_II);
                    _updateAon.EoiReceiptDateMake_II = sanitizer.Sanitize(model.EoiReceiptDateMake_II);
                    _updateAon.EoiCompletionResponseMake_II = sanitizer.Sanitize(model.EoiCompletionResponseMake_II);
                    _updateAon.dateApprovalEoiMake_II = sanitizer.Sanitize(model.dateApprovalEoiMake_II);
                    _updateAon.dateissueProjectSenctionMake_II = sanitizer.Sanitize(model.dateissueProjectSenctionMake_II);

                    _updateAon.datecommencementDevPrototypeMakeII = sanitizer.Sanitize(model.datecommencementDevPrototypeMakeII);
                    _updateAon.datecompletionDevPrototypeMakeII = sanitizer.Sanitize(model.datecompletionDevPrototypeMakeII);
                    _updateAon.datecommencementUserTrialsMakeII = sanitizer.Sanitize(model.datecommencementUserTrialsMakeII);
                    _updateAon.datecompletionUserTrialsRedinessReviewMakeII = sanitizer.Sanitize(model.datecompletionUserTrialsRedinessReviewMakeII);
                    _updateAon.dateFreezingPSQRsMakeII = sanitizer.Sanitize(model.dateFreezingPSQRsMakeII);



                    _updateAon.dateFormulationJPMT_DD = sanitizer.Sanitize(model.dateFormulationJPMT_DD);
                    _updateAon.dateFormulationPMT_DD = sanitizer.Sanitize(model.dateFormulationPMT_DD);
                    _updateAon.dateIdentificationDCPP_DD = sanitizer.Sanitize(model.dateIdentificationDCPP_DD);
                    _updateAon.datedetaildesignbySHQ_DD = sanitizer.Sanitize(model.datedetaildesignbySHQ_DD);
                    _updateAon.dateissuetrialdeveiation_DD = sanitizer.Sanitize(model.dateissuetrialdeveiation_DD);
                    _updateAon.dateprojectReview_DD = sanitizer.Sanitize(model.dateprojectReview_DD);
                    _updateAon.daterealisationPrototype_DD = sanitizer.Sanitize(model.daterealisationPrototype_DD);
                    _updateAon.dateCommencementPSQR = sanitizer.Sanitize(model.dateCommencementPSQR);
                    _updateAon.dateCulminationPSQRValid_DD = sanitizer.Sanitize(model.dateCulminationPSQRValid_DD);

                    _updateAon.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAon.CreatedOn = System.DateTime.Now;
                    entities.SaveChanges();

                    var LastInsertrPFId = _updateAon.rfp_id;

                    var rfpID = entities.acq_rfp_vendors.Where(x => x.Rfp_fk_Id == LastInsertrPFId).FirstOrDefault();
                    var LastInsertId = 0;
                    if (rfpID!=null)
                    {
                         LastInsertId = rfpID.rfp_id;
                    }
                    else
                    {
                        LastInsertId = LastInsertrPFId;
                    }

                   

                    if (model.type_ID == 1)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 1 && x.rfpID == LastInsertId).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 2)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2 && x.rfpID == LastInsertId).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 3)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3 && x.rfpID == LastInsertId).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 4)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 4 && x.rfpID == LastInsertId).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    if (model.SelectedIDArray != null)
                    {
                        acq_rfp_VendorsDetails objdeta = new acq_rfp_VendorsDetails();
                        if (model.SelectedIDArray != null)
                        {
                            foreach (var item in model.SelectedIDArray)
                            {
                                objdeta.rfpID = LastInsertId;
                                objdeta.vendor_id = Convert.ToInt32(item);
                                objdeta.type_ID = model.type_ID;
                                entities.acq_rfp_VendorsDetails.Add(objdeta);
                                entities.SaveChanges();
                            }
                        }
                    }
                    ViewBag.Message = "Data Update Successfully";
                    //TempData["Message"] = "Data Update Successfully..!!";
                    //return RedirectToAction("Index");
                    //return View();
                }
            }
            return View(model1);
        }
        [Route("MODProjectstageapplicability")]
        public ActionResult projectstageapplicability()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Project Stage Applicability".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
                        {
                            isAccessible = true;
                        }
                    }
                }

                if (!isAccessible)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            // Project List Bind
            List<applicabilityViewModel> list = new List<applicabilityViewModel>();
            applicabilityViewModel model = new applicabilityViewModel();
            var SectionID = Session["SectionID"];

            if ((int)SectionID == 2 || (int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();
                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 3 || (int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();
                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 4 || (int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG") || x.System_case == "Y").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();
                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }

            var struid = "";
            if (Session["SectionID"] != null)
            {
                struid = mService.SectionID(Session["SectionID"].ToString());
            }
            List<MODListViewModel> list1 = new List<MODListViewModel>();
            string query = "";
            if (struid == "SuperAdmin")
            {
                struid = "%";
            }
            else
            {
               
            }

            query = "SELECT DISTINCT TemplateID,TaskSlno, TaskDescription" +
" FROM dbo.tbl_mst_Template" +
" WHERE(TaskSlno NOT IN('1', '2')) order by TemplateID";
            DataTable dt = return_datatable(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    MODListViewModel obj2 = new MODListViewModel();

                    obj2.pending_in_stage = item["TaskDescription"].ToString();
                    obj2.TaskSlno = Convert.ToInt32(item["TaskSlno"]);


                    list1.Add(obj2);
                }
            }
            model.StageList = list1;
            model.ProjectList = list;
            return View(model);
        }
        [HttpPost]
        [Route("MODProjectstageapplicability")]
        public ActionResult projectstageapplicability(applicabilityViewModel model)
        {

            // Project List Bind
            List<applicabilityViewModel> list = new List<applicabilityViewModel>();
            applicabilityViewModel model1 = new applicabilityViewModel();
            var SectionID = Session["SectionID"];
            if ((int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce").OrderBy(x => x.item_description).ToList();

                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();
                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);

                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();

                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service.Contains("Navy")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();

                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service.Contains("ICG")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        applicabilityViewModel obj = new applicabilityViewModel();

                        obj.AoN = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }


            acq_projectstage_applicability obj1 = new acq_projectstage_applicability();
            obj1.AoN = model.AoN;
            obj1.applicable = model.applicable;
            obj1.Remarks = model.Remarks;
            obj1.stage = model.TaskSlno;


            entities.acq_projectstage_applicability.Add(obj1);
            entities.SaveChanges();


            ViewBag.Message = "Data Saved Successfully";


            model.ProjectList = list;

            var struid = "";
            if (Session["SectionID"] != null)
            {
                struid = mService.SectionID(Session["SectionID"].ToString());
            }
            List<MODListViewModel> list1 = new List<MODListViewModel>();
            string query = "";
            if (struid == "SuperAdmin")
            {
                struid = "%";
            }
            else
            {

            }
            query = "SELECT DISTINCT TaskSlno, pending_in_stage" +
" FROM dbo.acq_project_status_pendingstage" +
" WHERE(TaskSlno NOT IN('2', '3')) and Service_Lead_Service like '" + struid + "'";
           
            DataTable dt = return_datatable(query);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        MODListViewModel obj2 = new MODListViewModel();

                        obj2.pending_in_stage = item["pending_in_stage"].ToString();
                        obj2.TaskSlno = Convert.ToInt32(item["TaskSlno"]);


                    list1.Add(obj2);
                    }
                }


            model.StageList = list1;

            return View(model);
        }
        [Route("MODStageProgressAcquisitionManager")]
        public ActionResult StageProgressAcquisitionManager()
        {
            // Project List Bind
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Stage Progress (Acquisition Manager)".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 2 || Convert.ToInt32(Session["SectionID"]) == 3 || Convert.ToInt32(Session["SectionID"]) == 4)
                        {
                            isAccessible = true;
                        }
                    }
                }

                if (!isAccessible)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            List<SaveAcqRFPMasterViewModel> list = new List<SaveAcqRFPMasterViewModel>();
            SaveAcqRFPMasterViewModel model = new SaveAcqRFPMasterViewModel();
            var SectionID = Session["SectionID"];

            if ((int)SectionID == 2 || (int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 3 || (int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army" && (x.System_case == null || x.System_case=="N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 4 || (int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG") || x.System_case == "Y").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
           
            List<acq_rfp_VendorsDetails> ContractVendor = new List<acq_rfp_VendorsDetails>();
            List<SaveAcqRFPMasterViewModel> ContractListData = new List<SaveAcqRFPMasterViewModel>();

             ContractVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 4).ToList();
            if (ContractVendor.Count == 0)
            {
                ContractVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3).ToList();

                if(ContractVendor.Count == 0)
                {
                    ContractVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2).ToList();
                }
            }
           
            if (ContractVendor != null)
            {
                foreach (var item in ContractVendor)
                {
                    SaveAcqRFPMasterViewModel ObjContractVendor = new SaveAcqRFPMasterViewModel();
                    ObjContractVendor.vendorID = item.vendor_id;
                    ObjContractVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    ContractListData.Add(ObjContractVendor);
                }
            }

            model.ProjectList = list;
            model.ContractVendorList = ContractListData;
            return View(model);
        }

        [HttpPost]
        [Route("MODStageProgressAcquisitionManager")]
        public ActionResult StageProgressAcquisitionManager(SaveAcqRFPMasterViewModel model)
        {
            List<SaveAcqRFPMasterViewModel> list = new List<SaveAcqRFPMasterViewModel>();
            SaveAcqRFPMasterViewModel model1 = new SaveAcqRFPMasterViewModel();
            //var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false).OrderBy(x => x.item_description).ToList();
            //if (projectList != null)
            //{
            //    foreach (var item in projectList)
            //    {
            //        SaveAcqRFPMasterViewModel obj1 = new SaveAcqRFPMasterViewModel();
            //        obj1.aon_id = item.aon_id;
            //        obj1.item_description = Cipher.Decrypt(item.item_description, password);
            //        list.Add(obj1);
            //    }
            //}
            var SectionID = Session["SectionID"];

            if ((int)SectionID == 2 || (int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 3 || (int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army" && (x.System_case == null || x.System_case == "N")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 4 || (int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG") || x.System_case == "Y").OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        SaveAcqRFPMasterViewModel obj = new SaveAcqRFPMasterViewModel();
                        obj.aon_id = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        list.Add(obj);
                    }
                }
            }

            // Vendor DropDown List 

            List<SaveAcqRFPMasterViewModel> venList = new List<SaveAcqRFPMasterViewModel>();
            var vendorList = entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
            if (vendorList != null)
            {
                foreach (var item in vendorList)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.VendorId;
                    ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                    venList.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> BidVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var bidVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 1).ToList();
            if (bidVendor != null)
            {
                foreach (var item in bidVendor)
                {
                    SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                    ObjVendor.vendorID = item.vendor_id;
                    ObjVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    BidVendorListData.Add(ObjVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> TcpVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var tcpVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2).ToList();
            if (tcpVendor != null)
            {
                foreach (var item in tcpVendor)
                {
                    SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                    ObjTCPVendor.vendorID = item.vendor_id;
                    ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    TcpVendorListData.Add(ObjTCPVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> FetVendorListData = new List<SaveAcqRFPMasterViewModel>();
            var FetVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3).ToList();
            if (tcpVendor != null)
            {
                foreach (var item in tcpVendor)
                {
                    SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                    ObjTCPVendor.vendorID = item.vendor_id;
                    ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    FetVendorListData.Add(ObjTCPVendor);
                }
            }

            List<SaveAcqRFPMasterViewModel> ContractListData = new List<SaveAcqRFPMasterViewModel>();
            var ContractVendor = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 4).ToList();
            if (ContractVendor != null)
            {
                foreach (var item in ContractVendor)
                {
                    SaveAcqRFPMasterViewModel ObjContractVendor = new SaveAcqRFPMasterViewModel();
                    ObjContractVendor.vendorID = item.vendor_id;
                    ObjContractVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                    ContractListData.Add(ObjContractVendor);
                }
            }

            model1.FetVendorList = FetVendorListData;
            model1.TcpVendorList = TcpVendorListData;
            model1.ProjectList = list;
            model1.VendorList = venList;
            model1.BidVendorList = BidVendorListData;
            model1.ContractVendorList = ContractListData;

            if (model.rfp_id == 0)
            {
                acq_rfp_master obj = new acq_rfp_master();
                obj.type_rfp = model.type_rfp;
                obj.aon_id = model.aon_id;
                obj.draft_rfp_date = model.draft_rfp_date;
                obj.date_submissionto_adgacq = model.date_submissionto_adgacq;
                obj.date_acceptanceby_adgacq = model.date_acceptanceby_adgacq;
                obj.date_collegiate_vetting = model.date_collegiate_vetting;
                obj.date_approval_rfp = model.date_approval_rfp;
                obj.date_issue_rfp = model.date_issue_rfp;
                obj.date_prebid_meeting = model.date_prebid_meeting;
                obj.date_prebid_replies = model.date_prebid_replies;
                obj.lastdate_bidsubmission = model.lastdate_bidsubmission;
                obj.date_bid_opening = model.date_bid_opening;
                obj.ExtendedLastDate_BidSubmission = model.ExtendedLastDate_BidSubmission;
                obj.Date_cnc_constitution = model.Date_cnc_constitution;
                obj.Date_cnc_benchmarking = model.Date_cnc_benchmarking;
                obj.Date_commercial_bid_opening = model.Date_commercial_bid_opening;
                obj.Date_cnc_conclusion = model.Date_cnc_conclusion;
                obj.Date_cnc_report = model.Date_cnc_report;

                obj.date_Submission_CFANote_DGacq = model.date_Submission_CFANote_DGacq;
                obj.date_Acq_approvalforsending_to_MoDFin = model.date_Submission_CFANote_DGacq;
                obj.date_submissionto_CFA = model.date_submissionto_CFA;
                obj.date_Approval_CFAnote = model.date_Approval_CFAnote;

                obj.date_DCN_Sentfor_InterMinisterialConsultations = model.date_DCN_Sentfor_InterMinisterialConsultations;
                obj.date_MoF_Concurrence = model.date_MoF_Concurrence;
                obj.date_DCN_submissionto_CCS = model.date_DCN_submissionto_CCS;
                obj.ApproveToCSS_CFA = model.ApproveToCSS_CFA;
                obj.DateContractSigning = model.DateContractSigning;

                obj.date_approval_CFA_CCS = model.date_approval_CFA_CCS;
                obj.date_ContractSigning = model.date_ContractSigning;

                obj.Eoi_DPRDateIssueMakeI = model.Eoi_DPRDateIssueMakeI;
                obj.Eoi_DPRReceiptDateMakeI = model.Eoi_DPRReceiptDateMakeI;
                obj.Eoi_DPRDateCompletionandResponseMakeI = model.Eoi_DPRDateCompletionandResponseMakeI;
                obj.dtforwardingshortlistedDPMakeI = model.dtforwardingshortlistedDPMakeI;
                obj.dateapprovalshortlistedvendorsMakeI = model.dateapprovalshortlistedvendorsMakeI;

                obj.dateselectionDasbySHQ_dprbMakeI = model.dateselectionDasbySHQ_dprbMakeI;
                obj.dateapprovalCFAfundingarrangementissueProjectMakeI = model.dateapprovalCFAfundingarrangementissueProjectMakeI;

                obj.dateselectionDasbySHQ_dprbMakeII = model.dateselectionDasbySHQ_dprbMakeII;
                obj.dateapprovalCFAfundingarrangementissueProjectMakeII = model.dateapprovalCFAfundingarrangementissueProjectMakeII;

                obj.datecommencementpreliminaryMakeI = model.datecommencementpreliminaryMakeI;
                obj.datefinalisationdetaileddesign = model.datefinalisationdetaileddesign;
                obj.datecommencementfabrication_devProMakeI = model.datecommencementfabrication_devProMakeI;
                obj.datetechnicallimitedfieldstrialsMakeI = model.datetechnicallimitedfieldstrialsMakeI;
                obj.datefreezingPSQRStoSQRSMakeI = model.datefreezingPSQRStoSQRSMakeI;
                obj.dateintimationvendorssubmissionrevisedMakeI = model.dateintimationvendorssubmissionrevisedMakeI;
                obj.datesubmissionrevisedcommercialoffersvendorsMakeI = model.datesubmissionrevisedcommercialoffersvendorsMakeI;

                obj.DateIssueMake_II = model.DateIssueMake_II;
                obj.EoiReceiptDateMake_II = model.EoiReceiptDateMake_II;
                obj.EoiCompletionResponseMake_II = model.EoiCompletionResponseMake_II;
                obj.dateApprovalEoiMake_II = model.dateApprovalEoiMake_II;
                obj.dateissueProjectSenctionMake_II = model.dateissueProjectSenctionMake_II;

                obj.datecommencementDevPrototypeMakeII = model.datecommencementDevPrototypeMakeII;
                obj.datecompletionDevPrototypeMakeII = model.datecompletionDevPrototypeMakeII;
                obj.datecommencementUserTrialsMakeII = model.datecommencementUserTrialsMakeII;
                obj.datecompletionUserTrialsRedinessReviewMakeII = model.datecompletionUserTrialsRedinessReviewMakeII;
                obj.dateFreezingPSQRsMakeII = model.dateFreezingPSQRsMakeII;

                obj.dateFormulationJPMT_DD = model.dateFormulationJPMT_DD;
                obj.dateFormulationPMT_DD = model.dateFormulationPMT_DD;
                obj.dateIdentificationDCPP_DD = model.dateIdentificationDCPP_DD;
                obj.datedetaildesignbySHQ_DD = model.datedetaildesignbySHQ_DD;
                obj.dateissuetrialdeveiation_DD = model.dateissuetrialdeveiation_DD;
                obj.dateprojectReview_DD = model.dateprojectReview_DD;
                obj.daterealisationPrototype_DD = model.daterealisationPrototype_DD;
                obj.dateCommencementPSQR = model.dateCommencementPSQR;
                obj.dateCulminationPSQRValid_DD = model.dateCulminationPSQRValid_DD;


                obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                obj.CreatedOn = System.DateTime.Now;
                entities.acq_rfp_master.Add(obj);
                entities.SaveChanges();

                var LastInsertrPFId = obj.rfp_id;

                acq_rfp_vendors rfpVendor = new acq_rfp_vendors();
                rfpVendor.Rfp_fk_Id = LastInsertrPFId;
                entities.acq_rfp_vendors.Add(rfpVendor);
                entities.SaveChanges();

                var LastInsertId = rfpVendor.rfp_id;

                acq_rfp_VendorsDetails objdeta = new acq_rfp_VendorsDetails();
                if (model.SelectedIDArray != null)
                {
                    foreach (var item in model.SelectedIDArray)
                    {
                        objdeta.rfpID = LastInsertId;
                        objdeta.vendor_id = Convert.ToInt32(item);
                        objdeta.type_ID = model.type_ID;
                        entities.acq_rfp_VendorsDetails.Add(objdeta);
                        entities.SaveChanges();
                    }
                }
                ViewBag.Message = "Data Saved Successfully";
                //TempData["Message"] = "Data Saved Successfully..!!";
                //return RedirectToAction("Index");
            }

            else if (model.rfp_id > 0)
            {
                var _updateAon = entities.acq_rfp_master.Where(x => x.rfp_id == model.rfp_id).FirstOrDefault();
                if (_updateAon != null)
                {
                    _updateAon.type_rfp = model.type_rfp;
                    _updateAon.aon_id = model.aon_id;
                    _updateAon.draft_rfp_date = model.draft_rfp_date;
                    _updateAon.date_submissionto_adgacq = model.date_submissionto_adgacq;
                    _updateAon.date_acceptanceby_adgacq = model.date_acceptanceby_adgacq;
                    _updateAon.date_collegiate_vetting = model.date_collegiate_vetting;
                    _updateAon.date_approval_rfp = model.date_approval_rfp;
                    _updateAon.date_issue_rfp = model.date_issue_rfp;
                    _updateAon.date_prebid_meeting = model.date_prebid_meeting;
                    _updateAon.date_prebid_replies = model.date_prebid_replies;
                    _updateAon.lastdate_bidsubmission = model.lastdate_bidsubmission;
                    _updateAon.date_bid_opening = model.date_bid_opening;
                    _updateAon.ExtendedLastDate_BidSubmission = model.ExtendedLastDate_BidSubmission;

                    _updateAon.date_tec_constitution = model.date_tec_constitution;
                    _updateAon.date_tec_commencement = model.date_tec_commencement;
                    _updateAon.date_tec_completion = model.date_tec_completion;
                    _updateAon.date_tec_reportsubmission = model.date_tec_reportsubmission;
                    _updateAon.date_tec_approval = model.date_tec_approval;
                    _updateAon.Date_toc_Constitution = model.Date_toc_Constitution;
                    _updateAon.Date_toc_report = model.Date_toc_report;
                    _updateAon.Date_toc_acceptance_CompetantAuth = model.Date_toc_acceptance_CompetantAuth;
                    _updateAon.Date_received_Fet = model.Date_received_Fet;
                    _updateAon.Date_accepted_Fet = model.Date_accepted_Fet;
                    _updateAon.Date_cnc_constitution = model.Date_cnc_constitution;
                    _updateAon.Date_cnc_benchmarking = model.Date_cnc_benchmarking;
                    _updateAon.Date_commercial_bid_opening = model.Date_commercial_bid_opening;
                    _updateAon.Date_cnc_conclusion = model.Date_cnc_conclusion;
                    _updateAon.Date_cnc_report = model.Date_cnc_report;

                    _updateAon.date_Submission_CFANote_DGacq = model.date_Submission_CFANote_DGacq;
                    _updateAon.date_Acq_approvalforsending_to_MoDFin = model.date_Acq_approvalforsending_to_MoDFin;
                    _updateAon.date_submissionto_CFA = model.date_submissionto_CFA;
                    _updateAon.date_Approval_CFAnote = model.date_Approval_CFAnote;

                    _updateAon.date_DCN_Sentfor_InterMinisterialConsultations = model.date_DCN_Sentfor_InterMinisterialConsultations;
                    _updateAon.date_MoF_Concurrence = model.date_MoF_Concurrence;
                    _updateAon.date_DCN_submissionto_CCS = model.date_DCN_submissionto_CCS;
                    _updateAon.ApproveToCSS_CFA = model.ApproveToCSS_CFA;
                    _updateAon.DateContractSigning = model.DateContractSigning;

                    _updateAon.date_approval_CFA_CCS = model.date_approval_CFA_CCS;
                    _updateAon.date_ContractSigning = model.date_ContractSigning;

                    _updateAon.Eoi_DPRDateIssueMakeI = model.Eoi_DPRDateIssueMakeI;
                    _updateAon.Eoi_DPRReceiptDateMakeI = model.Eoi_DPRReceiptDateMakeI;
                    _updateAon.Eoi_DPRDateCompletionandResponseMakeI = model.Eoi_DPRDateCompletionandResponseMakeI;
                    _updateAon.dtforwardingshortlistedDPMakeI = model.dtforwardingshortlistedDPMakeI;
                    _updateAon.dateapprovalshortlistedvendorsMakeI = model.dateapprovalshortlistedvendorsMakeI;
                    _updateAon.dateselectionDasbySHQ_dprbMakeI = model.dateselectionDasbySHQ_dprbMakeI;
                    _updateAon.dateapprovalCFAfundingarrangementissueProjectMakeI = model.dateapprovalCFAfundingarrangementissueProjectMakeI;

                    _updateAon.dateselectionDasbySHQ_dprbMakeII = model.dateselectionDasbySHQ_dprbMakeII;
                    _updateAon.dateapprovalCFAfundingarrangementissueProjectMakeII = model.dateapprovalCFAfundingarrangementissueProjectMakeII;

                    _updateAon.datecommencementpreliminaryMakeI = model.datecommencementpreliminaryMakeI;
                    _updateAon.datefinalisationdetaileddesign = model.datefinalisationdetaileddesign;
                    _updateAon.datecommencementfabrication_devProMakeI = model.datecommencementfabrication_devProMakeI;
                    _updateAon.datetechnicallimitedfieldstrialsMakeI = model.datetechnicallimitedfieldstrialsMakeI;
                    _updateAon.datefreezingPSQRStoSQRSMakeI = model.datefreezingPSQRStoSQRSMakeI;
                    _updateAon.dateintimationvendorssubmissionrevisedMakeI = model.dateintimationvendorssubmissionrevisedMakeI;
                    _updateAon.datesubmissionrevisedcommercialoffersvendorsMakeI = model.datesubmissionrevisedcommercialoffersvendorsMakeI;


                    _updateAon.DateIssueMake_II = model.DateIssueMake_II;
                    _updateAon.EoiReceiptDateMake_II = model.EoiReceiptDateMake_II;
                    _updateAon.EoiCompletionResponseMake_II = model.EoiCompletionResponseMake_II;
                    _updateAon.dateApprovalEoiMake_II = model.dateApprovalEoiMake_II;
                    _updateAon.dateissueProjectSenctionMake_II = model.dateissueProjectSenctionMake_II;

                    _updateAon.datecommencementDevPrototypeMakeII = model.datecommencementDevPrototypeMakeII;
                    _updateAon.datecompletionDevPrototypeMakeII = model.datecompletionDevPrototypeMakeII;
                    _updateAon.datecommencementUserTrialsMakeII = model.datecommencementUserTrialsMakeII;
                    _updateAon.datecompletionUserTrialsRedinessReviewMakeII = model.datecompletionUserTrialsRedinessReviewMakeII;
                    _updateAon.dateFreezingPSQRsMakeII = model.dateFreezingPSQRsMakeII;

                    _updateAon.dateFormulationJPMT_DD = model.dateFormulationJPMT_DD;
                    _updateAon.dateFormulationPMT_DD = model.dateFormulationPMT_DD;
                    _updateAon.dateIdentificationDCPP_DD = model.dateIdentificationDCPP_DD;
                    _updateAon.datedetaildesignbySHQ_DD = model.datedetaildesignbySHQ_DD;
                    _updateAon.dateissuetrialdeveiation_DD = model.dateissuetrialdeveiation_DD;
                    _updateAon.dateprojectReview_DD = model.dateprojectReview_DD;
                    _updateAon.daterealisationPrototype_DD = model.daterealisationPrototype_DD;
                    _updateAon.dateCommencementPSQR = model.dateCommencementPSQR;
                    _updateAon.dateCulminationPSQRValid_DD = model.dateCulminationPSQRValid_DD;

                    _updateAon.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAon.CreatedOn = System.DateTime.Now;
                    entities.SaveChanges();

                    var LastInsertrPFId = _updateAon.rfp_id;

                    var rfpID = entities.acq_rfp_vendors.Where(x => x.Rfp_fk_Id == LastInsertrPFId).FirstOrDefault();

                    var LastInsertId = rfpID.rfp_id;

                    if (model.type_ID == 1)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 1 && x.rfpID == LastInsertId).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 2)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 2).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 3)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 3).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    else if (model.type_ID == 4)
                    {
                        var _deleteRfpData = entities.acq_rfp_VendorsDetails.Where(x => x.type_ID == 4).ToList();
                        if (_deleteRfpData != null)
                        {
                            entities.acq_rfp_VendorsDetails.RemoveRange(_deleteRfpData);
                            entities.SaveChanges();
                        }
                    }
                    if (model.SelectedIDArray != null)
                    {
                        acq_rfp_VendorsDetails objdeta = new acq_rfp_VendorsDetails();
                        if (model.SelectedIDArray != null)
                        {
                            foreach (var item in model.SelectedIDArray)
                            {
                                objdeta.rfpID = LastInsertId;
                                objdeta.vendor_id = Convert.ToInt32(item);
                                objdeta.type_ID = model.type_ID;
                                entities.acq_rfp_VendorsDetails.Add(objdeta);
                                entities.SaveChanges();
                            }
                        }
                    }
                    ViewBag.Message = "Data Update Successfully";
                }
            }
            return View(model1);
        }

       
        public string GetRecordById(int id)
        {
            SaveAcqRFPMasterViewModel model = new SaveAcqRFPMasterViewModel();

            var Obj = entities.acq_rfp_master.Where(x => x.aon_id == id).FirstOrDefault();
            var getCategorization = entities.acq_project_master.Where(x => x.aon_id == id && x.IsDeleted == false).FirstOrDefault();
            model.aon_id = getCategorization.aon_id;
            model.Categorisation = getCategorization.Categorisation;
           

            if (Obj != null)
            {
                model.rfp_id = Obj.rfp_id;
                var rfpID = entities.acq_rfp_vendors.Where(x => x.Rfp_fk_Id == model.rfp_id).FirstOrDefault();

                // Vendor List
                List<SaveAcqRFPMasterViewModel> Badge = new List<SaveAcqRFPMasterViewModel>();
                List<SaveAcqRFPMasterViewModel> venList = new List<SaveAcqRFPMasterViewModel>();
                var vendorList = entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();

                //var query=from r in 

                var QUERYDATA = entities.GetselectedVendors();

                string query = "select r.vendorid,r.vendorname,(select 1 from acq_rfp_vendorsdetails p where p.rfpid=1 and p.vendor_id=r.VendorId and p.type_id=1)selected from tbl_tblvendor r order by 1";
                DataTable dt = return_datatable(query);

                foreach(var quer in QUERYDATA)
                {
                    Badge.Add(new SaveAcqRFPMasterViewModel
                    {
                        vendorID = quer.vendorid,
                        VendorName = Cipher.Decrypt(quer.vendorname, password),
                    });
                }
                //Badge = QUERYDATA.AsEnumerable().Select(x => new SaveAcqRFPMasterViewModel
                //{
                    
                //});
                if (Badge != null && Badge.Count() > 0)
                {
                    foreach (var item in Badge)
                    {
                        SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                        ObjVendor.vendorID = item.vendorID;
                        ObjVendor.VendorName = item.VendorName;
                        venList.Add(ObjVendor);
                    }
                }

                model.VendorMaster = venList;

                // Selected RFP Vendor
                //var rfpVendorLastRecord = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id).LastOrDefault();
                //int max = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id).Max(x => x.type_ID);
                var max = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id).Max(x => x.type_ID);


                int maxTypeid = Convert.ToInt16(max);
                List<SaveAcqRFPMasterViewModel> BidVendorListData = new List<SaveAcqRFPMasterViewModel>();
                if (rfpID != null)
                {
                    var rfpVendor = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id && x.type_ID == 1).ToList();
                    if (rfpVendor != null && rfpVendor.Count()>0)
                    {
                        foreach (var item in rfpVendor)
                        {
                            SaveAcqRFPMasterViewModel ObjVendor = new SaveAcqRFPMasterViewModel();
                            ObjVendor.vendorID = item.vendor_id;
                            ObjVendor.type_ID = item.type_ID;
                            ObjVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                            BidVendorListData.Add(ObjVendor);
                        }
                    }
                }
                model.SelectedVendorsType1 = BidVendorListData;


                // Selected Bid Vendor

                List<SaveAcqRFPMasterViewModel> TcpBIDListData = new List<SaveAcqRFPMasterViewModel>();
                if (rfpID != null)
                {
                    var bidVendor = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id && x.type_ID == maxTypeid).ToList();
                    if (bidVendor != null && bidVendor.Count()>0)
                    {
                        foreach (var item in bidVendor)
                        {
                            SaveAcqRFPMasterViewModel ObjBidVendor = new SaveAcqRFPMasterViewModel();
                            ObjBidVendor.vendorID = item.vendor_id;
                            ObjBidVendor.type_ID = item.type_ID;
                            ObjBidVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                            TcpBIDListData.Add(ObjBidVendor);
                        }
                    }
                    //else
                    //{
                    //    TcpBIDListData = BidVendorListData;
                    //}
                }
                model.SelectedBidType1 = TcpBIDListData;


                // Selected tcp Vendor
                List<SaveAcqRFPMasterViewModel> TcpVendorListData = new List<SaveAcqRFPMasterViewModel>();
                if (rfpID != null)
                {
                    var tcpVendor = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id && x.type_ID == maxTypeid).ToList();
                    if (tcpVendor != null && tcpVendor.Count()>0)
                    {
                        foreach (var item in tcpVendor)
                        {
                            SaveAcqRFPMasterViewModel ObjTCPVendor = new SaveAcqRFPMasterViewModel();
                            ObjTCPVendor.vendorID = item.vendor_id;
                            ObjTCPVendor.type_ID = item.type_ID;
                            ObjTCPVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                            TcpVendorListData.Add(ObjTCPVendor);
                        }
                    }
                }
                model.SelectedTcpType1 = TcpVendorListData;

                // Selected fet Vendor
                List<SaveAcqRFPMasterViewModel> FetVendorListData = new List<SaveAcqRFPMasterViewModel>();
                if (rfpID != null)
                {
                    var FetVendor = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id && x.type_ID == maxTypeid).ToList();
                
                    if (FetVendor != null)
                    {
                        foreach (var item in FetVendor)
                        {
                            SaveAcqRFPMasterViewModel ObjFETVendor = new SaveAcqRFPMasterViewModel();
                            ObjFETVendor.vendorID = item.vendor_id;
                            ObjFETVendor.type_ID = item.type_ID;
                            ObjFETVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                            FetVendorListData.Add(ObjFETVendor);
                        }
                    }
                }
                model.SelectedFetType1 = FetVendorListData;
                // Selected Contract Vendor
                List<SaveAcqRFPMasterViewModel> ContractListData = new List<SaveAcqRFPMasterViewModel>();
                if (rfpID != null)
                {
                    var ContractVendor = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == rfpID.rfp_id && x.type_ID == maxTypeid).ToList();

                    if (ContractVendor != null)
                    {
                        foreach (var item in ContractVendor)
                        {
                            SaveAcqRFPMasterViewModel ObjContVendor = new SaveAcqRFPMasterViewModel();
                            ObjContVendor.vendorID = item.vendor_id;
                            ObjContVendor.type_ID = item.type_ID;
                            ObjContVendor.VendorName = Cipher.Decrypt(item.tbl_tblVendor.VendorName, password);
                            ContractListData.Add(ObjContVendor);
                        }
                    }
                }

                model.ContractVendorList = ContractListData;
                //var struid = Convert.ToInt32(Session["UserID"].ToString());
                //var AoNID = id;
                Session["MAoN"] = id;

                //model.GetTrials = entities.acq_trials.Where(x => x.IsDeleted == false && x.CreatedBy == struid && x.aonID == AoNID).ToList();

               

                model.type_rfp = Obj.type_rfp;
                model.aon_id = Obj.aon_id;
                model.rfp_id = Obj.rfp_id;
                model.draft_rfp_date = Obj.draft_rfp_date;
                model.date_submissionto_adgacq = Obj.date_submissionto_adgacq;
                model.date_acceptanceby_adgacq = Obj.date_acceptanceby_adgacq;
                model.date_collegiate_vetting = Obj.date_collegiate_vetting;
                model.date_approval_rfp = Obj.date_approval_rfp;
                model.date_issue_rfp = Obj.date_issue_rfp;
                model.date_prebid_meeting = Obj.date_prebid_meeting;
                model.date_prebid_replies = Obj.date_prebid_replies;
                model.lastdate_bidsubmission = Obj.lastdate_bidsubmission;
                model.date_bid_opening = Obj.date_bid_opening;
                model.ExtendedLastDate_BidSubmission = Obj.ExtendedLastDate_BidSubmission;
                model.date_tec_constitution = Obj.date_tec_constitution;
                model.date_tec_commencement = Obj.date_tec_commencement;
                model.date_tec_completion = Obj.date_tec_completion;
                model.date_tec_reportsubmission = Obj.date_tec_reportsubmission;
                model.date_tec_approval = Obj.date_tec_approval;
                model.Date_toc_Constitution = Obj.Date_toc_Constitution;
                model.Date_toc_report = Obj.Date_toc_report;
                model.Date_toc_acceptance_CompetantAuth = Obj.Date_toc_acceptance_CompetantAuth;
                model.Date_received_Fet = Obj.Date_received_Fet;
                model.Date_accepted_Fet = Obj.Date_accepted_Fet;

                //model.Categorisation = Obj.acq_project_master.Categorisation;

                model.Categorisation = getCategorization.Categorisation;



                model.Date_cnc_constitution = Obj.Date_cnc_constitution;
                model.Date_cnc_benchmarking = Obj.Date_cnc_benchmarking;
                model.Date_commercial_bid_opening = Obj.Date_commercial_bid_opening;
                model.Date_cnc_conclusion = Obj.Date_cnc_conclusion;
                model.Date_cnc_report = Obj.Date_cnc_report;


                model.date_Submission_CFANote_DGacq = Obj.date_Submission_CFANote_DGacq;
                model.date_Acq_approvalforsending_to_MoDFin = Obj.date_Acq_approvalforsending_to_MoDFin;
                model.date_submissionto_CFA = Obj.date_submissionto_CFA;
                model.date_Approval_CFAnote = Obj.date_Approval_CFAnote;

                model.date_DCN_Sentfor_InterMinisterialConsultations = Obj.date_DCN_Sentfor_InterMinisterialConsultations;
                model.date_MoF_Concurrence = Obj.date_MoF_Concurrence;
                model.date_DCN_submissionto_CCS = Obj.date_DCN_submissionto_CCS;
                model.ApproveToCSS_CFA = Obj.ApproveToCSS_CFA;
                model.DateContractSigning = Obj.DateContractSigning;


                model.date_approval_CFA_CCS = Obj.date_approval_CFA_CCS;
                model.date_ContractSigning = Obj.date_ContractSigning;

                model.Eoi_DPRDateIssueMakeI = Obj.Eoi_DPRDateIssueMakeI;
                model.Eoi_DPRReceiptDateMakeI = Obj.Eoi_DPRReceiptDateMakeI;
                model.Eoi_DPRDateCompletionandResponseMakeI = Obj.Eoi_DPRDateCompletionandResponseMakeI;
                model.dtforwardingshortlistedDPMakeI = Obj.dtforwardingshortlistedDPMakeI;
                model.dateapprovalshortlistedvendorsMakeI = Obj.dateapprovalshortlistedvendorsMakeI;

                model.dateselectionDasbySHQ_dprbMakeI = Obj.dateselectionDasbySHQ_dprbMakeI;
                model.dateapprovalCFAfundingarrangementissueProjectMakeI = Obj.dateapprovalCFAfundingarrangementissueProjectMakeI;

                model.dateselectionDasbySHQ_dprbMakeII = Obj.dateselectionDasbySHQ_dprbMakeII;
                model.dateapprovalCFAfundingarrangementissueProjectMakeII = Obj.dateapprovalCFAfundingarrangementissueProjectMakeII;

                model.datecommencementpreliminaryMakeI = Obj.datecommencementpreliminaryMakeI;
                model.datefinalisationdetaileddesign = Obj.datefinalisationdetaileddesign;
                model.datecommencementfabrication_devProMakeI = Obj.datecommencementfabrication_devProMakeI;
                model.datetechnicallimitedfieldstrialsMakeI = Obj.datetechnicallimitedfieldstrialsMakeI;
                model.datefreezingPSQRStoSQRSMakeI = Obj.datefreezingPSQRStoSQRSMakeI;
                model.dateintimationvendorssubmissionrevisedMakeI = Obj.dateintimationvendorssubmissionrevisedMakeI;
                model.datesubmissionrevisedcommercialoffersvendorsMakeI = Obj.datesubmissionrevisedcommercialoffersvendorsMakeI;

                model.DateIssueMake_II = Obj.DateIssueMake_II;
                model.EoiReceiptDateMake_II = Obj.EoiReceiptDateMake_II;
                model.EoiCompletionResponseMake_II = Obj.EoiCompletionResponseMake_II;
                model.dateApprovalEoiMake_II = Obj.dateApprovalEoiMake_II;
                model.dateissueProjectSenctionMake_II = Obj.dateissueProjectSenctionMake_II;

                model.datecommencementDevPrototypeMakeII = Obj.datecommencementDevPrototypeMakeII;
                model.datecompletionDevPrototypeMakeII = Obj.datecompletionDevPrototypeMakeII;
                model.datecommencementUserTrialsMakeII = Obj.datecommencementUserTrialsMakeII;
                model.datecompletionUserTrialsRedinessReviewMakeII = Obj.datecompletionUserTrialsRedinessReviewMakeII;
                model.dateFreezingPSQRsMakeII = Obj.dateFreezingPSQRsMakeII;

                model.dateFormulationJPMT_DD = Obj.dateFormulationJPMT_DD;
                model.dateFormulationPMT_DD = Obj.dateFormulationPMT_DD;
                model.dateIdentificationDCPP_DD = Obj.dateIdentificationDCPP_DD;
                model.datedetaildesignbySHQ_DD = Obj.datedetaildesignbySHQ_DD;
                model.dateissuetrialdeveiation_DD = Obj.dateissuetrialdeveiation_DD;
                model.dateprojectReview_DD = Obj.dateprojectReview_DD;
                model.daterealisationPrototype_DD = Obj.daterealisationPrototype_DD;
                model.dateCommencementPSQR = Obj.dateCommencementPSQR;
                model.dateCulminationPSQRValid_DD = Obj.dateCulminationPSQRValid_DD;


                var VendorMasterObj = entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
                var VendorObj = entities.acq_rfp_VendorsDetails.Where(x => x.rfpID == Obj.rfp_id && x.type_ID == 1).ToList();
                var VendorList = new int[VendorMasterObj.Count];

            }
            // Get Trials Data
            var struid = Convert.ToInt32(Session["UserID"].ToString());
            model.GetTrials = entities.acq_trials.Where(x => x.IsDeleted == false && x.CreatedBy == struid && x.aonID == id).ToList();
            string data = JsonConvert.SerializeObject(model);
            return data;
        }
        [Route("MODAction")]
        public ActionResult Action(int? ID)
        {
            ACQTrialSaveViewModel model = new ACQTrialSaveViewModel();

            if (ID.HasValue)
            {
                var ACQTrial = entities.acq_trials.Where(x => x.id == ID.Value).FirstOrDefault();
                model.id = ACQTrial.id;
                model.trial_type = ACQTrial.trial_type;
                model.date_commencement = ACQTrial.date_commencement;
                model.date_completion = ACQTrial.date_completion;
                model.remarks = ACQTrial.remarks;
            }
            return PartialView("_Action", model);
        }


        [HttpPost]
        [Route("MODAction")]
        public JsonResult Action(ACQTrialSaveViewModel model)
        {
            JsonResult json = new JsonResult();

            var isExitTrials = entities.acq_trials.Where(x => x.trial_type == model.trial_type && x.aonID == model.aonID && x.IsDeleted != true).FirstOrDefault();
            if (isExitTrials != null)
            {
                json.Data = new { Success = false, Message = "Trials Already Exit.." };
                return json;
            }

            try
            {
                if (model.id == 0) // Save Records
                {

                    acq_trials obj = new acq_trials();
                    obj.aonID = model.aonID;
                    obj.trial_type = model.trial_type;
                    obj.date_commencement = model.date_commencement;
                    obj.date_completion = model.date_completion;
                    obj.remarks = model.remarks;
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.IsDeleted = false;
                    entities.acq_trials.Add(obj);
                    entities.SaveChanges();

                    json.Data = new { Success = true };
                }
                else if (model.id > 0) // Update Records
                {
                    var updateAcqTrials = entities.acq_trials.Where(x => x.id == model.id).FirstOrDefault();
                    updateAcqTrials.trial_type = model.trial_type;
                    updateAcqTrials.date_commencement = model.date_commencement;
                    updateAcqTrials.date_completion = model.date_completion;
                    updateAcqTrials.remarks = model.remarks;
                    updateAcqTrials.CreatedBy = Convert.ToInt32(Session["UserID"]); ;
                    updateAcqTrials.CreatedOn = System.DateTime.Now;
                    updateAcqTrials.DeletedBy = Convert.ToInt32(Session["UserID"]); ;
                    updateAcqTrials.DeletedOn = System.DateTime.Now;
                    updateAcqTrials.IsDeleted = false;
                    entities.SaveChanges();
                    json.Data = new { Success = true };
                }
            }
            catch (Exception ex)
            {
                json.Data = new { Success = false, Message = "Unable To Save and Update Records" };
                throw ex;
            }
            return json;
        }


        [HttpPost]
       [Route("MODDelete")]
        public ActionResult Delete(int ID)
        {
            try
            {
                var _deleteAcqTrials = entities.acq_trials.Where(x => x.id == ID).FirstOrDefault();
                if (_deleteAcqTrials != null)
                {
                    _deleteAcqTrials.IsDeleted = true;
                    _deleteAcqTrials.DeletedBy = Convert.ToInt32(Session["UserID"]);
                    _deleteAcqTrials.DeletedOn = System.DateTime.Now;
                    entities.SaveChanges();
                }

                else
                {
                    MODEntities entities = new MODEntities();
                    var getType = entities.acq_trials.Where(x => x.id == ID).ToList();
                    string data = JsonConvert.SerializeObject(getType);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }

        public JsonResult Edit(int ID)
        {
            JsonResult json = new JsonResult();
            ACQTrialSaveViewModel model = new ACQTrialSaveViewModel();

            if (ID != null)
            {
                var ACQTrial = entities.acq_trials.Where(x => x.id == ID).FirstOrDefault();
                model.id = ACQTrial.id;
                model.trial_type = ACQTrial.trial_type;
                model.date_commencement = ACQTrial.date_commencement;
                model.date_completion = ACQTrial.date_completion;
                model.remarks = ACQTrial.remarks;
            }
            return json;
        }
    }
}