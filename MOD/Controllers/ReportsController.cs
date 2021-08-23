using DDPAdmin.Services.Master;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using MOD.Service;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using static MOD.MvcApplication;

namespace MOD.Controllers
{

    [SessionExpire]
    [SessionExpireRefNo]
    public class ReportsController : Controller
    {
        MODEntities _entities = new MODEntities();
        masterService mService = new masterService();
        GanttData ganttData = new GanttData();
        // private static string WebAPIUrl = ConfigurationManager.AppSettings["APIUrl"].ToString();
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        string password = "p@SSword";

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

        public ReportsController()
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
            EncriptStageData();
            BruteForce bruteForce = new BruteForce();
            BruteForceAttackss.bcontroller = "Reports";
            // BruteForceAttackss.bcontroller = "User";
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "Reports")
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
                BruteForceAttackss.bcontroller = "Reports";
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
        public void EncriptStageData()
        {
            List<Categorisation> categorisations = new List<Categorisation>
            {
                new Categorisation { Text = "RFP",  Value = "RFP" },
                new Categorisation { Text = "TEC",  Value = "TEC" },
                new Categorisation { Text = "FET",  Value = "FET" },
                new Categorisation { Text = "GS Eval",  Value = "GS Eval" },
                new Categorisation { Text = "CNC",  Value = "CNC" },
                new Categorisation { Text = "CFA Approval",  Value = "CFA Approval" }
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
            ViewBag.StageData = Catdata;
        }

        [Route("ServiceWiseReport")]
        public ActionResult ServiceWiseReport(string Service_Lead_Service, string pending_in_stage, string system_case)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Service-Wise Report on the Pending Cases at various Stages".ToLower())
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
            string pending_in_stage1 = null;
            string Service_Lead_Service1 = null;
            if (Service_Lead_Service != null & Service_Lead_Service != "")
            {
                Service_Lead_Service1 = Cipher.Decrypt(Service_Lead_Service, "");
            }
            else
            {
                Service_Lead_Service1 = Service_Lead_Service;
            }

            if (pending_in_stage != null & pending_in_stage != "")
            {
                pending_in_stage1 = Cipher.Decrypt(pending_in_stage, "");
            }
            else
            {
                pending_in_stage1 = pending_in_stage;
            }
            var struid = "";
            if (Session["SectionID"] != null)
            {
                struid = mService.SectionID(Session["SectionID"].ToString());
            }
            List<MODListViewModel> list1 = new List<MODListViewModel>();

            if (struid == "SuperAdmin")
            {
                var _serviceWiseReport = _entities.acq_project_status_pendingstage.ToList();
                if (_serviceWiseReport != null)
                {
                    ViewBag.service1 = Service_Lead_Service1;
                    try
                    {


                        foreach (var item in _serviceWiseReport)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Categorisation = item.Categorisation;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Trials_Required = item.Trials_Required;
                            obj.pending_in_stage = item.pending_in_stage;
                            obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                            list1.Add(obj);
                        }
                    }
                    catch (Exception e)
                    {
                        Response.Write("Step 1 : " + e.Message);
                    }
                }

            }
            if (struid == "Navy" || struid == "ICG")
            {
                var _serviceWiseReport = _entities.acq_project_status_pendingstage.ToList();
                if (_serviceWiseReport != null)
                {
                    ViewBag.service1 = Service_Lead_Service1;

                    try
                    {
                        foreach (var item in _serviceWiseReport)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Categorisation = item.Categorisation;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Trials_Required = item.Trials_Required;
                            obj.pending_in_stage = item.pending_in_stage;
                            obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                            list1.Add(obj);
                        }
                    }
                    catch (Exception e)
                    {
                        Response.Write("Step 2 : " + e.Message);
                    }

                }

            }
            else
            {

                var _serviceWiseReport1 = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service == struid).ToList();
                if (_serviceWiseReport1 != null)
                {
                    try
                    {
                        ViewBag.service1 = Service_Lead_Service1;
                        foreach (var item in _serviceWiseReport1)
                        {
                            MODListViewModel obj = new MODListViewModel();
                            obj.Service_Lead_Service = item.Service_Lead_Service;
                            obj.item_description = Cipher.Decrypt(item.item_description, password);
                            obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                            obj.Cost = item.Cost;
                            obj.Categorisation = item.Categorisation;
                            obj.IC_percentage = item.IC_percentage;
                            obj.Trials_Required = item.Trials_Required;
                            obj.pending_in_stage = item.pending_in_stage;
                            obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                            list1.Add(obj);
                        }
                    }
                    catch (Exception e)
                    {
                        Response.Write("Step 2 : " + e.Message);
                    }
                }
            }
            if (struid == "Navy" || struid == "ICG")
            {
                var Stagelist = list1.Select(m => m.pending_in_stage).Distinct();
                List<Categorisation> Catdata = new List<Categorisation>();
                try
                {
                    foreach (var item in Stagelist)
                    {
                        Categorisation cat1 = new Categorisation()
                        {
                            Text = item,
                            Value = Cipher.Encrypt(item, "")
                        };
                        Catdata.Add(cat1);
                    };
                }
                catch (Exception e)
                {
                    Response.Write("Step 2 : " + e.Message);
                }
                ViewBag.stage = Catdata;
                string[] viewdata = { "Navy", "ICG" };
                ViewBag.Service = viewdata;
            }
            else
            {
                var Stagelist = list1.Select(m => m.pending_in_stage).Distinct();
                List<Categorisation> Catdata = new List<Categorisation>();
                foreach (var item in Stagelist)
                {
                    Categorisation cat1 = new Categorisation()
                    {
                        Text = item,
                        Value = Cipher.Encrypt(item, "")
                    };
                    Catdata.Add(cat1);
                };
                ViewBag.stage = Catdata;
                // ViewBag.stage = list1.Select(m => m.pending_in_stage).Distinct();
                ViewBag.Service = list1.Select(m => m.Service_Lead_Service).Distinct();
            }
            if (Service_Lead_Service == null && pending_in_stage == null)
            {
                ViewBag.stage1 = "";
                ViewBag.Service1 = "";
            }
            else
            {
                ViewBag.stage1 = pending_in_stage1;
                ViewBag.Service1 = Service_Lead_Service1;
            }

            MODListViewModel model = new MODListViewModel();
            List<MODListViewModel> list = new List<MODListViewModel>();
            if (Service_Lead_Service == null || pending_in_stage == null)
            {

                if (struid == "SuperAdmin")
                {
                    var _serviceWiseReport = _entities.acq_project_status_pendingstage.ToList();
                    if (_serviceWiseReport != null)
                    {
                        ViewBag.service1 = Service_Lead_Service1;
                        try
                        {
                            foreach (var item in _serviceWiseReport)
                            {
                                MODListViewModel obj = new MODListViewModel();
                                obj.Service_Lead_Service = item.Service_Lead_Service;
                                obj.item_description = Cipher.Decrypt(item.item_description, password);
                                obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                obj.Cost = item.Cost;
                                obj.Categorisation = item.Categorisation;
                                obj.IC_percentage = item.IC_percentage;
                                obj.Trials_Required = item.Trials_Required;
                                obj.pending_in_stage = item.pending_in_stage;
                                obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                list.Add(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            Response.Write("Step 2 : " + e.Message);
                        }
                    }

                }
                else
                {

                    var _serviceWiseReport1 = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service == struid).ToList();
                    if (_serviceWiseReport1 != null)
                    {
                        @ViewBag.service1 = Service_Lead_Service1;
                        try
                        {
                            foreach (var item in _serviceWiseReport1)
                            {

                                MODListViewModel obj = new MODListViewModel();
                                obj.Service_Lead_Service = item.Service_Lead_Service;
                                obj.item_description = Cipher.Decrypt(item.item_description, password);
                                obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                obj.Cost = item.Cost;
                                obj.Categorisation = item.Categorisation;
                                obj.IC_percentage = item.IC_percentage;
                                obj.Trials_Required = item.Trials_Required;
                                obj.pending_in_stage = item.pending_in_stage;
                                obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                list.Add(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            Response.Write("Step 2 : " + e.Message);
                        }
                    }
                }

            }
            else
            {
                if (!string.IsNullOrEmpty(Service_Lead_Service) && !string.IsNullOrEmpty(pending_in_stage))
                {
                    
                    @ViewBag.service1 = Service_Lead_Service1;
                    ViewBag.category = pending_in_stage1;
                    if (Service_Lead_Service1 == "Navy" || Service_Lead_Service1 == "ICG")
                    {
                        var result = _entities.acq_project_status_pendingstage.Where(x => (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG" || x.System_case == "Y") && x.pending_in_stage == pending_in_stage1).ToList();
                        if (result != null)
                        {
                            try
                            {
                                foreach (var item in result)
                                {
                                    MODListViewModel obj = new MODListViewModel();
                                    obj.Service_Lead_Service = item.Service_Lead_Service;
                                    obj.item_description = Cipher.Decrypt(item.item_description, password);
                                    obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                    obj.Cost = item.Cost;
                                    obj.System_case = item.System_case;
                                    obj.Categorisation = item.Categorisation;
                                    obj.IC_percentage = item.IC_percentage;
                                    obj.Trials_Required = item.Trials_Required;
                                    obj.pending_in_stage = item.pending_in_stage;
                                    obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                    list.Add(obj);
                                }
                            }
                            catch (Exception e)
                            {
                                Response.Write("Step 2 : " + e.Message);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(Service_Lead_Service) && !string.IsNullOrEmpty(pending_in_stage))
                {
                    @ViewBag.service1 = Service_Lead_Service1;
                    ViewBag.category = pending_in_stage1;
                    var result = _entities.acq_project_status_pendingstage.Where(x => (x.Service_Lead_Service.Contains(Service_Lead_Service1)) && (x.System_case == "N" || x.System_case == null) && x.pending_in_stage == pending_in_stage1).ToList();
                    if (result != null)
                    {
                        try
                        {
                            foreach (var item in result)
                            {
                                MODListViewModel obj = new MODListViewModel();
                                obj.Service_Lead_Service = item.Service_Lead_Service;
                                obj.item_description = Cipher.Decrypt(item.item_description, password);
                                obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                obj.Cost = item.Cost;
                                obj.Categorisation = item.Categorisation;
                                obj.IC_percentage = item.IC_percentage;
                                obj.Trials_Required = item.Trials_Required;
                                obj.pending_in_stage = item.pending_in_stage;
                                obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                list.Add(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            Response.Write("Step 2 : " + e.Message);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(Service_Lead_Service))
                {
                    @ViewBag.service1 = Service_Lead_Service1;
                    var result = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList();
                    if (result != null)
                    {
                        try
                        {
                            foreach (var item in result)
                            {
                                MODListViewModel obj = new MODListViewModel();
                                obj.Service_Lead_Service = item.Service_Lead_Service;
                                obj.item_description = Cipher.Decrypt(item.item_description, password);
                                obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                obj.Cost = item.Cost;
                                obj.Categorisation = item.Categorisation;
                                obj.IC_percentage = item.IC_percentage;
                                obj.Trials_Required = item.Trials_Required;
                                obj.pending_in_stage = item.pending_in_stage;
                                obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                list.Add(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            Response.Write("Step 2 : " + e.Message);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(pending_in_stage))
                {
                    ViewBag.category = pending_in_stage1;
                    var result = _entities.acq_project_status_pendingstage.Where(x => x.pending_in_stage == pending_in_stage1).ToList();
                    if (result != null)
                    {
                        try
                        {
                            foreach (var item in result)
                            {
                                MODListViewModel obj = new MODListViewModel();
                                obj.Service_Lead_Service = item.Service_Lead_Service;
                                obj.item_description = Cipher.Decrypt(item.item_description, password);
                                obj.Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN;
                                obj.Cost = item.Cost;
                                obj.Categorisation = item.Categorisation;
                                obj.IC_percentage = item.IC_percentage;
                                obj.Trials_Required = item.Trials_Required;
                                obj.pending_in_stage = item.pending_in_stage;
                                obj.date_of_entering_this_stage = item.date_of_entering_this_stage;
                                list.Add(obj);
                            }
                        }
                        catch (Exception e)
                        {
                            Response.Write("Step 2 : " + e.Message);
                        }
                    }
                }
            }
            try
            {

                model.AonList = list;
                string query = "";
                string msystem_case = "";
                if (Service_Lead_Service == null || Service_Lead_Service == "")
                    Service_Lead_Service1 = "%";

                if (pending_in_stage == null || pending_in_stage == "")
                    pending_in_stage1 = "%";
                if (msystem_case == null || msystem_case == "")
                    msystem_case = "%";


                if (system_case != null && system_case != "")
                    msystem_case = " and b.System_case=" + "'" + system_case + "'";

                query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey,b.TaskSlno, b.pending_in_stage, count(*) n_projects, " +
                    " isnull(sum(cast(b.cost AS decimal)),'0') total_aon_cost " +
    //" FROM acq_project_status_pendingstage b where b.Service_Lead_Service like '" + Service_Lead_Service1 + "' and b.pending_in_stage like '" + pending_in_stage1 + "' and  isnull(b.System_case,'') like '" + msystem_case + "'" +
    " FROM acq_project_status_pendingstage b where b.Service_Lead_Service like ? and b.pending_in_stage like ? and  isnull(b.System_case,'') like ?" +
    " GROUP BY b.TaskSlno, b.pending_in_stage order by TaskSlno";
                List<DetailCharts> dataPoints = new List<DetailCharts>();
                IEnumerable<Service_WiseReport> Badge = null;


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
                    cmd.Parameters.Add("@pending_in_stage", OleDbType.VarChar, 500);
                    cmd.Parameters["@pending_in_stage"].Value = pending_in_stage1;
                    cmd.Parameters.Add("@msystem_case", OleDbType.VarChar, 500);
                    cmd.Parameters["@msystem_case"].Value = msystem_case;
                    cmd.Connection = conn;
                    adap.SelectCommand = cmd;
                    adap.Fill(dt);
                    conn.Close();
                }



                // DataTable dt = return_datatable(query);
                int a = dt.Rows.Count;
                Badge = dt.AsEnumerable().Select(x => new Service_WiseReport
                {
                    Pkey = x.Field<long>("Pkey"),
                    pending_in_stage = x.Field<string>("pending_in_stage"),
                    n_projects = x.Field<int>("n_projects"),
                    total_aon_cost = x.Field<decimal>("total_aon_cost")

                });
                foreach (Service_WiseReport item in Badge)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(item.pending_in_stage), Convert.ToDouble(item.n_projects), Convert.ToDouble(item.total_aon_cost)));
                }

                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            }
            catch (Exception e)
            {
                Response.Write("Step 2 : " + e.Message);
            }
            return View(model);
        }

        [Route("RServiceWiseGraph")]
        public ActionResult ServiceWiseGraph()
        {
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            dataPoints = mService.Service_WiseReport();
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }
        [Route("RServiceWiseGraphAll")]
        public ActionResult ServiceWiseGraphAll()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Stage wise Cycle time analysis (All services)".ToLower())
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
            string query = "";
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            List<DetailCharts> dataPoints2 = new List<DetailCharts>();
            List<DetailCharts> dataPoints3 = new List<DetailCharts>();
            List<Grid3> Mmodel1 = new List<Grid3>();



            query = "SELECT distinct j.stage,isnull(j.stage_name,'0')stage_name,isnull((case stage when 2 then 6 when 4 then 10 when 6 then 24 when 10 then 6 when 12 then 26 when 13 then 18 end),'0') dap_timeline," +
" ISNULL((select avg(p.no_of_weeks) from acq_project_status_avgdelay p where p.stage = j.stage and p.Service_Lead_Service = 'AirForce'),0)no_of_weeks_airforce," +
" ISNULL((select avg(p.no_of_weeks) from acq_project_status_avgdelay p where p.stage = j.stage and p.Service_Lead_Service = 'Army'),0)no_of_weeks_army," +
" ISNULL((select avg(p.no_of_weeks) from acq_project_status_avgdelay p where p.stage = j.stage and p.Service_Lead_Service = 'Navy'),0)no_of_weeks_navy" +
     " FROM acq_project_status_avgdelay j";

            DataTable dt = return_datatable(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Grid3 list = new Grid3
                    {
                        stage_name = item["stage_name"].ToString(),
                        dap_timeline = item["dap_timeline"].ToString(),
                        no_of_weeks_airforce = Convert.ToInt32(item["no_of_weeks_airforce"].ToString()),
                        no_of_weeks_army = Convert.ToInt32(item["no_of_weeks_army"].ToString()),
                        no_of_weeks_navy = Convert.ToInt32(item["no_of_weeks_navy"].ToString()),

                    };
                    Mmodel1.Add(list);
                }
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["no_of_weeks_airforce"].ToString()), Convert.ToDouble(row["no_of_weeks_airforce"].ToString())));
                }
                ViewBag.DataPointsA = JsonConvert.SerializeObject(dataPoints);
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints2.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["no_of_weeks_army"].ToString()), Convert.ToDouble(row["no_of_weeks_army"].ToString())));
                }
                ViewBag.DataPointsAr = JsonConvert.SerializeObject(dataPoints2);
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints3.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["no_of_weeks_navy"].ToString()), Convert.ToDouble(row["no_of_weeks_navy"].ToString())));
                }
                ViewBag.DataPointsNa = JsonConvert.SerializeObject(dataPoints3);

                foreach (DataRow row in dt.Rows)
                {

                    dataPoints1.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["dap_timeline"].ToString()), Convert.ToDouble(row["dap_timeline"].ToString())));
                }
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

                ViewBag.Grid1 = Mmodel1;
            }
            return View();
        }

        [Route("RParticularstage")]
        public ActionResult particularstage(string stage_name, string Service_Lead_Service, string Categorisation)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Trend analysis for a particular stage".ToLower())
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
            string Categorisation1 = null;
            string Service_Lead_Service1 = null;
            string stage_name1 = null;
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
            if (stage_name != null & stage_name != "")
            {
                stage_name1 = Cipher.Decrypt(stage_name, "");
            }
            else
            {
                stage_name1 = stage_name;
            }
            string query = "";
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            List<Grid4> Mmodel1 = new List<Grid4>();


            if (stage_name == null || stage_name == "")
            {
                stage_name = "%";
            }
            if (Service_Lead_Service1 == null || Service_Lead_Service1 == "")
            {
                Service_Lead_Service1 = "%";
            }
            if (Categorisation1 == null || Categorisation1 == "")
            {
                Categorisation1 = "%";
            }

            query = "select f.*,a.item_description,g.completed_on from acq_project_status_avgdelay f," +
                " acq_project_status_timelines g , acq_project_master a where a.aon_id=f.aon_id and f.aon_id=g.aon_id and " +
            // " f.stage=g.TaskSlno and f.stage_name=? and f.Service_Lead_Service like ? and f.Categorisation like '" + Categorisation1 + "' order by g.completed_on";
            " f.stage=g.TaskSlno and f.stage_name='" + stage_name1 + "' and f.Service_Lead_Service like '" + Service_Lead_Service1 + "' and f.Categorisation like '" + Categorisation1 + "' order by g.completed_on";

            DataTable dt = return_datatable(query);


            //DataTable dt = new DataTable();
            //using (OleDbConnection conn = masterService.DB())
            //{
            //    OleDbDataAdapter adap = new OleDbDataAdapter();
            //    OleDbCommand cmd = new OleDbCommand();
            //    cmd.CommandText = query;
            //    cmd.CommandType = CommandType.Text;
            //    cmd.Parameters.Add("@stage_name", OleDbType.VarChar, 500);
            //    cmd.Parameters["@stage_name"].Value = stage_name1;                
            //    cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
            //    cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service1;
            //    //cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
            //    //cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service1;

            //    //cmd.Parameters.Add("@Categorisation", OleDbType.VarChar, 500);
            //    //cmd.Parameters["@Categorisation"].Value = Categorisation1;
            //    cmd.Connection = conn;
            //    adap.SelectCommand = cmd;
            //    // cmd.ExecuteNonQuery();
            //    adap.Fill(dt);
            //    conn.Close();
            //}


            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Grid4 list = new Grid4
                    {
                        no_of_weeks = Convert.ToInt32(item["no_of_weeks"].ToString()),
                        stage_name = item["stage_name"].ToString(),
                        item_description = Cipher.Decrypt(item["item_description"].ToString(), password),

                    };
                    Mmodel1.Add(list);
                }
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(Cipher.Decrypt(row["item_description"].ToString(), password)), Convert.ToDouble(row["no_of_weeks"].ToString()), Convert.ToDouble(row["no_of_weeks"].ToString())));
                }
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);



                ViewBag.Grid1 = Mmodel1;
            }
            return View();
        }
        [Route("RStageWiseDelay")]
        public ActionResult StageWiseDelay(string Categorisation, string Service_Lead_Service, string stage)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Stage wise cycle time analysis  (TM)".ToLower())
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

            string query = "";
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            List<Grid1> Mmodel1 = new List<Grid1>();

            if (Categorisation1 == null || Categorisation1 == "")
            {
                Categorisation1 = "%";
            }

            if (Service_Lead_Service1 == null || Service_Lead_Service1 == "")
            {
                Service_Lead_Service1 = "%";
            }

            if (stage == null || stage == "")
            {
                stage = "'0.0'";
            }
            else if (stage == "TM")
            {
                stage = "'12','13'";

            }
            else if (stage == "AM")
            {
                stage = "'2','4','6','10'";
            }

            query = "select d.stage,d.stage_name,avg(d.no_of_weeks)no_of_weeks,isnull((case stage when 2 then " +
                " 6 when 4 then 10 when 6 then 24 when 10 then 6 when 12 then 26 when 13 then 18 end),'0') dap_timeline from acq_project_status_avgdelay d " +
            //"  where d.Service_Lead_Service like '" + Service_Lead_Service1 + "' and d.Categorisation like '" + Categorisation1 + "' and d.Categorisation not in " +
            //"('Design & Development','Make-I','Make-II','Make-III') and d.stage not in (" + stage + ")  group by d.stage,d.stage_name order by 1";
             "  where d.Service_Lead_Service like ? and d.Categorisation like ? and d.Categorisation not in " +
            "('Design & Development','Make-I','Make-II','Make-III') and d.stage not in ( " + stage + " )  group by d.stage,d.stage_name order by 1";

            //DataTable dt = return_datatable(query);

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
                cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service1;
                cmd.Parameters.Add("@Categorisation", OleDbType.VarChar, 500);
                cmd.Parameters["@Categorisation"].Value = Categorisation1;

                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }





            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Grid1 list = new Grid1
                    {
                        avg_delay_days = Convert.ToInt32(item["no_of_weeks"].ToString()),
                        taskDescription = item["stage_name"].ToString(),
                        dap_timeline = item["dap_timeline"].ToString(),

                    };
                    Mmodel1.Add(list);
                }
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["no_of_weeks"].ToString()), Convert.ToDouble(row["no_of_weeks"].ToString())));
                }
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                foreach (DataRow row in dt.Rows)
                {

                    dataPoints1.Add(new DetailCharts(Convert.ToString(row["stage_name"].ToString()), Convert.ToDouble(row["dap_timeline"].ToString()), Convert.ToDouble(row["dap_timeline"].ToString())));
                }
                ViewBag.DataPoints1 = JsonConvert.SerializeObject(dataPoints1);

                ViewBag.Grid1 = Mmodel1;
            }
            return View();
        }
        [Route("RReport")]
        public ActionResult ReportOnClick(string stage, string Service_Lead_Service, string Categorisation)
        {
            IEnumerable<StageWiseDelayReport_N> BadgeChart = null;
            if (Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }

            if (Categorisation == null || Categorisation == "")
            {
                Categorisation = "%";
            }

            string query = "select d.*,a.item_description from acq_project_status_avgdelay d,acq_project_master a where " +
               // "d.Service_Lead_Service like '" + Service_Lead_Service + "' and d.Categorisation like '" + Categorisation + "' " +
               "d.Service_Lead_Service like ? and d.Categorisation like ? " +
                "and d.Categorisation not in " +
                // "('Design & Development','Make-I','Make-II','Make-III') and d.stage_name like '" + stage + "' and d.aon_id=a.aon_id order by " +
                "('Design & Development','Make-I','Make-II','Make-III') and d.stage_name like '" + stage + "' and d.aon_id=a.aon_id order by " +
                "d.stage_name,d.no_of_weeks desc";
            // DataTable dt = return_datatable(query);

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;

                // cmd.Parameters.AddWithValue("@Id", CatID);

                cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
                cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service;

                cmd.Parameters.Add("@Categorisation", OleDbType.VarChar, 500);
                cmd.Parameters["@Categorisation"].Value = Categorisation;

                //cmd.Parameters.Add("@stage", OleDbType.VarChar, 500);
                //cmd.Parameters["@stage"].Value = stage;
                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }



            BadgeChart = dt.AsEnumerable().Select(x => new StageWiseDelayReport_N
            {
                Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                item_description = Cipher.Decrypt(x.Field<string>("item_description"), password),
                Categorisation = x.Field<string>("Categorisation"),
                stage = x.Field<decimal>("stage"),
                stage_name = x.Field<string>("stage_name"),
                no_of_weeks = x.Field<int>("no_of_weeks"),
                aon_id = Cipher.Encrypt_Portal(x.Field<int>("aon_id").ToString()),
            });
            ViewBag.Grid1 = BadgeChart;
            return View();
            //return Json(new { data = ViewBag.Grid1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VendcatwiseReport(string Service_Lead_Service)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Vendor Category Wise Report".ToLower())
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
            if (Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }
            else
            {
                Service_Lead_Service = Cipher.Decrypt(Service_Lead_Service, "");
            }
            List<CatWiseList> Catdata = new List<CatWiseList>();
            var CatDatalist = _entities.prc_Getvendcatwise_report();
            foreach (var item in CatDatalist)
            {
                string TaskDescription = null;
                if (item.stage.ToString() == "1")
                {
                    TaskDescription = "RFP";
                }
                else if (item.stage.ToString() == "2")
                {
                    TaskDescription = "Bid Submission";
                }
                else if (item.stage.ToString() == "3")
                {
                    TaskDescription = "TFC";
                }
                else if (item.stage.ToString() == "4")
                {
                    TaskDescription = "FET";
                }
                else
                { TaskDescription = "CNC"; }
                CatWiseList cat1 = new CatWiseList()
                {
                    stage = Cipher.Encrypt(item.stage.ToString(), ""),
                    TaskDescription = TaskDescription,
                    Foreign_OEM = Convert.ToInt32(item.Foreign_OEM.ToString() == "" || item.Foreign_OEM.ToString() == null ? "0" : item.Foreign_OEM.ToString()),
                    PSU = Convert.ToInt32(item.PSU.ToString() == "" || item.PSU.ToString() == null ? "0" : item.PSU.ToString()),
                    //Foreign_OEM1 == Convert.ToInt32(item.Foreign_OEM == "" || item.Foreign_OEM == null ? "0" : item.Foreign_OEM),
                    Indian_Private_MSME = Convert.ToInt32(item.Indian_Private_MSME.ToString() == "" || item.Indian_Private_MSME.ToString() == null ? "0" : item.Indian_Private_MSME.ToString()),
                    Indian_Private__Non_MSME = Convert.ToInt32(item.Indian_Private__Non_MSME.ToString() == "" || item.Indian_Private__Non_MSME.ToString() == null ? "0" : item.Indian_Private_MSME.ToString()),
                    Other = item.Other.ToString() == "" || item.Other.ToString() == null ? "0" : item.Other.ToString()
                };
                Catdata.Add(cat1);
            };
            ViewBag.catData = Catdata;
            char b = '"';
            string query = "select * from (select isnull(g.VendorCategoryName,'Undecided') as vendorcat,sum(cast(t.Cost as numeric)) as total_Cost from (select * from acq_project_master  where DeletedBy is null)t " +
  " left outer join tbl_tblVendor v on t.VendorsIDs=v.vendorid left outer join tbl_tblVendorCategory g on v.VendorCategory=g.VendorCategoryID where t.Service_Lead_Service like ? " +
  "group by isnull(g.VendorCategoryName,'Undecided')) m PIVOT(sum(total_Cost)  FOR vendorcat IN(" + b + "PSU" + b + "," + b + "Indian Private-MSME" + b + ", " + b + "Foreign OEM" + b + "," + b + "Indian Private-Non MSME" + b + "," + b + "Other" + b + " ," + b + "Undecided" + b + ")) AS PivotTable";

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {

                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                // cmd.Parameters.AddWithValue("@Id", CatID);
                cmd.Parameters.Add("@Id", OleDbType.VarChar, 500);
                cmd.Parameters["@Id"].Value = Service_Lead_Service;
                cmd.Connection = conn;
                //conn.Open();
                // OleDbDataReader reader = cmd.ExecuteReader();                
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }



            List<CatWiseListUnion> Catbreakupdata = new List<CatWiseListUnion>();
            foreach (DataRow dr in dt.Rows)
            {
                CatWiseListUnion cat1 = new CatWiseListUnion()
                {
                    stage = Cipher.Encrypt("0", ""),
                    TaskDescription = "AON Accord",
                    Foreign_OEM = Convert.ToInt32(dr["Foreign OEM"].ToString() == "" || dr["Foreign OEM"].ToString() == null ? "0" : dr["Foreign OEM"].ToString()),
                    PSU = Convert.ToInt32(dr["PSU"].ToString() == "" || dr["PSU"].ToString() == null ? "0" : dr["PSU"].ToString()),
                    //Foreign_OEM1 == Convert.ToInt32(item.Foreign_OEM == "" || item.Foreign_OEM == null ? "0" : item.Foreign_OEM),
                    Indian_Private_MSME = Convert.ToInt32(dr["Indian Private-MSME"].ToString() == "" || dr["Indian Private-MSME"].ToString() == null ? "0" : dr["Indian Private-MSME"].ToString()),
                    Indian_Private__Non_MSME = Convert.ToInt32(dr["Indian Private-Non MSME"].ToString() == "" || dr["Indian Private-Non MSME"].ToString() == null ? "0" : dr["Indian Private-Non MSME"].ToString()),
                    Other = Convert.ToInt32(dr["Other"].ToString() == "" || dr["Other"].ToString() == null ? "0" : dr["Other"].ToString()),
                    Undecided = Convert.ToInt32(dr["Undecided"].ToString() == "" || dr["Undecided"].ToString() == null ? "0" : dr["Undecided"].ToString())
                };
                Catbreakupdata.Add(cat1);
            }

            ViewBag.Catbreakupdata = Catbreakupdata;
            return View();
        }

        public ActionResult VendcatwiseReportDetails(string CatID, string StageId)
        {
            string StageId1 = null;
            string query = null;
            if (StageId == null || StageId == "")
            {
                StageId1 = "%";
            }
            else
            {
                StageId1 = Cipher.Decrypt(StageId, "");
            }
            if (CatID == null || CatID == "")
            {
                CatID = "%";
                if (StageId1 == "0")
                {
                    CatID = "%";
                }
            }
            if (StageId1 == "0")
            {
                if (CatID == "6")
                {
                    CatID = "0";
                }

                query = "select t.aon_id, t.item_description,t.Categorisation,t.Service_Lead_Service,'0' as stage,sum(cast(t.Cost as numeric)) as total_aon_cost , g.VendorCategoryName as vendorcategory from " +
  "  (select * from acq_project_master where DeletedBy is null)t  left outer join tbl_tblVendor v on t.VendorsIDs=v.vendorid left outer join tbl_tblVendorCategory g on v.VendorCategory=g.VendorCategoryID " +
                //"group by t.aon_id,t.item_description,t.Categorisation,t.Service_Lead_Service,g.VendorCategoryName having max(isnull(v.vendorcategory,0)) like " + CatID + "";
                "group by t.aon_id,t.item_description,t.Categorisation,t.Service_Lead_Service,g.VendorCategoryName having max(isnull(v.vendorcategory,0)) like ? ";

            }
            else
            {
                query = "select f.aon_id, f.item_description,f.Categorisation,f.Service_Lead_Service,(cast(f.cost as numeric))total_aon_cost,max(q.type_id)stage,g.VendorCategoryName as vendorcategory from (select * from acq_project_master f where f.DeletedBy is null)f left outer join acq_rfp_master d on f.aon_id=d.aon_id" +
  " left outer join acq_rfp_vendors p on p.rfp_id=d.rfp_id left outer join acq_rfp_vendorsdetails q on p.Rfp_fk_Id=q.rfpid left outer join tbl_tblVendor r on q.vendor_id=r.VendorId left outer join tbl_tblVendorCategory g on r.VendorCategory=g.VendorCategoryID" +
  " group by f.aon_id,f.item_description,f.Categorisation,f.Service_Lead_Service, g.VendorCategoryName,(cast(f.cost as numeric)) having count(distinct r.vendorcategory)=1 and max(r.vendorcategory) like ? and max(q.type_id) like '" + StageId1 + "'";

            }
            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {

                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                // cmd.Parameters.AddWithValue("@Id", CatID);
                cmd.Parameters.Add("@Id", OleDbType.VarChar, 500);
                cmd.Parameters["@Id"].Value = CatID;
                cmd.Connection = conn;
                //conn.Open();
                // OleDbDataReader reader = cmd.ExecuteReader();                
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }
            //DataTable dt = return_datatable(query);

            List<CatWiseDetailList> Catdata = new List<CatWiseDetailList>();
            ViewBag.CatDataCount = "0";
            string Description = null;
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["stage"].ToString() == "1")
                {
                    Description = "RFP";
                }
                else if (dr["stage"].ToString() == "2")
                {
                    Description = "Bid Submission";
                }
                else if (dr["stage"].ToString() == "3")
                {
                    Description = "TFC";
                }
                else if (dr["stage"].ToString() == "4")
                {
                    Description = "FET";
                }
                else if (dr["stage"].ToString() == "5")
                { Description = "CNC"; }
                else
                {
                    Description = "AON Accord";
                }
                //string a = Encryption.Decrypt(dr["item_description"].ToString()); Cipher.Decrypt(item["item_description"].ToString(), password)
                string b = Cipher.Decrypt(dr["item_description"].ToString(), password);
                CatWiseDetailList cat1 = new CatWiseDetailList()
                {
                    Description = Description,
                    aon_id = Convert.ToInt32(dr["aon_id"].ToString() == "" || dr["aon_id"].ToString() == null ? "0" : dr["aon_id"].ToString()),
                    Aon_description = Cipher.Decrypt(dr["item_description"].ToString(), password),
                    Categorisation = dr["Categorisation"].ToString(),
                    Service_Lead_Service = dr["Service_Lead_Service"].ToString(),
                    total_aon_cost = Convert.ToInt32(dr["total_aon_cost"].ToString() == "" || dr["total_aon_cost"].ToString() == null ? "0" : dr["total_aon_cost"].ToString()),
                    stage = Convert.ToInt32(dr["stage"].ToString() == "" || dr["stage"].ToString() == null ? "0" : dr["stage"].ToString()),
                    vendorcategory = dr["vendorcategory"].ToString()
                };
                Catdata.Add(cat1);
                ViewBag.CatDataCount = "1";
            };
            ViewBag.CatDataDetails = Catdata;
            return View();
        }


        [Route("PReport")]
        public async Task<JsonResult> PopUpReport(string stage, string Service_Lead_Service, string Categorisation)
        {
            List<StageWisePendingReport_N> BadgeChart = new List<StageWisePendingReport_N>() ;
            if (Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }

            if (Categorisation == null || Categorisation == "")
            {
                Categorisation = "%";
            }
            string query = "select * from [acq_project_status_pendingstage] where pending_in_stage= '" + stage + "'   ";
             
             
            //string query = "select d.*,a.item_description from acq_project_status_avgdelay d,acq_project_master a where " +
            //   // "d.Service_Lead_Service like '" + Service_Lead_Service + "' and d.Categorisation like '" + Categorisation + "' " +
            //   "d.Service_Lead_Service like ? and d.Categorisation like ? " +
            //    "and d.Categorisation not in " +
            //    // "('Design & Development','Make-I','Make-II','Make-III') and d.stage_name like '" + stage + "' and d.aon_id=a.aon_id order by " +
            //    "('Design & Development','Make-I','Make-II','Make-III') and d.stage_name like '" + stage + "' and d.aon_id=a.aon_id order by " +
            //    "d.stage_name,d.no_of_weeks desc";


            // DataTable dt = return_datatable(query);

            DataTable dt = new DataTable();
            using (OleDbConnection conn = masterService.DB())
            {
                OleDbDataAdapter adap = new OleDbDataAdapter();
                OleDbCommand cmd = new OleDbCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;

                // cmd.Parameters.AddWithValue("@Id", CatID);

                cmd.Parameters.Add("@Service_Lead_Service", OleDbType.VarChar, 500);
                cmd.Parameters["@Service_Lead_Service"].Value = Service_Lead_Service;

                cmd.Parameters.Add("@Categorisation", OleDbType.VarChar, 500);
                cmd.Parameters["@Categorisation"].Value = Categorisation;

                //cmd.Parameters.Add("@stage", OleDbType.VarChar, 500);
                //cmd.Parameters["@stage"].Value = stage;
                cmd.Connection = conn;
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                conn.Close();
            }


            try
            {

                DataTable dt1 = dt;
                for(int i=0;i<dt.Rows.Count;i++)
                {
                    StageWisePendingReport_N obj = new StageWisePendingReport_N();
                    try
                    {
                        
                        obj.Categorisation = dt.Rows[i]["Categorisation"].ToString();
                        obj.Service_Lead_Service = dt.Rows[i]["Service_Lead_Service"].ToString();
                        obj.item_description = Cipher.Decrypt(dt.Rows[i]["item_description"].ToString(), password);
                        obj.Date_of_Accord_of_AoN = Convert.ToDateTime(dt.Rows[i]["Date_of_Accord_of_AoN"].ToString());
                        obj.Cost = dt.Rows[i]["Cost"].ToString();
                        obj.IC_percentage = dt.Rows[i]["IC_percentage"].ToString();
                        obj.Trials_Required = dt.Rows[i]["Trials_Required"].ToString();
                        obj.TaskSlno = Convert.ToDecimal(dt.Rows[i]["TaskSlno"].ToString());
                        obj.System_case = dt.Rows[i]["System_case"].ToString();
                        obj.pending_in_stage = dt.Rows[i]["pending_in_stage"].ToString();
                        BadgeChart.Add(obj);
                    }
                    catch(Exception ex)
                    {
                        BadgeChart.Add(obj);
                        continue;
                    }
                    
                   
                }

                //BadgeChart = dt.AsEnumerable().Select(x => new StageWisePendingReport_N
                //{
                //    Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                //    item_description = Cipher.Decrypt(x.Field<string>("item_description"), password),

                //    Date_of_Accord_of_AoN = x.Field<DateTime>("Date_of_Accord_of_AoN"),
                //    Cost = x.Field<string>("Cost"),
                //    IC_percentage = x.Field<string>("IC_percentage"),
                //    Trials_Required = x.Field<string>("Trials_Required"),
                //    TaskSlno = x.Field<decimal>("TaskSlno"),
                //    System_case = x.Field<string>("System_case"),



                //    Categorisation = x.Field<string>("Categorisation"),
                //    //date_of_entering_this_stage = x.Field<DateTime>("date_of_entering_this_stage"),
                //    pending_in_stage = x.Field<string>("pending_in_stage"),
                //    date_of_entering_this_stage = x.Field<DateTime>("date_of_entering_this_stage"),
                //    Pkey = x.Field<int>("Pkey").ToString(),
                //});
                ViewBag.Grid1 = BadgeChart;
            }
            catch (Exception e)
            {
                
            }
           // return View();
            return Json(new { data = ViewBag.Grid1 }, JsonRequestBehavior.AllowGet);
        }


        [Route("ServiceWiseReport1")]
        public ActionResult ServiceWiseReport1(string Service_Lead_Service, string pending_in_stage, string system_case)
        {

            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Service-Wise Report on the Pending Cases at various Stages".ToLower())
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
            string pending_in_stage1 = null;
            string Service_Lead_Service1 = null;
            if (Service_Lead_Service != null & Service_Lead_Service != "")
            {
                Service_Lead_Service1 = Cipher.Decrypt(Service_Lead_Service, "");
            }
            else
            {
                Service_Lead_Service1 = Service_Lead_Service;
            }

            if (pending_in_stage != null & pending_in_stage != "")
            {
                pending_in_stage1 = Cipher.Decrypt(pending_in_stage, "");
            }
            else
            {
                pending_in_stage1 = pending_in_stage;
            }
            var struid = "";
            if (Session["SectionID"] != null)
            {
                struid = mService.SectionID(Session["SectionID"].ToString());
            }
            List<MODListViewModel> list1 = new List<MODListViewModel>();

            if (struid == "Navy" || struid == "ICG")
            {
                var Stagelist = list1.Select(m => m.pending_in_stage).Distinct();
                List<Categorisation> Catdata = new List<Categorisation>();
                try
                {
                    foreach (var item in Stagelist)
                    {
                        Categorisation cat1 = new Categorisation()
                        {
                            Text = item,
                            Value = Cipher.Encrypt(item, "")
                        };
                        Catdata.Add(cat1);
                    };
                }
                catch (Exception e)
                {
                    Response.Write("Step 2 : " + e.Message);
                }
                ViewBag.stage1 = Catdata;
                string[] viewdata = { "Navy", "ICG" };
                ViewBag.Service = viewdata;
            }
            else
            {
                var Stagelist = list1.Select(m => m.pending_in_stage).Distinct();
                List<Categorisation> Catdata = new List<Categorisation>();
                foreach (var item in Stagelist)
                {
                    Categorisation cat1 = new Categorisation()
                    {
                        Text = item,
                        Value = Cipher.Encrypt(item, "")
                    };
                    Catdata.Add(cat1);
                };
                ViewBag.stage1 = Catdata;
                // ViewBag.stage = list1.Select(m => m.pending_in_stage).Distinct();
                ViewBag.Service = list1.Select(m => m.Service_Lead_Service).Distinct();
            }
            if (Service_Lead_Service == null && pending_in_stage == null)
            {
                ViewBag.stage1 = "";
                ViewBag.Service1 = "";
            }
            else
            {
                ViewBag.stage1 = pending_in_stage1;
                ViewBag.Service1 = Service_Lead_Service1;
            }
            return View();
        }

    }
    

 }