using DDPAdmin.Services.Master;
using Gantt_Chart.Models;
using Gantt_Chart.Service;
using MOD.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web.Mvc;



namespace MOD.Controllers
{
    public class ReportsController : Controller
    {
        MODEntities _entities = new MODEntities();
        masterService mService = new masterService();
        GanttData ganttData = new GanttData();
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
        public ActionResult ServiceWiseReport(string Service_Lead_Service, string pending_in_stage ,string system_case)
        {
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
                    ViewBag.service1 = Service_Lead_Service;

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

            }
            if (struid == "Navy" || struid =="ICG")
            {
                var _serviceWiseReport = _entities.acq_project_status_pendingstage.ToList();
                if (_serviceWiseReport != null)
                {
                    ViewBag.service1 = Service_Lead_Service;

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

            }
            else
            {

                var _serviceWiseReport1 = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service == struid).ToList();
                if (_serviceWiseReport1 != null)
                {
                    ViewBag.service1 = Service_Lead_Service;
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
            }
            if(struid == "Navy" || struid =="ICG")
            {
                ViewBag.stage = list1.Select(m => m.pending_in_stage).Distinct();
                string[] viewdata = { "Navy", "ICG" };
                ViewBag.Service = viewdata;
            }
            else
            {
                ViewBag.stage = list1.Select(m => m.pending_in_stage).Distinct();
                ViewBag.Service = list1.Select(m => m.Service_Lead_Service).Distinct();
            }
            if (Service_Lead_Service == null && pending_in_stage == null)
            {
                ViewBag.stage1 = "";
                ViewBag.Service1 = "";
            }
            else
            {
                ViewBag.stage1 = pending_in_stage;
                ViewBag.Service1 = Service_Lead_Service;
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
                        ViewBag.service1 = Service_Lead_Service;
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

                }
                else
                {

                    var _serviceWiseReport1 = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service == struid).ToList();
                    if (_serviceWiseReport1 != null)
                    {
                        @ViewBag.service1 = Service_Lead_Service;
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
                }
                 
            }
            else
            {
                if (!string.IsNullOrEmpty(Service_Lead_Service) && !string.IsNullOrEmpty(pending_in_stage))
                {
                    @ViewBag.service1 = Service_Lead_Service;
                    ViewBag.category = pending_in_stage;
                    if(Service_Lead_Service == "Navy" || Service_Lead_Service == "ICG")
                    {
                    var result = _entities.acq_project_status_pendingstage.Where(x => (x.Service_Lead_Service == "Navy" || x.Service_Lead_Service == "ICG" || x.System_case == "Y") && x.pending_in_stage == pending_in_stage).ToList();
                        if (result != null)
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
                    }
                }
                if (!string.IsNullOrEmpty(Service_Lead_Service) && !string.IsNullOrEmpty(pending_in_stage))
                {
                    @ViewBag.service1 = Service_Lead_Service;
                    ViewBag.category = pending_in_stage;
                    var result = _entities.acq_project_status_pendingstage.Where(x => (x.Service_Lead_Service.Contains(Service_Lead_Service)) && (x.System_case =="N" || x.System_case==null) && x.pending_in_stage == pending_in_stage).ToList();
                    if (result != null)
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
                }
                else if (!string.IsNullOrEmpty(Service_Lead_Service))
                {
                    @ViewBag.service1 = Service_Lead_Service;
                    var result = _entities.acq_project_status_pendingstage.Where(x => x.Service_Lead_Service.Contains(Service_Lead_Service)).ToList();
                    if (result != null)
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
                }
                else if (!string.IsNullOrEmpty(pending_in_stage))
                {
                    ViewBag.category = pending_in_stage;
                    var result = _entities.acq_project_status_pendingstage.Where(x => x.pending_in_stage == pending_in_stage).ToList();
                    if (result != null)
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
                }
            }
            model.AonList = list;
            string query = "";
            string msystem_case = "";
            if (Service_Lead_Service == null || Service_Lead_Service == "")
                Service_Lead_Service = "%";

            if (pending_in_stage == null || pending_in_stage == "")
                pending_in_stage = "%";


            if (system_case != null && system_case != "")
                msystem_case = " and b.System_case=" + "'" + system_case + "'";

            query = "SELECT ISNULL(ROW_NUMBER() OVER (ORDER BY @@identity), - 1) AS Pkey,b.TaskSlno, b.pending_in_stage, count(*) n_projects, " +
                " isnull(sum(cast(b.cost AS decimal)),'0') total_aon_cost " +
" FROM acq_project_status_pendingstage b where b.Service_Lead_Service like '" + Service_Lead_Service + "' and b.pending_in_stage like '" + pending_in_stage + "' "+msystem_case+""+

" GROUP BY b.TaskSlno, b.pending_in_stage order by TaskSlno";

            List<DetailCharts> dataPoints = new List<DetailCharts>();
            IEnumerable<Service_WiseReport> Badge = null;

            DataTable dt = return_datatable(query);
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
            return View(model);
        }

        public ActionResult ServiceWiseGraph()
        {
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            dataPoints = mService.Service_WiseReport();
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);
            return View();
        }

        public ActionResult ServiceWiseGraphAll()
        {
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


        public ActionResult particularstage(string stage_name, string Service_Lead_Service, string Categorisation)
        {
            string query = "";
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            List<Grid4> Mmodel1 = new List<Grid4>();


            if (stage_name == null || stage_name == "")
            {
                stage_name = "%";
            }
            if (Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }
            if (Categorisation == null || Categorisation == "")
            {
                Categorisation = "%";
            }

            query = "select f.*,a.item_description,g.completed_on from acq_project_status_avgdelay f," +
                " acq_project_status_timelines g , acq_project_master a where a.aon_id=f.aon_id and f.aon_id=g.aon_id and " +
                " f.stage=g.TaskSlno and f.stage_name='" + stage_name + "' and f.Service_Lead_Service like '"+ Service_Lead_Service + "' and f.Categorisation like '"+ Categorisation + "' order by g.completed_on";


            DataTable dt = return_datatable(query);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    Grid4 list = new Grid4
                    {
                        no_of_weeks = Convert.ToInt32(item["no_of_weeks"].ToString()),
                        stage_name = item["stage_name"].ToString(),
                        item_description = Cipher.Decrypt(item["item_description"].ToString(),password),

                    };
                    Mmodel1.Add(list);
                }
                foreach (DataRow row in dt.Rows)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(Cipher.Decrypt(row["item_description"].ToString(),password)), Convert.ToDouble(row["no_of_weeks"].ToString()), Convert.ToDouble(row["no_of_weeks"].ToString())));
                }
                ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

                

                ViewBag.Grid1 = Mmodel1;
            }
            return View();
        }

        public ActionResult StageWiseDelay(string Categorisation, string Service_Lead_Service, string stage)
        {
            string query = "";
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            List<Grid1> Mmodel1 = new List<Grid1>();

            if (Categorisation == null || Categorisation=="")
            {
                Categorisation = "%";
            }

            if(Service_Lead_Service == null || Service_Lead_Service == "")
            {
                Service_Lead_Service = "%";
            }

            if(stage==null || stage=="")
            {
                stage = "'0.0'";
            }
            else if(stage=="TM")
            {
                stage = "'12','13'";
                
            }
            else if (stage == "AM")
            {
                stage = "'2','4','6','10'";
            }

            query = "select d.stage,d.stage_name,avg(d.no_of_weeks)no_of_weeks,isnull((case stage when 2 then " +
                " 6 when 4 then 10 when 6 then 24 when 10 then 6 when 12 then 26 when 13 then 18 end),'0') dap_timeline from acq_project_status_avgdelay d " +
            "  where d.Service_Lead_Service like '" + Service_Lead_Service + "' and d.Categorisation like '" + Categorisation + "' and d.Categorisation not in " +
            "('Design & Development','Make-I','Make-II','Make-III') and d.stage not in (" + stage + ")  group by d.stage,d.stage_name order by 1";

            
            DataTable dt = return_datatable(query);
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

        public ActionResult ReportOnClick(string stage, string Service_Lead_Service,string Categorisation)
        {
            IEnumerable<StageWiseDelayReport> BadgeChart = null;
            if(Service_Lead_Service==null || Service_Lead_Service=="")
            {
                Service_Lead_Service = "%";
            }

            if(Categorisation==null || Categorisation=="")
            {
                Categorisation = "%";
            }

            string query = "select d.*,a.item_description from acq_project_status_avgdelay d,acq_project_master a where " +
                "d.Service_Lead_Service like '"+ Service_Lead_Service + "' and d.Categorisation like '"+ Categorisation + "' " +
                "and d.Categorisation not in " +
                "('Design & Development','Make-I','Make-II','Make-III') and d.stage_name like '" + stage + "' and d.aon_id=a.aon_id order by " +
                "d.stage_name,d.no_of_weeks desc";
            DataTable dt = return_datatable(query);
            BadgeChart = dt.AsEnumerable().Select(x => new StageWiseDelayReport
            {
                Service_Lead_Service = x.Field<string>("Service_Lead_Service"),
                item_description = Cipher.Decrypt(x.Field<string>("item_description"), password),
                Categorisation = x.Field<string>("Categorisation"),
                stage = x.Field<decimal>("stage"),
                stage_name = x.Field<string>("stage_name"),
                no_of_weeks = x.Field<int>("no_of_weeks"),
                aon_id = x.Field<int>("aon_id"),
            });
            ViewBag.Grid1 = BadgeChart;
            return View();
            //return Json(new { data = ViewBag.Grid1 }, JsonRequestBehavior.AllowGet);
        }
    }
}