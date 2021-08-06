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
using Ganss.XSS;
using static MOD.MvcApplication;
using MOD.Service;
using System.Threading.Tasks;
using System.Configuration;

namespace MOD.Controllers
{
    // [Authorize]
    public class HomeController : Controller
    {
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        private readonly MODEntities _entities;
        public HomeController()
        {
            _entities = new MODEntities();
            BindEncriptData();
            EncriptServicesData();
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
                if (BruteForceAttackss.bcontroller == "Home")
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
                                    System.Web.HttpContext.Current.Response.Redirect("http://localhost:51994/");
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
                BruteForceAttackss.bcontroller = "Home";
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

        masterService mService = new masterService();
        string password = "p@SSword";
        GanttData ganttData = new GanttData();
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        private static string WebPortalUrlLogout = ConfigurationManager.AppSettings["WebPortalUrlLogout"].ToString();
        public ActionResult sign(string mu)
        {
            if (mu != null)
            {
                Session["UserID"] = mu;

            }
            return RedirectToAction("Index");
        }
        //[SessionExpire]
        [Route("User")]
        public ActionResult Index()
        {
            String id = "";
            if (Session["UserID"] != null)
            {
                id = Encryption.Decryptl(Session["UserID"].ToString());
              
                if (id != "")
                {

                    using (var _context = new MODEntities())
                    {
                        var isValid = _context.tbl_tbl_User.Where(x => x.InternalEmailID == id).FirstOrDefault();
                        if (isValid != null)
                        {
                            var IsLogout = _context.acq_audit_trail.Where(s => s.UserEmail == id).OrderByDescending(s => s.LogId).FirstOrDefault();
                            if(IsLogout!=null)
                            {
                                if(IsLogout.Action!= "Logout")
                                {
                                    if(IsLogout.IPAddress==isValid.IPAddress)
                                    {
                                        // int UserId = isValid.UserId;
                                        FormsAuthentication.SetAuthCookie(isValid.InternalEmailID, false);
                                        Session["UserID"] = isValid.UserId;
                                        Session["UserName"] = isValid.UserName;
                                        Session["SectionID"] = isValid.SectionID;
                                        Session["WebPortalUrl"] = WebPortalUrl;
                                        Session["EmailID"] = isValid.InternalEmailID;

                                        List<tbl_Master_Role> list = _context.tbl_Master_Role.Where(x => x.UserID == isValid.UserId).ToList();
                                        Session["RoleList"] = list;
                                        return View();
                                    }
                                    else
                                    {
                                        return Redirect(WebPortalUrlLogout);
                                    }
                                }
                                else
                                {
                                    return Redirect(WebPortalUrlLogout);
                                }
                                
                            }
                            else
                            {
                                return RedirectToAction("Login", "Account");
                            }
                           
                        }
                        else
                        {
                            TempData["Message"] = "Login Failed.User Name or Password Doesn't Exist.";
                            return Redirect(WebPortalUrlLogout);
                        }
                    }
                }
                else
                {
                    return Redirect(WebPortalUrlLogout);
                }
            }
            else
            {
                return Redirect(WebPortalUrlLogout);
            }
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HGantt")]
        public ActionResult Gantt(int Id)
        {
            ViewBag.DataSource = ganttData.ProjectNewData(Id);
            return View();
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HBaseline")]
        public ActionResult Baseline(int Id)
        {
            ViewBag.DataSource = ganttData.BaselineData(Id);
            return View();
        }

        [HttpGet]
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HProjectDetail")]
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
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HCasesTable")]
        public JsonResult GetCasesTable(decimal id, string Categorisation, string Service_Lead_Service, DateTime myDate)
        {
            List<CaseViewModel> list = new List<CaseViewModel>();
            IEnumerable<CaseViewModel> Badge = null;
            Badge = mService.GetCasesTable(id, Categorisation, Service_Lead_Service, myDate);
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
        [Route("HDashboard")]
        [SessionExpire]
        [SessionExpireRefNo]
        public ActionResult Dashboard(string Categorisation, string Service_Lead_Service)
        {
            string Categorisation1 = null;
            string Service = null;
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
                Service = Cipher.Decrypt(Service_Lead_Service, "");
            }
            else
            {
                Service = Service_Lead_Service;
            }

            if (Session["UserID"] != null)
            {
                int id = Convert.ToInt32(1);
                List<DashboardViewModel> list = new List<DashboardViewModel>();
                IEnumerable<DashboardViewModel> Badge = null;
                Badge = mService.GetDashboardDetails(sanitizer.Sanitize(Categorisation1), sanitizer.Sanitize(Service));
                ViewBag.Dashboard = Badge;
                //ViewBag.Categorisation = new SelectList(_entities.acq_project_master, "Categorisation", "Categorisation");
                //ViewBag.Service_Lead_Service = new SelectList(_entities.acq_project_master, "Service_Lead_Service", "Service_Lead_Service");        
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HDasboardDetails")]
        public JsonResult GetProjectDasboardDetails(string id, string mTaskSlno, string id5, string Categorisation, string Service_Lead_Service)
        {
            List<WaliTest> list = new List<WaliTest>();
            IEnumerable<WaliTest> Badge = null;
            if (ModelState.IsValid)
            {
                try
                {
                    Badge = mService.GetProjectDasboardDetails(id, mTaskSlno, id5, Categorisation, Service_Lead_Service);
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
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HMonthWiseChart")]
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
        [SessionExpire]
        [SessionExpireRefNo]
        [Route("HDetailChart")]
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