using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using Newtonsoft.Json;
using DDPAdmin.Services.Master;
using System.Web.Security;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MODEntities _entities;
        public HomeController()
        {
            _entities = new MODEntities();
        }
        masterService mService = new masterService();
        string password = "p@SSword";
        GanttData ganttData = new GanttData();
        //[SessionExpire]
        public ActionResult Index(string id)
        {

            if (id != null)
            {
                Int16 mId = Convert.ToInt16(Encryption.Decrypt(Request.QueryString["id"].ToString()));
                using (var _context = new MODEntities())
                {
                    var isValid = _context.tbl_tbl_User.Where(x => x.UserId == mId).FirstOrDefault();
                    if (isValid != null)
                    {
                        FormsAuthentication.SetAuthCookie(isValid.InternalEmailID, false);
                        Session["UserID"] = isValid.UserId;
                        Session["UserName"] = isValid.UserName;
                        Session["SectionID"] = isValid.SectionID;
                        return View();
                    }
                    else
                    {

                        TempData["Message"] = "Login Failed.User Name or Password Doesn't Exist.";
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else
            {
                return View();
            }
        }
       // [SessionExpire]
        public ActionResult Gantt(int Id)
        {
            ViewBag.DataSource = ganttData.ProjectNewData(Id);
            return View();
        }
        //[SessionExpire]
        public ActionResult Baseline(int Id)
        {
            ViewBag.DataSource = ganttData.BaselineData(Id);
            return View();
        }
        [HttpGet]
        public ActionResult GetTryProjectDetails(long id)
        {
            IEnumerable<tryProjectViewModel> Badge = null;
            Badge = mService.GetTryProjectDetails(id, Convert.ToInt64(Session["DepartID"].ToString()));
            ViewBag.Task = Badge;
            IEnumerable<SelectListItem> mTask = Badge.Select(m => new SelectListItem()
            {
                Text = m.TaskDescription.ToString(),
                Value = m.TaskSlno.ToString(),
            });
            return Json(mTask, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetCasesTable(decimal id,string Categorisation, string Service_Lead_Service,DateTime myDate)
        {
            List<CaseViewModel> list = new List<CaseViewModel>();
            IEnumerable<CaseViewModel> Badge = null;
            Badge = mService.GetCasesTable(id,Categorisation, Service_Lead_Service, myDate);
            foreach (CaseViewModel item in Badge)
            {
                CaseViewModel obj = new CaseViewModel
                {
                    BF = item.BF,
                    Cases = item.Cases,
                    Casesdisposed = item.Casesdisposed,
                    Outstanding = item.Outstanding,
                };
                list.Add(obj);
            }

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Dashboard(string Categorisation, string Service_Lead_Service)
        {
            int id = Convert.ToInt32(1);
            List<DashboardViewModel> list = new List<DashboardViewModel>();
            IEnumerable<DashboardViewModel> Badge = null;
            Badge = mService.GetDashboardDetails(Categorisation, Service_Lead_Service);
            ViewBag.Dashboard = Badge;
            //ViewBag.Categorisation = new SelectList(_entities.acq_project_master, "Categorisation", "Categorisation");
            //ViewBag.Service_Lead_Service = new SelectList(_entities.acq_project_master, "Service_Lead_Service", "Service_Lead_Service");        
            return View();
        }
        public JsonResult GetProjectDasboardDetails(string id, string mTaskSlno, string id5, string Categorisation, string Service_Lead_Service)
        {
            List<WaliTest> list = new List<WaliTest>();
            IEnumerable<WaliTest> Badge = null;
            if (ModelState.IsValid)
            {
                try
                {
                    Badge = mService.GetProjectDasboardDetails(id, mTaskSlno, id5, Categorisation,Service_Lead_Service);
                    foreach (WaliTest item in Badge)
                    {
                        WaliTest obj = new WaliTest
                        {
                            ProjectId = item.ProjectId,
                            ProjectName = Cipher.Decrypt(item.ProjectName, password),
                            Template_type = item.Template_type,
                            StartDate = item.StartDate,
                            AoNDate = Convert.ToDateTime(item.AoNDate.ToString("dd-MMM-yyyy")),
                            CreatedBy = item.CreatedBy,
                            IsActive = item.IsActive,
                            categorisation = item.Template_type,
                            service = item.service,
                            scheduled_date_of_completion = item.scheduled_date_of_completion,
                            actual_no_of_days = item.actual_no_of_days,
                            scheduled_no_of_days = item.scheduled_no_of_days,
                            percent_delay = item.percent_delay,
                        };
                        list.Add(obj);
                    }
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetMonthWiseChart(int monthid)
        {
            long mid = Convert.ToInt64(monthid);
            return Redirect("Home/GetDashboardDetails?id=" + mid);
            //int id = Convert.ToInt32(1);
            //List<DashboardViewModel> list = new List<DashboardViewModel>();
            //IEnumerable<DashboardViewModel> Badge = null;
            //Badge = mService.GetDashboardDetails(id);
            //ViewBag.Dashboard = Badge;
            //return View();
        }

        #region Charts Detasils
        public ActionResult DetailCharts()
        {
            //first chart pie
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            dataPoints = mService.Chart12();
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            //second barchart
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            dataPoints1 = mService.Chart123();
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            //first grid
            IEnumerable<Grid1> Badge1 = null;
            Badge1 = mService.GetGrid1();
            ViewBag.Grid1 = Badge1;
            //second grid
            IEnumerable<Grid2> Badge = null;
            Badge = mService.GetGrid2();
            ViewBag.Grid2 = Badge;
            ViewBag.Total = Badge.Sum(x => x.army_projects);
            ViewBag.Total1 = Badge.Sum(x => x.iaf_projects);
            ViewBag.Total2 = Badge.Sum(x => x.navy_projects);
            ViewBag.Total3 = Badge.Sum(x => x.other_projects);
            return View();
        }

        #endregion

    }
}