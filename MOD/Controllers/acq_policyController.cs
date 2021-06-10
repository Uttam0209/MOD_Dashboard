using DDPAdmin.Services.Master;
using Ganss.XSS;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using MOD.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    public class acq_policyController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        MODEntities entities = new MODEntities();
        masterService mService = new masterService();
        string password = "p@SSword";

        public acq_policyController()
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
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "acq_policy")
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
                BruteForceAttackss.bcontroller = "acq_policy";
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
        // GET: acq_policy
        [SessionExpire]
        [SessionExpireRefNo]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Delay Due to Plocy Issues".ToLower())
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
            int SectionID = Convert.ToInt32(Session["SectionID"]);
            List<AcfpolicyviewModel> list = new List<AcfpolicyviewModel>();
            AcfpolicyviewModel model = new AcfpolicyviewModel();
            if (SectionID == 13)
            {
                var IndexData = entities.acq_policy.Where(x => x.IsDeleted == false).Select(s => new AcfpolicyviewModel { item_description = s.acq_project_master.item_description, aon_id = s.aon_id, tdate = s.tdate, fdate = s.fdate, policyid = s.policyid }).ToList();
                IndexData.ForEach(f =>
                {
                    f.item_description = Cipher.Decrypt(f.item_description, password);
                });
                ViewBag.Temdata = IndexData;
            }
            else
            {
                var IndexData = entities.acq_policy.Where(x => x.IsDeleted == false && x.section_id == SectionID).Select(s => new AcfpolicyviewModel { item_description = s.acq_project_master.item_description, aon_id = s.aon_id, tdate = s.tdate, fdate = s.fdate, policyid = s.policyid }).ToList();

                IndexData.ForEach(f =>
                {
                    f.item_description = Cipher.Decrypt(f.item_description, password);
                });
                ViewBag.Temdata = IndexData;
            }
            return View();
        }

        public string createFile(string data, string filename)
        {
            string filepath = "";
            string path = Server.MapPath("~/Attachments/PolicyIssues/" + filename);
            using (StreamWriter sw = System.IO.File.CreateText(path))
            {
                sw.WriteLine(data);
                sw.Close();
            }
            filepath = "~/Attachments/PolicyIssues/" + filename;
            return filepath;
        }

        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        // [Route("Acq_Index")]
        public ActionResult Index(FormCollection Fc)
        {
            HttpPostedFileBase fileBase = Request.Files["file"];
            byte[] fileDetails = new byte[fileBase.ContentLength];
            fileBase.InputStream.Read(fileDetails, 0, fileBase.ContentLength);
            string fileData = Cipher.Encrypt((Convert.ToBase64String(fileDetails)), password);
            string[] Filenamechar = fileBase.FileName.Split('.');
            Filenamechar[1] = ".txt";
            string Filename = Filenamechar[0] + Filenamechar[1];
            string FilePath = createFile(fileData, Filename);
            return RedirectToAction("Details");
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("ACreate")]
        public ActionResult Create()
        {
            // Project List Bind
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Delay Due to Plocy Issues".ToLower())
                    {
                      //  if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
            List<AcfpolicyviewModel> list = new List<AcfpolicyviewModel>();
            AcfpolicyviewModel model = new AcfpolicyviewModel();
            var SectionID = Session["SectionID"];
            List<tbl_mst_Template> Templateexists = entities.tbl_mst_Template.Where(x => x.IsActive == "Y").ToList();
            ViewBag.TemplateexistsData = Templateexists;
            if ((int)SectionID == 8 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "AirForce" && x.System_case != "Y")).OrderBy(x => x.item_description).ToList();
                ViewBag.ProjectList = projectList;
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        AcfpolicyviewModel obj = new AcfpolicyviewModel();
                        obj.stagid = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        obj.Categorisation = item.Categorisation;
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 9 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Army" && x.System_case != "Y")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        AcfpolicyviewModel obj = new AcfpolicyviewModel();
                        obj.stagid = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        obj.Categorisation = item.Categorisation;
                        list.Add(obj);
                    }
                }
            }
            if ((int)SectionID == 10 || (int)SectionID == 13)
            {
                var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service.Contains("Navy") || x.Service_Lead_Service.Contains("ICG") || x.System_case == "Y")).OrderBy(x => x.item_description).ToList();
                if (projectList != null)
                {
                    foreach (var item in projectList)
                    {
                        AcfpolicyviewModel obj = new AcfpolicyviewModel();
                        obj.stagid = item.aon_id;
                        obj.item_description = Cipher.Decrypt(item.item_description, password);
                        obj.Categorisation = item.Categorisation;
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
            // ViewBag.TemplateexistsData = model.StageList;
            model.ProjectList = list;
            TempData["ProjectList"] = list;
            return View(model);
        }
        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("ACreate")]
        public ActionResult Create(AcfpolicyviewModel model)
        {
            acq_policy obj = new acq_policy();
            try
            {
                string FilePath = "";
                HttpPostedFileBase fileBase = Request.Files["file"];
                if (fileBase.ContentLength > 0)
                {
                    byte[] fileDetails = new byte[fileBase.ContentLength];
                    fileBase.InputStream.Read(fileDetails, 0, fileBase.ContentLength);
                    string fileData = Cipher.Encrypt((Convert.ToBase64String(fileDetails)), password);
                    string[] Filenamechar = fileBase.FileName.Split('.');
                    Filenamechar[1] = ".txt";
                    string Filename = Filenamechar[0] + Filenamechar[1];
                    FilePath = createFile(fileData, Filename);
                }

                using (DbContextTransaction dbTran = entities.Database.BeginTransaction())
                {
                    try
                    {
                        int SectionID = Convert.ToInt32(Session["SectionID"]);
                        obj.fdate = model.fdate;
                        obj.tdate = model.tdate;
                        obj.Remarks = sanitizer.Sanitize(Cipher.Encrypt(model.Remarks, password));
                        obj.aon_id = model.aon_id;
                        obj.section_id = SectionID;
                        obj.pdfattachment = FilePath;
                        obj.stagid = model.stagid; //Convert.ToInt32(model.TaskSlno);
                        obj.IsDeleted = false;
                        entities.acq_policy.Add(obj);
                        entities.SaveChanges();
                        dbTran.Commit();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        dbTran.Rollback();
                        throw;
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            List<tbl_mst_Template> Templateexists = entities.tbl_mst_Template.Where(x => x.IsActive == "Y").ToList();
            ViewBag.TemplateexistsData = Templateexists;
            model.ProjectList = TempData["ProjectList"] as List<AcfpolicyviewModel>;
            return View(model);
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("Delete")]
        public ActionResult Delete(int ID)
        {
            List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
            bool isAccessible = false;
            foreach (var item in RoleList)
            {
                if (item.FormName.ToLower() == "Delay Due to Plocy Issues".ToLower())
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
            try
            {
                var _deleteUser = entities.acq_policy.Where(x => x.policyid == ID).FirstOrDefault();
                if (_deleteUser != null)
                {
                    _deleteUser.IsDeleted = true;
                    _deleteUser.DeletedBy = Convert.ToInt32(Session["UserID"]);
                    _deleteUser.DeletedOn = System.DateTime.Now;
                    entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }
        [SessionExpire]
        [SessionExpireRefNo]
        // [Route("Edit")]
        public ActionResult Edit(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Delay Due to Plocy Issues".ToLower())
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
            AcfpolicyviewModel model = new AcfpolicyviewModel();
            List<AcfpolicyviewModel> list = new List<AcfpolicyviewModel>();
            try
            {
                // var _editPolicy = entities.acq_policy.Where(x => x.aon_id == ID && x.IsDeleted== false).FirstOrDefault();
                var _editPolicy = entities.acq_policy.Where(x => x.policyid == ID && x.IsDeleted == false).FirstOrDefault();
                var IndexData = entities.acq_policy.Select(s => new AcfpolicyviewModel { item_description = s.acq_project_master.item_description }).ToList();
                IndexData.ForEach(f =>
                {
                    f.item_description = Cipher.Decrypt(f.item_description, password);
                    model.item_description = f.item_description;
                });
                var IndexData1 = entities.acq_policy.Select(s => new AcfpolicyviewModel { Categorisation = s.tbl_mst_Template.TaskDescription }).ToList();
                //var IndexD = entities.acq_project_status_pendingstage.Where(s => s.TaskSlno == _editPolicy.stagid).Select(p => p.pending_in_stage).FirstOrDefault();

                IndexData1.ForEach(f =>
                {
                    model.Categorisation = f.Categorisation;
                });
                /* var IndexData2 = entities.acq_policy.Select(s => new AcfpolicyviewModel { Remarks = s.acq_project_master.Remarks }).ToList();
                 IndexData.ForEach(f =>
                 {
                     f.Remarks = Cipher.Decrypt(f.Remarks, password);
                     model.Remarks = f.Remarks;
                 });*/
                int SectionID = Convert.ToInt32(Session["SectionID"]);
                model.tdate = _editPolicy.tdate;
                model.policyid = _editPolicy.policyid;
                model.stagid = _editPolicy.stagid;
                model.aon_id = _editPolicy.aon_id;
                model.fdate = _editPolicy.fdate;
                //model.Remarks = _editPolicy.Remarks;
                model.Remarks = Cipher.Decrypt(_editPolicy.Remarks, password);
                //string filepath = _editPolicy.pdfattachment.ToString();
                /*
                                FileStream fs2 = new FileStream(Server.MapPath(filepath), FileMode.OpenOrCreate, FileAccess.Read);
                                StreamReader reader = new StreamReader(fs2);
                                string filePath = Cipher.Decrypt(reader.ReadToEnd(),password);
                                model.pdfattachment = filePath;*/
                //var SectionID = Session["SectionID"];
                List<tbl_mst_Template> Templateexists = entities.tbl_mst_Template.Where(x => x.IsActive == "Y").ToList();
                ViewBag.TemplateexistsData = Templateexists;
                if ((int)SectionID == 8 || (int)SectionID == 13)
                {
                    var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "AirForce" && x.System_case != "Y")).OrderBy(x => x.item_description).ToList();
                    ViewBag.ProjectList = projectList;
                    if (projectList != null)
                    {
                        foreach (var item in projectList)
                        {
                            AcfpolicyviewModel obj = new AcfpolicyviewModel();
                            obj.stagid = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Categorisation = item.Categorisation;
                            list.Add(obj);
                        }
                    }
                }
                if ((int)SectionID == 9 || (int)SectionID == 13)
                {
                    var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service == "Army" && x.System_case != "Y")).OrderBy(x => x.item_description).ToList();
                    if (projectList != null)
                    {
                        foreach (var item in projectList)
                        {
                            AcfpolicyviewModel obj = new AcfpolicyviewModel();
                            obj.stagid = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Categorisation = item.Categorisation;
                            list.Add(obj);
                        }
                    }
                }
                if ((int)SectionID == 10 || (int)SectionID == 13)
                {
                    var projectList = entities.acq_project_master.Where(x => x.IsDeleted == false && (x.Service_Lead_Service.Contains("Navy") || x.Service_Lead_Service.Contains("ICG") || x.System_case == "Y")).OrderBy(x => x.item_description).ToList();
                    if (projectList != null)
                    {
                        foreach (var item in projectList)
                        {
                            AcfpolicyviewModel obj = new AcfpolicyviewModel();
                            obj.stagid = item.aon_id;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Categorisation = item.Categorisation;
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
                //ViewBag.TemplateexistsData = model.StageList;
                model.ProjectList = list;
                // return View(model);
            }
            catch
            {

            }
            TempData["policyId"] = model.policyid;
            TempData["tdate"] = model.tdate;
            return View(model);
        }

        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        // [Route("Update")]
        public ActionResult Update(AcfpolicyviewModel model)
        {
            try
            {
                var updatePolicy = entities.acq_policy.Where(s => s.policyid == model.policyid).FirstOrDefault();
                updatePolicy.aon_id = model.aon_id;
                updatePolicy.tdate = model.tdate;
                updatePolicy.fdate = model.fdate;
                //updatePolicy.pdfattachment = Cipher.Encrypt(model.pdfattachment, password);
                updatePolicy.Remarks = sanitizer.Sanitize(Cipher.Encrypt(model.Remarks, password));
                updatePolicy.stagid = Convert.ToInt32(model.TaskSlno);
                updatePolicy.IsDeleted = false;
                entities.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("Index");
        }
    }


}