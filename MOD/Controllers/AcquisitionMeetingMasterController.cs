using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ganss.XSS;
using Gantt_Chart.Models;
using MOD.Models;
using MOD.Service;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    
    public class AcquisitionMeetingMasterController : Controller
    {
        MODEntities _entities = new MODEntities();
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        public AcquisitionMeetingMasterController()
        {
            
            BruteForce bruteForce = new BruteForce();
            BruteForceAttackss.bcontroller ="AcquisitionMeetingMaster";
            if (BruteForceAttackss.bcontroller != "")
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
                if (BruteForceAttackss.bcontroller == "AcquisitionMeetingMaster")
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
                BruteForceAttackss.bcontroller = "AcquisitionMeetingMaster";
            }
        }

        [SessionExpire]
        [SessionExpireRefNo]
        [Route("AIndex")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Meeting Master".ToLower())
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
            AcquisitionMeetingMasterListViewModel model = new AcquisitionMeetingMasterListViewModel();
            model.Meeting_MasterList = _entities.acq_meeting_master.Where(x=>x.Deleted == false).ToList();
            return View(model);
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("Create")]
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Meeting Master".ToLower())
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
            AcquisitionCreateMasterViewModel model = new AcquisitionCreateMasterViewModel();
            return View(model);
        }
        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("Create")]
        public ActionResult Create(AcquisitionCreateMasterViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    acq_meeting_master obj = new acq_meeting_master();
                    obj.dac_dpb = sanitizer.Sanitize(model.dac_dpb);
                    obj.meeting_date = model.meeting_date;
                    if(model != null)
                    {
                        obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    }
                    else
                    {
                        obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    }
                    obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    obj.Remarks = sanitizer.Sanitize(model.Remarks);
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.Deleted = false;
                    _entities.acq_meeting_master.Add(obj);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("Edit")]
        public ActionResult Edit(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Meeting Master".ToLower())
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
                var _editAcqMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == ID).FirstOrDefault();
                AcquisitionCreateMasterViewModel model = new AcquisitionCreateMasterViewModel();
                model.meeting_id = _editAcqMeeting.meeting_id;
                model.dac_dpb = _editAcqMeeting.dac_dpb;
                model.meeting_date = _editAcqMeeting.meeting_date;
                model.Date_of_Issue_of_Minutes = _editAcqMeeting.Date_of_Issue_of_Minutes;
                model.Remarks = _editAcqMeeting.Remarks;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("Update")]
        public ActionResult Update(AcquisitionCreateMasterViewModel model)
        {
            try
            {
                var _updateAcqMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == model.meeting_id).FirstOrDefault();
                if (_updateAcqMeeting != null)
                {

                    _updateAcqMeeting.dac_dpb = sanitizer.Sanitize(model.dac_dpb);
                    _updateAcqMeeting.meeting_date = model.meeting_date;
                    _updateAcqMeeting.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    _updateAcqMeeting.Remarks = sanitizer.Sanitize(model.Remarks);
                    _updateAcqMeeting.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateAcqMeeting.CreatedOn = System.DateTime.Now;
                    _updateAcqMeeting.Deleted = false;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("AIndex");
        }
        [SessionExpire]
        [SessionExpireRefNo]
       // [Route("Delete")]
        public ActionResult Delete(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Meeting Master".ToLower())
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
            try
            {
                var _deleteMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == ID).FirstOrDefault();
                if(_deleteMeeting != null)
                {
                    _deleteMeeting.DeletedBy = Convert.ToInt32(Session["UserID"]);
                    _deleteMeeting.DeletedOn = System.DateTime.Now;
                    _deleteMeeting.Deleted = true;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }
    }
}