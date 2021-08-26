using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDPAdmin.Services.Master;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using MOD.Service;
using Newtonsoft.Json;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    [SessionExpire]
    [SessionExpireRefNo]
    public class WReportsController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        masterService mService = new masterService();
        GanttData ganttData = new GanttData();
        private readonly MODEntities _entities;
        string password = "p@SSword";

        public WReportsController()
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
            _entities = new MODEntities();
            EncriptServicesData();

            BruteForce bruteForce = new BruteForce();
            BruteForceAttackss.bcontroller = "WReports";
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "WReports")
                {
                    if (BruteForceAttackss.refreshcount == 0 && BruteForceAttackss.date == null)
                    {
                        BruteForceAttackss.date = System.DateTime.Now;
                        BruteForceAttackss.refreshcount = 1;
                    }
                    else
                    {
                        TimeSpan tt = System.DateTime.Now - BruteForceAttackss.date.Value;
                        if (tt.TotalSeconds <= 30 && BruteForceAttackss.refreshcount > 2)
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
                BruteForceAttackss.bcontroller = "WReports";
            }
        }
        // GET: WReports
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

        //        public string GetGraph1(string id)
        //        {
        //            string query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, q.TaskDescription, count(*) projects"+
        //"FROM acq_project_status_1 w, tbl_mst_Template q"+
        //"WHERE w.actual_end_date BETWEEN '2019-05-01' AND '2020-12-31' AND w.task_id = q.taskslno"+
        //"GROUP BY q.TaskSlno, q.TaskDescription";
        //            DataTable dt = return_datatable(query);
        //            string data = JsonConvert.SerializeObject(dt);
        //            return data;
        //        }

        public string GetTable1(string id)
        {
            IEnumerable<WReports1> Badge = null;
            string query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, Service_Lead_Service, " +
                "item_description, Convert(varchar,Date_of_Accord_of_AoN,103) as Date_of_Accord_of_AoN, Cost, Categorisation, IC_percentage, Trials_Required, " +
                "TaskDescription, ISNULL(completed_on,'') as completed_on, ISNULL(no_of_weeks,'') as no_of_weeks," +
                " dap_timeline, TaskSlno" +
                " FROM acq_project_status_timelines g" +
                " WHERE g.aon_id = '" + id + "'";
            DataTable dt = return_datatable(query);
            Badge = dt.AsEnumerable().Select(x => new WReports1
            {
                Pkey = x.Field<long>("Pkey"),
                Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                item_description = (Cipher.Decrypt(x.Field<string>("item_description"), password)).ToString(),
                Date_of_Accord_of_AoN = x.Field<string>("Date_of_Accord_of_AoN"),
                Cost = x.Field<string>("Cost"),
                Categorisation = x.Field<string>("Categorisation"),
                IC_percentage = x.Field<string>("IC_percentage"),
                Trials_Required = x.Field<string>("Trials_Required"),
                TaskDescription = x.Field<string>("TaskDescription"),
                completed_on = x.Field<DateTime>("completed_on"),
                no_of_weeks = x.Field<int>("no_of_weeks"),
                dap_timeline = x.Field<string>("dap_timeline"),
                TaskSlno = x.Field<decimal>("TaskSlno"),

            });
            string data = JsonConvert.SerializeObject(Badge.FirstOrDefault());
            return data;
        }

        public JsonResult GetTable(string id)
        {
            List<WReports> list = new List<WReports>();
            IEnumerable<WReports> Badge = null;
            string query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, Service_Lead_Service, " +
            " item_description, Date_of_Accord_of_AoN, Cost, Categorisation, IC_percentage, Trials_Required, TaskDescription, " +
            " ISNULL(completed_on,null) as completed_on, ISNULL(no_of_weeks,'') as no_of_weeks, dap_timeline, TaskSlno" +
            " FROM acq_project_status_timelines g" +
            " WHERE g.aon_id ='" + id + "'";
            DataTable dt = return_datatable(query);
            Badge = dt.AsEnumerable().Select(x => new WReports
            {
                Pkey = x.Field<long>("Pkey"),
                Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                item_description = (Cipher.Decrypt(x.Field<string>("item_description"), password)).ToString(),
                Date_of_Accord_of_AoN = x.Field<DateTime>("Date_of_Accord_of_AoN"),
                Cost = x.Field<string>("Cost"),
                Categorisation = x.Field<string>("Categorisation"),
                IC_percentage = x.Field<string>("IC_percentage"),
                Trials_Required = x.Field<string>("Trials_Required"),
                TaskDescription = x.Field<string>("TaskDescription"),
                completed_on = x.Field<Nullable<DateTime>>("completed_on"),
                no_of_weeks = x.Field<int>("no_of_weeks"),
                dap_timeline = x.Field<string>("dap_timeline"),
                TaskSlno = x.Field<decimal>("TaskSlno"),

            });
            foreach (WReports item in Badge)
            {
                WReports obj = new WReports
                {
                    Pkey = item.Pkey,
                    Service_Lead_Service = item.Service_Lead_Service,
                    item_description = item.item_description,
                    Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN,
                    Cost = item.Cost,
                    Categorisation = item.Categorisation,
                    IC_percentage = item.IC_percentage,
                    Trials_Required = item.Trials_Required,
                    TaskDescription = item.TaskDescription,
                    completed_on = item.completed_on,
                    no_of_weeks = item.no_of_weeks,
                    dap_timeline = item.dap_timeline,
                    TaskSlno = item.TaskSlno,
                };
                list.Add(obj);
            }

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        //[Route("WRNewReport")]
        public ActionResult NewReports(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {

            }
            else
            {
                try
                {
                    id = Cipher.Decrypt_Portal(id);
                }
                catch
                {
                }
                ViewBag.SelectedAon = id;
            }
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Case Wise Report Indicating Timelines At Various Stages And Current Status".ToLower())
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
            if (id == null || id == "")
            {
                ViewBag.id = "";
            }
            else
            {
                ViewBag.id = id;
            }
            var struid = "";
            if (Session["SectionID"] != null)
            {
                struid = mService.SectionID(Session["SectionID"].ToString());
            }

            IEnumerable<WReports> Badge1 = null;
            Badge1 = mService.GetReportGrid1();
            //ViewBag.Grid1 = Badge1;
            ViewBag.Grid2 = Badge1.FirstOrDefault();
            List<acq_project_master> list = new List<acq_project_master>();
            IEnumerable<acq_project_master> projectList = null;
            if (struid == "SuperAdmin")
            {
                projectList = _entities.acq_project_master.Where(x => x.DeletedBy == null).ToList();
            }
            else
            {
                projectList = _entities.acq_project_master.Where(x => x.Service_Lead_Service == struid && x.DeletedBy == null).ToList();
            }

            if (projectList != null)
            {
                foreach (var item in projectList)
                {
                    acq_project_master obj = new acq_project_master();
                    obj.aon_id = item.aon_id;
                    obj.item_description = Cipher.Decrypt(item.item_description, password);
                    list.Add(obj);
                }
            }
            projectList = list;

            // ViewBag.item_description = new SelectList(projectList, "aon_id", "item_description");
            ViewBag.item_description = list;
            return View();
        }

        [Route("WRChartReport")]
        public ActionResult ChartReport1(string StartDate, string EndDate, string Service_Lead_Service)
        {
            string Service_Lead_Service1 = null;
            if (Service_Lead_Service != null && Service_Lead_Service != "")
            {
                Service_Lead_Service1 = Cipher.Decrypt(Service_Lead_Service, "");
            }
            else
            {
                Service_Lead_Service1 = Service_Lead_Service;
            }
            var SectionID = Session["SectionID"];
            string wherecond = "";

            if ((int)SectionID == 3 || (int)SectionID == 6 || (int)SectionID == 9)
                wherecond = " and service_lead_service like 'Army'";

            if ((int)SectionID == 2 || (int)SectionID == 5 || (int)SectionID == 8)
                wherecond = " and service_lead_service like 'AirForce'";

            if ((int)SectionID == 4 || (int)SectionID == 7 || (int)SectionID == 10)
                wherecond = " and service_lead_service in( 'Navy', 'ICG')";



            var mStartDate = ""; var mEndDate = "";
            Session["Fromdate"] = mStartDate;
            Session["Todate"] = mEndDate;
            if (StartDate != null && EndDate != null)
            {
                var userDt = StartDate;
                DateTime dtByUser = DateTime.Parse(userDt).Date;
                mStartDate = dtByUser.ToString("yyyy-MM-dd");
                var userDt1 = EndDate;
                DateTime dtByUser1 = DateTime.Parse(userDt1).Date;
                mEndDate = dtByUser1.ToString("yyyy-MM-dd");
                Session["Fromdate"] = mStartDate;
                Session["Todate"] = mEndDate;

                List<DetailCharts> dataPoints1 = new List<DetailCharts>();
                IEnumerable<ReportMileStonebyMonth> Badge = null;
                if (Service_Lead_Service1 == "" || Service_Lead_Service1 == null)
                    Service_Lead_Service1 = "%";
                string query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, q.TaskDescription,cast(q.Taskslno as int) Taskslno, count(*) projects" +
                " FROM acq_project_status_1 w, tbl_mst_Template q, acq_project_master d" +
                 " WHERE w.actual_end_date BETWEEN '" + mStartDate + "' AND '" + mEndDate + "' AND w.task_id = q.taskslno  and w.project_id = d.Aon_id and d.service_lead_service like '" + Service_Lead_Service1 + "'" + wherecond +
                // " WHERE w.actual_end_date BETWEEN '" + mStartDate + "' AND '" + mEndDate + "' AND w.task_id = q.taskslno  and w.project_id = d.Aon_id and d.service_lead_service like ? " + wherecond +
                " GROUP BY q.TaskSlno, q.TaskDescription order by 3";
                DataTable dt = return_datatable(query);

                Badge = dt.AsEnumerable().Select(x => new ReportMileStonebyMonth
                {
                    Pkey = x.Field<long>("Pkey"),
                    TaskDescription = x.Field<string>("TaskDescription"),
                    projects = x.Field<int>("projects"),

                });
                foreach (ReportMileStonebyMonth item in Badge)
                {
                    dataPoints1.Add(new DetailCharts(Convert.ToString(item.TaskDescription), Convert.ToDouble(item.projects), Convert.ToDouble(item.projects)));
                }
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);
            }

            
            return View();
        }
        [HttpPost]
        public ActionResult ReportOnClick(string id, string Service_Lead_Service)
        {
            if (Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }
            else
            {
                Service_Lead_Service = Cipher.Decrypt(Service_Lead_Service, "");
            }
            IEnumerable<WReports> BadgeChart = null;
            //BadgeChart = mService.GetReportGridOnClickChart(id);
            //ViewBag.Grid1 = BadgeChart;
            string query = "";
            if (Session["Fromdate"].ToString() == null || Session["Fromdate"].ToString() == "")
            {
                query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, q.TaskSlno, q.TaskDescription, g.Service_Lead_Service, g.item_description, " +
                                "g.Date_of_Accord_of_AoN, g.Cost, g.Categorisation, g.IC_percentage," +
                " g.Trials_Required FROM acq_project_status_1 w, tbl_mst_Template q, acq_project_master g" +
                                //" WHERE w.project_id = g.aon_id AND q.TaskDescription = '" + id + "' and g.Service_Lead_Service like '" + Service_Lead_Service + "'  AND w.actual_end_date BETWEEN '" + Session["Fromdate"].ToString() + "' AND '" + Session["Todate"].ToString() + "' AND w.task_id = q.taskslno";

                                " WHERE w.project_id = g.aon_id AND q.TaskDescription = ? and g.Service_Lead_Service like ?   AND w.task_id = q.taskslno";

            }
            else
            {
                query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey, q.TaskSlno, q.TaskDescription, g.Service_Lead_Service, g.item_description, " +
                   "g.Date_of_Accord_of_AoN, g.Cost, g.Categorisation, g.IC_percentage," +
   " g.Trials_Required FROM acq_project_status_1 w, tbl_mst_Template q, acq_project_master g" +
                   //" WHERE w.project_id = g.aon_id AND q.TaskDescription = '" + id + "' and g.Service_Lead_Service like '" + Service_Lead_Service + "'  AND w.actual_end_date BETWEEN '" + Session["Fromdate"].ToString() + "' AND '" + Session["Todate"].ToString() + "' AND w.task_id = q.taskslno";

                   " WHERE w.project_id = g.aon_id AND q.TaskDescription = ? and g.Service_Lead_Service like ?  AND w.actual_end_date BETWEEN '" + Session["Fromdate"].ToString() + "' AND '" + Session["Todate"].ToString() + "' AND w.task_id = q.taskslno";
            }
            // DataTable dt = return_datatable(query);

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@id", OleDbType.VarChar, 500);
                cmd.Parameters["@id"].Value = id;

                cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
                cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service;

                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }

            BadgeChart = dt.AsEnumerable().Select(x => new WReports
            {
                Pkey = x.Field<long>("Pkey"),
                Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                item_description = (Cipher.Decrypt(x.Field<string>("item_description"), password)).ToString(),
                Date_of_Accord_of_AoN = x.Field<DateTime>("Date_of_Accord_of_AoN"),
                Cost = x.Field<string>("Cost"),
                Categorisation = x.Field<string>("Categorisation"),
                IC_percentage = x.Field<string>("IC_percentage"),
                Trials_Required = x.Field<string>("Trials_Required"),
                TaskDescription = x.Field<string>("TaskDescription"),
                TaskSlno = x.Field<decimal>("TaskSlno"),

            });
            ViewBag.Grid1 = BadgeChart;
            //return View("ChartReport1");
            return Json(new { data = ViewBag.Grid1 }, JsonRequestBehavior.AllowGet);
        }


    }
}