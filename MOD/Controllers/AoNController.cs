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
using Ganss.XSS;
using static MOD.MvcApplication;
using MOD.Service;
using System.Configuration;

namespace MOD.Controllers
{
    //[Authorize]
    [SessionExpire]
    [SessionExpireRefNo]
    public class AoNController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        MODEntities _entities = new MODEntities();
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        string password = "p@SSword";
        masterService mService = new masterService();

        public AoNController()
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
            BindEncriptData();
            EncriptServicesData();
            BruteForce bruteForce = new BruteForce();
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "AoN")
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
                            if (System.Web.HttpContext.Current.Session["UserID"] != null)
                            {
                                List<UserViewModel> model = new List<UserViewModel>();
                                model = bruteForce.GetUserLoginBlock(System.Web.HttpContext.Current.Session["UserID"].ToString());
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
                BruteForceAttackss.bcontroller = "AoN";
            }
        }

        public void BindEncriptData()
        {
            List<Categorisation> categorisations = new List<Categorisation>
            {
                new Categorisation { Text = "Buy(Indian-IDDM)",  Value = "Buy(Indian-IDDM)" },
                new Categorisation { Text = "Buy(Indian)",  Value = "Buy(Indian)" },
                new Categorisation { Text = "Buy and Make(Indian)",  Value = "Buy and Make(Indian)" },
                new Categorisation { Text = "Buy(Buy and Make)",  Value = "Buy and Make" },
                new Categorisation { Text = "Buy(Global-Manufacture in India)",  Value = "Buy(Global-Manufacture in India)" },
                new Categorisation { Text = "Buy(Global-Manufacture in India)-IGA",  Value = "Buy(Global-Manufacture in India)-IGA" },
                new Categorisation { Text = "Buy(Global)",  Value = "Buy(Global)" },
                new Categorisation { Text = "Buy(Global)-IGA",  Value = "Buy(Global)-IGA" },
                new Categorisation { Text = "Buy(Global)-FMS Route",  Value = "Buy(Global)-FMS Route" },
                new Categorisation { Text = "Design & Development",  Value = "Design & Development" },
                new Categorisation { Text = "IGA",  Value = "IGA" },
                new Categorisation { Text = "Make",  Value = "Make" },
                new Categorisation { Text = "Sp Model",  Value = "Sp Model" },
                new Categorisation { Text = "Make-I",  Value = "Make-I" },
                new Categorisation { Text = "Make-II",  Value = "Make-II" },
                new Categorisation { Text = "Make-III",  Value = "Make-III" }
            };

            // Categorisation Catdata = new Categorisation();
            List<Categorisation> Catdata = new List<Categorisation>();
            foreach (var item in categorisations)
            {
                Categorisation cat1 = new Categorisation()
                {
                    Text = item.Text,
                    Value = Cipher.Encrypt(item.Value, "")
                };
                Catdata.Add(cat1);
            };
            ViewBag.catData = Catdata;
        }

        public void EncriptServicesData()
        {
            List<Categorisation> categorisations = new List<Categorisation>
            {
                new Categorisation { Text = "Army",  Value = "Army" },
                new Categorisation { Text = "Navy",  Value = "Navy" },
                new Categorisation { Text = "AirForce",  Value = "AirForce" },
                new Categorisation { Text = "ICG",  Value = "ICG" },
                new Categorisation { Text = "Joint",  Value = "Joint" }
            };
            List<Categorisation> Catdata = new List<Categorisation>();
            foreach (var item in categorisations)
            {
                Categorisation cat1 = new Categorisation()
                {
                    Text = item.Text,
                    Value = Cipher.Encrypt(item.Value, "")
                };
                Catdata.Add(cat1);
            };
            ViewBag.ServicesData = Catdata;
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

        //[Route("AoNIndex")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "AON Registration".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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

        [Route("AoNCreate")]
        public ActionResult AonCreate()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "AON Registration".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
            SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();
            model.MeetingMaster = _entities.acq_meeting_master.ToList();

            // Vendor List
            List<SaveAcqProjectMasterViewModel> venList = new List<SaveAcqProjectMasterViewModel>();
            var vendorList = _entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
            if (vendorList != null)
            {
                foreach (var item in vendorList)
                {
                    SaveAcqProjectMasterViewModel ObjVendor = new SaveAcqProjectMasterViewModel();
                    ObjVendor.vendorID = item.VendorId;
                    ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                    venList.Add(ObjVendor);
                }
            }
            model.VendorList = venList;

            // SectionList
            model.SectionMasterList = _entities.acq_section_master.ToList();
            // UserList 
            model.UserList = _entities.tbl_tbl_User.ToList();
            ViewBag.venderlist = _entities.tbl_tbl_User.ToList();
            return View(model);
        }

        [HttpPost]
        [Route("AoNCreate")]
        public ActionResult AonCreate(SaveAcqProjectMasterViewModel model)
        {
            model.MeetingMaster = _entities.acq_meeting_master.ToList();
            SaveAcqProjectMasterViewModel model1 = new SaveAcqProjectMasterViewModel();
            if (ModelState.IsValid)
            {
                try
                {
                    acq_project_master obj = new acq_project_master();
                    obj.Date_of_Accord_of_AoN = Convert.ToDateTime(model.Date_of_Accord_of_AoN);
                    obj.DPP_DAP = model.DPP_DAP;
                    ////Encrypted SHA Item Name
                    string itemName = sanitizer.Sanitize(Cipher.Encrypt(model.item_description, password));
                    obj.item_description = itemName;

                    ////Encrypted Item Name
                    //string itemName = Encryption.Encrypt(model.item_description);
                    //obj.item_description = itemName;
                    obj.Quantity = sanitizer.Sanitize(model.Quantity);
                    obj.Cost = sanitizer.Sanitize(Convert.ToDecimal(model.Cost).ToString());
                    obj.Tax_Duties = sanitizer.Sanitize(model.Tax_Duties);
                    obj.meeting_id = model.meeting_id;
                    obj.Service_Lead_Service = sanitizer.Sanitize(model.Service_Lead_Service);
                    obj.Type_of_Acquisition = sanitizer.Sanitize(model.Type_of_Acquisition);
                    obj.Categorisation = sanitizer.Sanitize(model.Categorisation);
                    obj.Trials_Required = sanitizer.Sanitize(model.Trials_Required);
                    obj.Essential_parameters = sanitizer.Sanitize(model.Essential_parameters);
                    obj.Any_other_aspect = sanitizer.Sanitize(model.Any_other_aspect);
                    obj.IC_percentage = sanitizer.Sanitize(model.IC_percentage);
                    obj.Option_clause_applicable = sanitizer.Sanitize(model.Option_clause_applicable);
                    obj.Offset_applicable = sanitizer.Sanitize(model.Offset_applicable);
                    obj.AMC_applicable = sanitizer.Sanitize(model.AMC_applicable);
                    obj.AMCRemarks = sanitizer.Sanitize(model.AMCRemarks);
                    obj.Warrenty_applicable = sanitizer.Sanitize(model.Warrenty_applicable);
                    obj.Warrenty_Remarks = sanitizer.Sanitize(model.Warrenty_Remarks);
                    obj.AoN_validity = sanitizer.Sanitize(model.AoN_validity);
                    obj.AoN_validity_unit = model.AoN_validity_unit;
                    obj.Remarks = sanitizer.Sanitize(model.Remarks);
                    obj.Currency = sanitizer.Sanitize(model.Currency);
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.VendorsIDs = sanitizer.Sanitize(model.VendorsIDs.ToString());
                    obj.DirectorateId = model.DirectorateId;
                    obj.ResponsiblePersonLeve1 = sanitizer.Sanitize(model.ResponsiblePersonLeve1);
                    obj.ResponsiblePersonLeve2 = sanitizer.Sanitize(model.ResponsiblePersonLeve2);
                    obj.ResponsiblePersonLeve3 = sanitizer.Sanitize(model.ResponsiblePersonLeve3);
                    obj.ResponsiblePersonLeve4 = sanitizer.Sanitize(model.ResponsiblePersonLeve4);
                    obj.IsDeleted = false;
                    obj.System_case = sanitizer.Sanitize(model.System_case);
                    //obj.AovType = sanitizer.Sanitize(model.AonType);
                    _entities.acq_project_master.Add(obj);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                model.MeetingMaster = _entities.acq_meeting_master.ToList();
                List<SaveAcqProjectMasterViewModel> venList = new List<SaveAcqProjectMasterViewModel>();
                var vendorList = _entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
                if (vendorList != null)
                {
                    foreach (var item in vendorList)
                    {
                        SaveAcqProjectMasterViewModel ObjVendor = new SaveAcqProjectMasterViewModel();
                        ObjVendor.vendorID = item.VendorId;
                        ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                        venList.Add(ObjVendor);
                    }
                }
                model.VendorList = venList;
                // SectionList
                model.SectionMasterList = _entities.acq_section_master.ToList();
                // UserList 
                model.UserList = _entities.tbl_tbl_User.ToList();
            }

            return View(model);
        }

        [Route("AoNEdit")]
        public ActionResult Edit(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "AON Registration".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
            SaveAcqProjectMasterViewModel model = new SaveAcqProjectMasterViewModel();

            model.SectionMasterList = _entities.acq_section_master.ToList();
            model.UserList = _entities.tbl_tbl_User.ToList();
            // Vendor List
            List<SaveAcqProjectMasterViewModel> venList = new List<SaveAcqProjectMasterViewModel>();
            var vendorList = _entities.tbl_tblVendor.Where(x => x.IsDeleted == false).ToList();
            if (vendorList != null)
            {
                foreach (var item in vendorList)
                {
                    SaveAcqProjectMasterViewModel ObjVendor = new SaveAcqProjectMasterViewModel();
                    ObjVendor.vendorID = item.VendorId;
                    ObjVendor.VendorName = Cipher.Decrypt(item.VendorName, password);
                    venList.Add(ObjVendor);
                }
            }
            model.VendorList = venList;

            try
            {
                var _editAonData = _entities.acq_project_master.Where(x => x.aon_id == ID).FirstOrDefault();
                model.aon_id = _editAonData.aon_id;
                model.Date_of_Accord_of_AoN = Convert.ToDateTime(_editAonData.Date_of_Accord_of_AoN);
                model.DPP_DAP = _editAonData.DPP_DAP;
                model.item_description = Cipher.Decrypt(_editAonData.item_description, password);
                model.Quantity = _editAonData.Quantity;
                model.Cost = Convert.ToDecimal(_editAonData.Cost == "" || _editAonData.Cost == null ? "0" : _editAonData.Cost);
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
                model.DirectorateId = _editAonData.DirectorateId;
                model.VendorsIDs = Convert.ToInt16(_editAonData.VendorsIDs == "" ? "0" : _editAonData.VendorsIDs);
                model.ResponsiblePersonLeve1 = _editAonData.ResponsiblePersonLeve1;
                model.ResponsiblePersonLeve2 = _editAonData.ResponsiblePersonLeve2;
                model.ResponsiblePersonLeve3 = _editAonData.ResponsiblePersonLeve3;
                model.ResponsiblePersonLeve4 = _editAonData.ResponsiblePersonLeve4;
                // Meeting DropDown Bind
                model.MeetingMaster = _entities.acq_meeting_master.ToList();
                model.System_case = _editAonData.System_case;
                //model.AonType = _editAonData.AovType;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        // [Route("AoNUpdate")]
        public ActionResult Update(SaveAcqProjectMasterViewModel model)
        {
            try
            {
                var _updateAon = _entities.acq_project_master.Where(x => x.aon_id == model.aon_id).FirstOrDefault();
                if (_updateAon != null)
                {
                    _updateAon.Date_of_Accord_of_AoN = Convert.ToDateTime(model.Date_of_Accord_of_AoN);
                    _updateAon.DPP_DAP = sanitizer.Sanitize(model.DPP_DAP);
                    _updateAon.item_description = sanitizer.Sanitize(Cipher.Encrypt(model.item_description, password));
                    _updateAon.Quantity = sanitizer.Sanitize(model.Quantity);
                    _updateAon.Cost = sanitizer.Sanitize(model.Cost.ToString());
                    _updateAon.meeting_id = model.meeting_id;
                    _updateAon.Service_Lead_Service = sanitizer.Sanitize(model.Service_Lead_Service);
                    _updateAon.Type_of_Acquisition = sanitizer.Sanitize(model.Type_of_Acquisition);
                    _updateAon.Categorisation = sanitizer.Sanitize(model.Categorisation);
                    _updateAon.Trials_Required = sanitizer.Sanitize(model.Trials_Required);
                    _updateAon.Essential_parameters = sanitizer.Sanitize(model.Essential_parameters);
                    _updateAon.Any_other_aspect = sanitizer.Sanitize(model.Any_other_aspect);
                    _updateAon.IC_percentage = sanitizer.Sanitize(model.IC_percentage);
                    _updateAon.Option_clause_applicable = sanitizer.Sanitize(model.Option_clause_applicable);
                    _updateAon.Offset_applicable = sanitizer.Sanitize(model.Offset_applicable);
                    _updateAon.AMC_applicable = sanitizer.Sanitize(model.AMC_applicable);
                    _updateAon.AoN_validity = sanitizer.Sanitize(model.AoN_validity);
                    _updateAon.AoN_validity_unit = sanitizer.Sanitize(model.AoN_validity_unit);
                    _updateAon.Remarks = sanitizer.Sanitize(model.Remarks);
                    _updateAon.Tax_Duties = sanitizer.Sanitize(model.Tax_Duties);
                    _updateAon.AMCRemarks = sanitizer.Sanitize(model.AMCRemarks);
                    _updateAon.Warrenty_applicable = sanitizer.Sanitize(model.Warrenty_applicable);
                    _updateAon.Warrenty_Remarks = sanitizer.Sanitize(model.Warrenty_Remarks);
                    _updateAon.Currency = sanitizer.Sanitize(model.Currency);
                    _updateAon.DirectorateId = model.DirectorateId;
                    _updateAon.VendorsIDs = sanitizer.Sanitize(model.VendorsIDs.ToString());
                    _updateAon.ResponsiblePersonLeve1 = sanitizer.Sanitize(model.ResponsiblePersonLeve1);
                    _updateAon.ResponsiblePersonLeve2 = sanitizer.Sanitize(model.ResponsiblePersonLeve2);
                    _updateAon.ResponsiblePersonLeve3 = sanitizer.Sanitize(model.ResponsiblePersonLeve3);
                    _updateAon.ResponsiblePersonLeve4 = sanitizer.Sanitize(model.ResponsiblePersonLeve4);
                    _updateAon.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAon.CreatedOn = System.DateTime.Now;
                    _updateAon.IsDeleted = false;
                    _updateAon.System_case = sanitizer.Sanitize(model.System_case);
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("Index");
        }

        //[Route("AoNDelete")]
        public ActionResult Delete(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower().Contains("AON Registration"))
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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

        [Route("AoNViewAoNList")]
        public ActionResult ViewAoNList(DateTime? StartDate, DateTime? EndDate, string Categorisation, string Service_Lead_Service)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "AON Granted During a Given Period Category-wise Alongwith Costs".ToLower())
                    {
                        //if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
            string Categorisation1 = null;
            string Service_Lead_Service1 = null;
            if (Categorisation != null & Categorisation != "")
            {
                Categorisation1 = Cipher.Decrypt(Categorisation, "");
            }
            else
            {
                Categorisation1 = Categorisation;
            }
            if (Service_Lead_Service != null & Service_Lead_Service != "")
            {
                Service_Lead_Service1 = Cipher.Decrypt(Service_Lead_Service, "");
            }
            else
            {
                Service_Lead_Service1 = Service_Lead_Service;
            }
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
                            // obj.Categorisation = Categorisation1;
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
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation1) && x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList());

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
                            // obj.Categorisation =Categorisation1;
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
                            // obj.Categorisation = Categorisation1;
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
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation1) && x.Service_Lead_Service.Contains(Service_Lead_Service1)).ToList());
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
                            // obj.Categorisation = Categorisation;
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
                            // obj.Categorisation = Categorisation1;
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
                            objAonLst.AddRange(AonList.Where(x => x.Categorisation.Contains(Categorisation1) && x.Service_Lead_Service.Contains(Service_Lead_Service1)).ToList());
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
                            // obj.Categorisation = Categorisation1;
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
                            //obj.Categorisation =Categorisation1;
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
                        if (AonList.Count() > 0)
                        {
                            var objAonLst = new List<acq_project_master>();
                            if (!string.IsNullOrEmpty(Categorisation) && !string.IsNullOrEmpty(Service_Lead_Service))
                            {
                                objAonLst.AddRange(AonList.Where(x => x.Categorisation == Categorisation1 && x.Service_Lead_Service == Service_Lead_Service1).ToList());
                            }
                            else if (!string.IsNullOrEmpty(Categorisation))
                            {
                                objAonLst.AddRange(AonList.Where(x => x.Categorisation == Categorisation1).ToList());
                            }
                            else if (!string.IsNullOrEmpty(Service_Lead_Service))
                            {
                                objAonLst.AddRange(AonList.Where(x => x.Service_Lead_Service == Service_Lead_Service1).ToList());
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
                                // obj.Categorisation = Categorisation1;
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
                }
                model.AonList = list;
            }
            string query = "";

            if (Service_Lead_Service1 == "" || Service_Lead_Service1 == null)
                Service_Lead_Service1 = "%";

            if (Categorisation1 == "" || Categorisation1 == null)
                Categorisation1 = "%";

            query = "select Financial_year," +
" count(*) no_of_aons,sum(cast(y.cost as decimal(16, 2))) total_cost_in_crs from" +
" (select y.*,concat('FY', CASE WHEN MONTH(y.date_of_accord_of_aon) < 4 THEN YEAR(y.date_of_accord_of_aon) - 1 ELSE" +
" YEAR(y.date_of_accord_of_aon) END) Financial_year from acq_project_master y )y" +
//" where y.Service_Lead_Service like '" + Service_Lead_Service1 + "' and y.categorisation like '" + Categorisation1 + "'" +
" where y.Service_Lead_Service like ? and y.categorisation like ? " +

" and  y.DeletedBy is null group by Financial_year";

            //DataTable dt = return_datatable(query);

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                // cmd.Parameters.AddWithValue("@Id", CatID);
                cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
                cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service1;
                cmd.Parameters.Add("@Categorisation", OleDbType.VarChar, 500);
                cmd.Parameters["@Categorisation"].Value = Categorisation1;
                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }

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
        [Route("AoNExcelupload")]
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

        [Route("AoNForeClosure")]
        public ActionResult AoNForeClosure()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "AON ForeClosure".ToLower())
                    {
                        //if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
        [Route("AoNForeClosure")]
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