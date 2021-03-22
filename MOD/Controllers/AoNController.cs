using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDPAdmin.Services.Master;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using Newtonsoft.Json;

namespace MOD.Controllers
{
    [Authorize]
    public class AoNController : Controller
    {
        MODEntities _entities = new MODEntities();
        string password = "p@SSword";
        masterService mService = new masterService();

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
        public ActionResult Index()
        {
            MODListViewModel model = new MODListViewModel();
            List<MODListViewModel> list = new List<MODListViewModel>();

            var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false).ToList();
            if (AonList != null)
            {
                foreach (var item in AonList)
                {
                    MODListViewModel obj = new MODListViewModel();
                    obj.aon_id = item.aon_id;
                    obj.item_description = Cipher.Decrypt(item.item_description, password);
                    obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                    obj.Quantity = item.Quantity;
                    obj.Service_Lead_Service = item.Service_Lead_Service;
                    obj.Categorisation = item.Categorisation;
                    obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                    obj.AoNClosureDate = item.AoNClosureDate;
                    obj.Cost = item.Cost;
                    obj.System_case = item.System_case;
                    list.Add(obj);
                }
            }
            model.AonList = list;
            return View(model);
        }
        public ActionResult AonCreate()
        {
            SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();
            model.MeetingMaster = _entities.acq_meeting_master.ToList();
            return View(model);
        }

        [HttpPost]
        public ActionResult AonCreate(SaveAcqProjectMasterViewModel model)
        {
            model.MeetingMaster = _entities.acq_meeting_master.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    acq_project_master obj = new acq_project_master();
                    obj.Date_of_Accord_of_AoN = Convert.ToDateTime(model.Date_of_Accord_of_AoN);
                    obj.DPP_DAP = model.DPP_DAP;

                    ////Encrypted SHA Item Name
                    string itemName = Cipher.Encrypt(model.item_description, password);
                    obj.item_description = itemName;

                    ////Encrypted Item Name
                    //string itemName = Encryption.Encrypt(model.item_description);
                    //obj.item_description = itemName;
                    obj.Quantity = model.Quantity;
                    obj.Cost = model.Cost;
                    obj.Tax_Duties = model.Tax_Duties;
                    obj.meeting_id = model.meeting_id;
                    obj.Service_Lead_Service = model.Service_Lead_Service;
                    obj.Type_of_Acquisition = model.Type_of_Acquisition;
                    obj.Categorisation = model.Categorisation;
                    obj.Trials_Required = model.Trials_Required;
                    obj.Essential_parameters = model.Essential_parameters;
                    obj.Any_other_aspect = model.Any_other_aspect;
                    obj.IC_percentage = model.IC_percentage;
                    obj.Option_clause_applicable = model.Option_clause_applicable;
                    obj.Offset_applicable = model.Offset_applicable;
                    obj.AMC_applicable = model.AMC_applicable;
                    obj.AMCRemarks = model.AMCRemarks;
                    obj.Warrenty_applicable = model.Warrenty_applicable;
                    obj.Warrenty_Remarks = model.Warrenty_Remarks;
                    obj.AoN_validity = model.AoN_validity;
                    obj.AoN_validity_unit = model.AoN_validity_unit;
                    obj.Remarks = model.Remarks;
                    obj.Currency = model.Currency;
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]); ;
                    obj.CreatedOn = System.DateTime.Now;
                    obj.IsDeleted = false;
                    obj.System_case = model.System_case;
                    _entities.acq_project_master.Add(obj);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(model);
        }

        public ActionResult Edit(int ID)
        {
            try
            {
                var _editAonData = _entities.acq_project_master.Where(x => x.aon_id == ID).FirstOrDefault();
                SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();
                model.aon_id = _editAonData.aon_id;
                model.Date_of_Accord_of_AoN = Convert.ToDateTime(_editAonData.Date_of_Accord_of_AoN);
                model.DPP_DAP = _editAonData.DPP_DAP;
                model.item_description = Cipher.Decrypt(_editAonData.item_description, password);
                model.Quantity = _editAonData.Quantity;
                model.Cost = _editAonData.Cost;
                model.meeting_id = _editAonData.meeting_id;
                model.Service_Lead_Service = _editAonData.Service_Lead_Service;
                model.Type_of_Acquisition = _editAonData.Type_of_Acquisition;
                model.Categorisation = _editAonData.Categorisation;
                model.Trials_Required = _editAonData.Trials_Required;
                model.Essential_parameters = _editAonData.Essential_parameters;
                model.Any_other_aspect = _editAonData.Any_other_aspect;
                model.IC_percentage = _editAonData.IC_percentage;
                model.Option_clause_applicable = _editAonData.Option_clause_applicable;
                model.Offset_applicable = _editAonData.Offset_applicable;
                model.AMC_applicable = _editAonData.AMC_applicable;
                model.AoN_validity = _editAonData.AoN_validity;
                model.AoN_validity_unit = _editAonData.AoN_validity_unit;
                model.Tax_Duties = _editAonData.Tax_Duties;
                model.AMCRemarks = _editAonData.AMCRemarks;
                model.Warrenty_applicable = _editAonData.Warrenty_applicable;
                model.Warrenty_Remarks = _editAonData.Warrenty_Remarks;
                model.Remarks = _editAonData.Remarks;
                model.Currency = _editAonData.Currency;
                // Meeting DropDown Bind
                model.MeetingMaster = _entities.acq_meeting_master.ToList();
                model.System_case = _editAonData.System_case;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(SaveAcqProjectMasterViewModel model)
        {
            try
            {
                var _updateAon = _entities.acq_project_master.Where(x => x.aon_id == model.aon_id).FirstOrDefault();
                if (_updateAon != null)
                {
                    _updateAon.Date_of_Accord_of_AoN = Convert.ToDateTime(model.Date_of_Accord_of_AoN);
                    _updateAon.DPP_DAP = model.DPP_DAP;
                    _updateAon.item_description = Cipher.Encrypt(model.item_description, password);
                    _updateAon.Quantity = model.Quantity;
                    _updateAon.Cost = model.Cost;
                    _updateAon.meeting_id = model.meeting_id;
                    _updateAon.Service_Lead_Service = model.Service_Lead_Service;
                    _updateAon.Type_of_Acquisition = model.Type_of_Acquisition;
                    _updateAon.Categorisation = model.Categorisation;
                    _updateAon.Trials_Required = model.Trials_Required;
                    _updateAon.Essential_parameters = model.Essential_parameters;
                    _updateAon.Any_other_aspect = model.Any_other_aspect;
                    _updateAon.IC_percentage = model.IC_percentage;
                    _updateAon.Option_clause_applicable = model.Option_clause_applicable;
                    _updateAon.Offset_applicable = model.Offset_applicable;
                    _updateAon.AMC_applicable = model.AMC_applicable;
                    _updateAon.AoN_validity = model.AoN_validity;
                    _updateAon.AoN_validity_unit = model.AoN_validity_unit;
                    _updateAon.Remarks = model.Remarks;
                    _updateAon.Tax_Duties = model.Tax_Duties;
                    _updateAon.AMCRemarks = model.AMCRemarks;
                    _updateAon.Warrenty_applicable = model.Warrenty_applicable;
                    _updateAon.Warrenty_Remarks = model.Warrenty_Remarks;
                    _updateAon.Currency = model.Currency;
                    _updateAon.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAon.CreatedOn = System.DateTime.Now;
                    _updateAon.IsDeleted = false;
                    _updateAon.System_case = model.System_case;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                var _deleteAoN = _entities.acq_project_master.Where(x => x.aon_id == ID).FirstOrDefault();
                if (_deleteAoN != null)
                {
                    _deleteAoN.IsDeleted = true;
                    _deleteAoN.DeletedBy = Convert.ToInt32(Session["UserID"]); ;
                    _deleteAoN.DeletedOn = System.DateTime.Now;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }

        public ActionResult ViewAoNList(DateTime? StartDate, DateTime? EndDate, string Categorisation, string Service_Lead_Service)
        {
            MODListViewModel model = new MODListViewModel();
            var SectionID = Session["SectionID"];

            if ((int)SectionID == 2 || (int)SectionID == 8)
            {
                var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "AirForce").OrderBy(x => x.Date_of_Accord_of_AoN).ToList();
                List<MODListViewModel> list = new List<MODListViewModel>();
                if (Categorisation == null && Service_Lead_Service == null)
                {
                    if (AonList != null)
                    {
                        foreach (var item in AonList)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                else
                {
                    if (StartDate.ToString() != null || EndDate.ToString() != null || !string.IsNullOrEmpty(Categorisation) || !string.IsNullOrEmpty(Service_Lead_Service))
                    {

                        var objAonLst = new List<acq_project_master>();
                        if (!string.IsNullOrEmpty(Categorisation))
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation) && x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());

                        //if (!string.IsNullOrEmpty(Service_Lead_Service))
                        //    objAonLst.AddRange(AonList.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());

                        if (StartDate.ToString() != null && EndDate.ToString() != null)
                            objAonLst.AddRange(AonList.Where(x => x.Date_of_Accord_of_AoN >= StartDate && x.Date_of_Accord_of_AoN <= EndDate).ToList());

                        foreach (var item in objAonLst)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                model.AonList = list;
            }
            else if ((int)SectionID == 3 || (int)SectionID == 9)
            {
                var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Army").OrderBy(x => x.Date_of_Accord_of_AoN).ToList();
                List<MODListViewModel> list = new List<MODListViewModel>();
                if (Categorisation == null && Service_Lead_Service == null)
                {
                    if (AonList != null)
                    {
                        foreach (var item in AonList)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                else
                {
                    if (StartDate.ToString() != null || EndDate.ToString() != null || !string.IsNullOrEmpty(Categorisation) || !string.IsNullOrEmpty(Service_Lead_Service))
                    {

                        var objAonLst = new List<acq_project_master>();
                        if (!string.IsNullOrEmpty(Categorisation))
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation) && x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        //if (!string.IsNullOrEmpty(Service_Lead_Service))
                        //    objAonLst.AddRange(AonList.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        if (StartDate.ToString() != null && EndDate.ToString() != null)
                            objAonLst.AddRange(AonList.Where(x => x.Date_of_Accord_of_AoN >= StartDate && x.Date_of_Accord_of_AoN <= EndDate).ToList());

                        foreach (var item in objAonLst)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                model.AonList = list;
            }
            else if ((int)SectionID == 4 || (int)SectionID == 10)
            {
                var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false && x.Service_Lead_Service == "Navy").OrderBy(x => x.Date_of_Accord_of_AoN).ToList();
                List<MODListViewModel> list = new List<MODListViewModel>();
                if (Categorisation == null && Service_Lead_Service == null)
                {
                    if (AonList != null)
                    {
                        foreach (var item in AonList)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                else
                {
                    if (StartDate.ToString() != null || EndDate.ToString() != null || !string.IsNullOrEmpty(Categorisation) || !string.IsNullOrEmpty(Service_Lead_Service))
                    {

                        var objAonLst = new List<acq_project_master>();
                        if (!string.IsNullOrEmpty(Categorisation))
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation) && x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        //if (!string.IsNullOrEmpty(Service_Lead_Service))
                        //    objAonLst.AddRange(AonList.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        if (StartDate.ToString() != null && EndDate.ToString() != null)
                            objAonLst.AddRange(AonList.Where(x => x.Date_of_Accord_of_AoN >= StartDate && x.Date_of_Accord_of_AoN <= EndDate).ToList());

                        foreach (var item in objAonLst)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                model.AonList = list;
            }
            else
            {
                var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false).ToList();
                List<MODListViewModel> list = new List<MODListViewModel>();
                if (Categorisation == null && Service_Lead_Service == null)
                {
                    if (AonList != null)
                    {
                        foreach (var item in AonList)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                else
                {
                    if (StartDate.ToString() != null || EndDate.ToString() != null || !string.IsNullOrEmpty(Service_Lead_Service) || !string.IsNullOrEmpty(Categorisation))
                    {

                        var objAonLst = new List<acq_project_master>();
                        if (!string.IsNullOrEmpty(Categorisation) && !string.IsNullOrEmpty(Service_Lead_Service))
                        {
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation) && x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        }
                        else if (!string.IsNullOrEmpty(Categorisation))
                        {
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation)).ToList());
                        }
                        else if (!string.IsNullOrEmpty(Service_Lead_Service))
                        {
                            objAonLst.AddRange(AonList.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());
                        }
                        else if (StartDate.ToString() != null && EndDate.ToString() != null)
                        {
                            objAonLst.AddRange(AonList.Where(x => x.Date_of_Accord_of_AoN >= StartDate && x.Date_of_Accord_of_AoN <= EndDate).ToList());
                        }


                        foreach (var item in objAonLst)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.aon_id = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.MeetingDate = Convert.ToDateTime(item.acq_meeting_master.meeting_date);
                            obj.Quantity = item.Quantity;
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.Categorisation = item.Categorisation;
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Currency = item.Currency;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.AoNaccordedbyDACDPB = item.acq_meeting_master.dac_dpb;

                            obj.Tax_Duties = item.Tax_Duties;
                            obj.Type_of_Acquisition = item.Type_of_Acquisition;
                            obj.Trials_Required = item.Trials_Required;
                            obj.Essential_parameters = item.Essential_parameters;
                            obj.Any_other_aspect = item.Any_other_aspect;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Option_clause_applicable = item.Option_clause_applicable;
                            obj.Offset_applicable = item.Offset_applicable;
                            obj.AMC_applicable = item.AMC_applicable;
                            obj.AMCRemarks = item.AMCRemarks;
                            obj.Warrenty_applicable = item.Warrenty_applicable;
                            obj.Warrenty_Remarks = item.Warrenty_Remarks;
                            obj.AoN_validity = item.AoN_validity;
                            obj.AoN_validity_unit = item.AoN_validity_unit;
                            obj.Remarks = item.Remarks;
                            list.Add(obj);
                        }
                    }
                }
                model.AonList = list;
            }
            string query = "";

            if (Service_Lead_Service == "" || Service_Lead_Service == null)
                Service_Lead_Service = "%";

            if (Categorisation == "" || Categorisation == null)
                Categorisation = "%";

            query = "select Financial_year," +
" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
" where y.Service_Lead_Service like '" + Service_Lead_Service + "' and y.categorisation like '" + Categorisation + "'" +
" and  y.DeletedBy is null group by Financial_year";


            //            if (Service_Lead_Service == "" && Categorisation == null)
            //            {

            //                query = "select Financial_year," +
            //" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
            //" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
            //" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
            //" where y.Service_Lead_Service like isnull(null, '%') and y.categorisation like isnull(null, '%')" +
            //" group by Financial_year";
            //            }
            //            else if (Service_Lead_Service!= null)
            //            {
            //                query = "select Financial_year," +
            //" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
            //" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
            //" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
            //" where y.Service_Lead_Service like isnull('" + Service_Lead_Service + "', '%') and y.categorisation like isnull(null, '%')" +
            //" group by Financial_year";

            //            }
            //            else if (Categorisation!= null)
            //            {
            //                query = "select Financial_year," +
            //" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
            //" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
            //" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
            //" where y.Service_Lead_Service like isnull(null, '%') and y.categorisation like isnull('" + Categorisation + "', '%')" +
            //" group by Financial_year";
            //            }
            //            else
            //            {
            //                query = "select Financial_year," +
            //" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
            //" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
            //" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
            //" where y.Service_Lead_Service like isnull('" + Service_Lead_Service + "', '%') and y.categorisation like isnull('" + Categorisation + "', '%')" +
            //" group by Financial_year";
            //            }
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            IEnumerable<AoNsGranted_WiseReport> Badge = null;

            DataTable dt = return_datatable(query);
            Badge = dt.AsEnumerable().Select(x => new AoNsGranted_WiseReport
            {
                Financial_year = x.Field<string>("Financial_year"),
                total_cost_in_crs = x.Field<decimal>("total_cost_in_crs"),
                no_of_aons = x.Field<int>("no_of_aons"),

            });
            foreach (AoNsGranted_WiseReport item in Badge)
            {
                dataPoints.Add(new DetailCharts(Convert.ToString(item.Financial_year), Convert.ToDouble(item.total_cost_in_crs), Convert.ToDouble(item.no_of_aons)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View(model);
        }

        OleDbConnection Econ;
        public ActionResult excelupload()
        {

            return View();
        }
        [HttpPost]

        public ActionResult excelupload(HttpPostedFileBase file)
        {

            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/excelfolder/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
            InsertExceldata(filepath, filename);
            TempData["DataUpload"] = "Data Upload Successfully";
            return View();
        }
        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }
        private void InsertExceldata(string fileepath, string filename)

        {

            string fullpath = Server.MapPath("/excelfolder/") + filename;

            ExcelConn(fullpath);

            string query = string.Format("Select * from [{0}]", "AoN$");

            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            Econ.Open();



            DataSet ds = new DataSet();

            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);

            Econ.Close();

            oda.Fill(ds);



            DataTable dt = ds.Tables[0];

            //model.MeetingMaster = _entities.acq_meeting_master.ToList();
            SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();

            try
            {
                acq_project_master obj = new acq_project_master();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ////Encrypted SHA Item Name
                        string itemName = Cipher.Encrypt(dt.Rows[i]["item_description"].ToString(), password);
                        obj.item_description = itemName;

                        ////Encrypted Item Name
                        //string itemName = Encryption.Encrypt(model.item_description);
                        //obj.item_description = itemName;
                        obj.Quantity = dt.Rows[i]["Quantity"].ToString();
                        obj.Cost = dt.Rows[i]["Cost"].ToString();
                        obj.Tax_Duties = dt.Rows[i]["Tax_Duties"].ToString();
                        obj.meeting_id = Convert.ToInt32(dt.Rows[i]["meeting_id"].ToString());
                        obj.Categorisation = dt.Rows[i]["Categorisation"].ToString();
                        obj.Service_Lead_Service = dt.Rows[i]["Service_Lead_Service"].ToString();
                        obj.Type_of_Acquisition = dt.Rows[i]["Type_of_Acquisition"].ToString();

                        obj.DPP_DAP = dt.Rows[i]["DPP_DAP"].ToString();
                        obj.AoN_Accorded_By = dt.Rows[i]["AoN_Accorded_By"].ToString();
                        obj.Date_of_Accord_of_AoN = Convert.ToDateTime(dt.Rows[i]["Date_of_Accord_of_AoN"].ToString());

                        obj.Trials_Required = dt.Rows[i]["Trials_Required"].ToString();
                        obj.Essential_parameters = dt.Rows[i]["Essential_parameters"].ToString();
                        obj.Any_other_aspect = dt.Rows[i]["Any_other_aspect"].ToString();
                        obj.IC_percentage = dt.Rows[i]["IC_percentage"].ToString();
                        obj.Option_clause_applicable = dt.Rows[i]["Option_clause_applicable"].ToString();
                        obj.Offset_applicable = dt.Rows[i]["Offset_applicable"].ToString();
                        obj.AMC_applicable = dt.Rows[i]["AMC_applicable"].ToString();
                        obj.AMCRemarks = dt.Rows[i]["AMCRemarks"].ToString();
                        obj.Warrenty_applicable = dt.Rows[i]["Warrenty_applicable"].ToString();
                        obj.Warrenty_Remarks = dt.Rows[i]["Warrenty_Remarks"].ToString();
                        obj.AoN_validity = dt.Rows[i]["Quantity"].ToString();
                        obj.AoN_validity_unit = dt.Rows[i]["AoN_validity"].ToString();
                        obj.Remarks = dt.Rows[i]["Remarks"].ToString();
                        obj.Currency = dt.Rows[i]["Currency"].ToString();
                        obj.CreatedBy = Convert.ToInt32(Session["UserID"]); ;
                        obj.CreatedOn = System.DateTime.Now;
                        obj.IsDeleted = false;
                        _entities.acq_project_master.Add(obj);
                        _entities.SaveChanges();
                        //return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult AoNForeClosure()
        {
            SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();
            List<SaveAcqProjectMasterViewModel> list = new List<SaveAcqProjectMasterViewModel>();
            var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false).ToList();
            if (AonList != null)
            {
                foreach (var item in AonList)
                {
                    SaveAcqProjectMasterViewModel obj = new SaveAcqProjectMasterViewModel();
                    obj.aon_id = item.aon_id;
                    obj.item_description = Cipher.Decrypt(item.item_description, password);
                    list.Add(obj);
                }
            }
            model.AoNList = list;
            return View(model);
        }

        [HttpPost]
        public ActionResult AoNForeClosure(SaveAcqProjectMasterViewModel model)
        {
            SaveAcqProjectMasterViewModel model1 = new SaveAcqProjectMasterViewModel();
            List<SaveAcqProjectMasterViewModel> list = new List<SaveAcqProjectMasterViewModel>();
            var AonList = _entities.acq_project_master.Where(x => x.IsDeleted == false).ToList();
            if (AonList != null)
            {
                foreach (var item in AonList)
                {
                    SaveAcqProjectMasterViewModel obj = new SaveAcqProjectMasterViewModel();
                    obj.aon_id = item.aon_id;
                    obj.item_description = Cipher.Decrypt(item.item_description, password);
                    list.Add(obj);
                }
            }
            model1.AoNList = list;

            var _updateAoNForeClosure = _entities.acq_project_master.Where(x => x.aon_id == model.aon_id && x.IsDeleted == false).FirstOrDefault();
            if (_updateAoNForeClosure != null)
            {
                try
                {
                    _updateAoNForeClosure.AoNClosureRemarks = model.AoNClosureRemarks;
                    _updateAoNForeClosure.AoNClosureDate = model.AoNClosureDate;
                    _updateAoNForeClosure.AoNForeClosureCreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAoNForeClosure.CreatedOn = System.DateTime.Now;
                    _updateAoNForeClosure.IsDeleted = false;
                    _entities.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            TempData["Msg"] = "Data Saved Successfully";
            return View(model1);
        }
    }
}