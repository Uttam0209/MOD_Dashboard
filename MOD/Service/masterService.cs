using DDPAdmin.Services.Master;
using Gantt_Chart.Models;
using MOD.Models;
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
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Gantt_Chart.Service
{
    public class masterService
    {
        private static string ConString = ConfigurationManager.AppSettings["ConString"].ToString();
        private readonly MODEntities _entities;
        string password = "p@SSword";
        public masterService()
        {
            _entities = new MODEntities();
        }
        public static OleDbConnection DB()
        {
            //OleDbConnection conn = new OleDbConnection(DecryptData(ConString));
           OleDbConnection conn = new OleDbConnection(ConString);
            conn.Open();
            return conn;
        }

        public static string DecryptData(string strData)
        {
            byte[] key = { };// Key   
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };
            byte[] inputByteArray = new byte[strData.Length];
            strData = strData.Replace("%2b", "+");

            try
            {
                key = Encoding.UTF8.GetBytes("W@!dghDW");
                DESCryptoServiceProvider ObjDES = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strData);

                MemoryStream Objmst = new MemoryStream();
                CryptoStream Objcs = new CryptoStream(Objmst, ObjDES.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                Objcs.Write(inputByteArray, 0, inputByteArray.Length);
                Objcs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8;
                return encoding.GetString(Objmst.ToArray());
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public List<ProjectViewModel> GetProjectDetails(long id)
        {
            List<ProjectViewModel> Mmodel = new List<ProjectViewModel>();
            IEnumerable<tbl_mst_Project> _isUserExdfxists = null;
            if (id != 0)
            {
                _isUserExdfxists = _entities.tbl_mst_Project.Where(x => x.IsActive == "Y" && x.ProjectId == id).ToList();

            }
            else
            {
                _isUserExdfxists = _entities.tbl_mst_Project.Where(x => x.IsActive == "Y").ToList();
            }

            if (_isUserExdfxists != null)
            {
                foreach (tbl_mst_Project item in _isUserExdfxists)
                {
                    ProjectViewModel list = new ProjectViewModel
                    {
                        ProjectId = item.ProjectId,
                        ProjectName = item.ProjectName,
                        TemplateType = item.TemplateType,
                        StartDate = item.StartDate,
                        CreatedBy = item.CreatedBy,
                        IsActive = item.IsActive
                    };
                    Mmodel.Add(list);
                }

            }
            return Mmodel;
        }
        public List<WaliTest> GetProjectDasboardDetails(string id, string mTaskSlno, string id5, string categorisation, string service)
        {
            List<WaliTest> Mmodel = new List<WaliTest>();
            DateTime mdate = Convert.ToDateTime("2020-01-01");
            SqlParameter param1 = new SqlParameter("@taskid", id);
            SqlParameter param2 = new SqlParameter("@stageid", mTaskSlno);
            SqlParameter param3 = new SqlParameter("@Type", id5);
            SqlParameter param4 = new SqlParameter("@Date", mdate);
            SqlParameter param5 = new SqlParameter("@categorisation", categorisation);
            SqlParameter param6 = new SqlParameter("@service", service);
            var _isUserExdfxists = _entities.Database.SqlQuery<temp_dashboardproject>("exec PROC_PRODUCT_DESCRITPTION @taskid,@stageid,@Type,@Date,@categorisation,@service ", param1, param2, param3, param4, param5, param6).ToList();
            if (_isUserExdfxists != null)
            {
                foreach (temp_dashboardproject item in _isUserExdfxists)
                {
                    WaliTest list = new WaliTest
                    {
                        ProjectId = Convert.ToInt32(item.ProjectId),
                        ProjectName = item.item_description,
                        Template_type = item.categorisation,
                        StartDate = Convert.ToDateTime(item.AoNDate),
                        AoNDate = Convert.ToDateTime(item.AoNDate),
                        service = item.service,
                        scheduled_date_of_completion = Convert.ToDateTime(item.scheduled_date_of_completion),
                        actual_no_of_days = item.actual_no_of_days,
                        scheduled_no_of_days = item.scheduled_no_of_days,
                        percent_delay = Convert.ToInt32(item.percent_delay),


                    };
                    Mmodel.Add(list);
                }
            }

            return Mmodel;
        }
        //public List<vw_assignofTask> GetAssignOfTaskwithUserDetails(long id)
        //{
        //    List<vw_assignofTask> Mmodel = new List<vw_assignofTask>();
        //    IEnumerable<vw_assignofTask> _isUserExdfxists = null;
        //    if (id != 0)
        //    {
        //        _isUserExdfxists = _entities.vw_assignofTask.Where(x => x.IsActive == "Y" && x.ProjectId == id).ToList();

        //    }
        //    else
        //    {
        //        _isUserExdfxists = _entities.vw_assignofTask.Where(x => x.IsActive == "Y").ToList();
        //    }

        //    if (_isUserExdfxists != null)
        //    {
        //        foreach (vw_assignofTask item in _isUserExdfxists)
        //        {
        //            vw_assignofTask list = new vw_assignofTask
        //            {
        //                ProjectId = item.ProjectId,
        //                TryPojectID = item.TryPojectID,
        //                Template_type = item.Template_type,
        //                TaskSlno = item.TaskSlno,
        //                TaskDescription = item.TaskDescription,
        //                StartDate = item.StartDate,
        //                EndDate = item.EndDate,
        //                Dependencies = item.Dependencies,
        //                super_task_id = item.super_task_id,
        //                Responsibility = item.Responsibility,
        //                UserName = item.UserName,
        //                ProjectName = item.ProjectName,
        //                IsActive = item.IsActive,
        //            };
        //            Mmodel.Add(list);
        //        }

        //    }
        //    return Mmodel;
        //}
        public List<DashboardViewModel> GetDashboardDetails(string Categorisation, string Service_Lead_Service)
        {
            List<DashboardViewModel> Mmodel = new List<DashboardViewModel>();
            if (Categorisation == null && Service_Lead_Service == null)
            {
                Categorisation = "''";
                Service_Lead_Service = "''";
            }
            SqlParameter param1 = new SqlParameter("@categorisation", Categorisation);
            SqlParameter param2 = new SqlParameter("@service", Service_Lead_Service);
            var mc = _entities.Database.SqlQuery<temp_dashboard>("exec PROC_DASHBOARD_DATA @categorisation,@service", param1, param2).ToList();
            //var mc = _entities.DASHBOARDDATA().ToList();
            foreach (temp_dashboard item in mc)
            {
                DashboardViewModel list = new DashboardViewModel
                {
                    color = item.color,
                    TaskDescription = item.TaskDescription.Trim(),
                    Id = item.TaskSlno.ToString(),
                    Qty = item.nProject.ToString(),
                    Name = item.ShortName
                };
                Mmodel.Add(list);
            }
            return Mmodel;
        }
        //public List<WaliTest> GetDashboardDetails1(long id, string taskid, string id5)
        //{
        //    List<WaliTest> Mmodel = new List<WaliTest>();
        //    DataTable dt = DashBoardData1(Convert.ToInt32(id), taskid, id5);
        //    foreach (DataRow item in dt.Rows)
        //    {
        //        WaliTest list = new WaliTest
        //        {
        //            ProjectId = Convert.ToInt32(item["ProjectId"].ToString()),
        //            ProjectName = item["item_description"].ToString(),
        //            categorisation = item["categorisation"].ToString(),
        //            service = item["service"].ToString(),
        //            AoNDate = Convert.ToDateTime(item["AoNDate"])
        //        };
        //        Mmodel.Add(list);
        //    }
        //    return Mmodel;
        //}
        //public DataTable DashfirstGridData()/// Get new deals list data
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    return Func_DataTable(cmd, "PROC_DASHBOARD_First_CountGrid");
        //}

        //public DataTable DashfirstGridData(int id)/// Get new deals list data
        //{
        //    SqlCommand cmd = new SqlCommand();
        //    return Func_DataTable(cmd, "PROC_DASHBOARD_First_CountGrid", id);
        //}
        public List<CaseViewModel> GetCasesTable(decimal id, string Categorisation, string Service_Lead_Service, DateTime myDate)
        {
            if(Service_Lead_Service!=null || Service_Lead_Service!="")
            {

            }
            else
            {
                Service_Lead_Service = "''";
            }
            if (Categorisation != null || Categorisation != "")
            {

            }
            else
            {
                Categorisation = "''";
            }
            List<CaseViewModel> Mmodel = new List<CaseViewModel>();
            SqlParameter param1 = new SqlParameter("@stageid", id);
            //SqlParameter param2 = new SqlParameter("@date", myDate);
            SqlParameter param2 = new SqlParameter("@date", "");
            SqlParameter param3 = new SqlParameter("@categorisation", Categorisation);
            SqlParameter param4 = new SqlParameter("@service", Service_Lead_Service);
            var _isUserExdfxists = _entities.Database.SqlQuery<temp_BF>("exec PROC_DASHBOARD_First_CountGrid @stageid,@date,@categorisation,@service", param1, param2, param3, param4).SingleOrDefault();
            if (_isUserExdfxists != null)
            {
                CaseViewModel list = new CaseViewModel
                {
                    BF = Convert.ToInt16(_isUserExdfxists.BF),
                    Cases = Convert.ToInt16(_isUserExdfxists.Cases),
                    Casesdisposed = Convert.ToInt16(_isUserExdfxists.Casesdisposed),
                    Outstanding = Convert.ToInt16(_isUserExdfxists.Outstanding),
                };
                Mmodel.Add(list);
            }
            return Mmodel;
        }
        public List<UpdateStatusViewModel> GetupdateStatusTaskDetails(long id)
        {
            List<UpdateStatusViewModel> Mmodel = new List<UpdateStatusViewModel>();
            IEnumerable<vw_assignofTask> _isUserExdfxists = null;

            _isUserExdfxists = _entities.vw_assignofTask.Where(x => x.ResponsibilityDepart == Convert.ToString(id)).ToList();
            if (_isUserExdfxists != null)
            {


                foreach (vw_assignofTask item in _isUserExdfxists)
                {
                    var _isUserStatusUpdateExdfxists = _entities.tbl_trn_UpdateStausDummy.Where(x => x.TaskSlno == Convert.ToString(item.TaskSlno)).FirstOrDefault();
                    DateTime mActuaStateDate = new DateTime();
                    DateTime mActuaEndDate = new DateTime();
                    string mRemark = "";
                    if (_isUserStatusUpdateExdfxists != null)
                    {
                        // mRemark = _isUserStatusUpdateExdfxists.Remark;
                        mActuaStateDate = Convert.ToDateTime(_isUserStatusUpdateExdfxists.ActuaStartDate);
                        mActuaEndDate = Convert.ToDateTime(_isUserStatusUpdateExdfxists.ActuaEndDate);
                    }
                    else
                    {
                        mRemark = "";
                        mActuaStateDate = Convert.ToDateTime(item.StartDate);
                        mActuaStateDate = Convert.ToDateTime(item.EndDate);
                    }
                    UpdateStatusViewModel list = new UpdateStatusViewModel
                    {
                        ProjectId = item.ProjectId,
                        TryPojectID = item.TryPojectID,
                        ActuaEndDate = mActuaEndDate,
                        ActuaStartDate = mActuaStateDate,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Remark = mRemark,
                        TaskSlno = item.TaskSlno.ToString(),
                        UserID = Convert.ToInt64(item.Responsibility),
                        TaskDescription = item.TaskDescription,
                        UserName = item.UserName,
                        ProjectName = item.ProjectName,
                    };
                    Mmodel.Add(list);
                }
            }
            return Mmodel;
        }
        public List<vw_assignofTask> GetSubTaskDetails(long id)
        {
            List<vw_assignofTask> Mmodel = new List<vw_assignofTask>();
            IEnumerable<vw_assignofTask> _isSubTaskExdfxists = null;
            if (id != 0)
            {
                var _isUserExdfxists = _entities.vw_assignofTask.Where(x => x.IsActive == "Y" && x.TryPojectID == id).FirstOrDefault();
                _isSubTaskExdfxists = _entities.vw_assignofTask.Where(x => x.IsActive == "Y" && x.super_task_id == _isUserExdfxists.TaskSlno).ToList();

            }
            else
            {
                _isSubTaskExdfxists = _entities.vw_assignofTask.Where(x => x.IsActive == "Y").ToList();
            }

            if (_isSubTaskExdfxists != null)
            {
                foreach (vw_assignofTask item in _isSubTaskExdfxists)
                {
                    vw_assignofTask list = new vw_assignofTask
                    {
                        ProjectId = item.ProjectId,
                        TryPojectID = item.TryPojectID,
                        Template_type = item.Template_type,
                        TaskSlno = item.TaskSlno,
                        TaskDescription = item.TaskDescription,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Dependencies = item.Dependencies,
                        super_task_id = item.super_task_id,
                        Responsibility = item.Responsibility,
                        UserName = item.UserName,
                        ProjectName = item.ProjectName,
                        IsActive = item.IsActive,
                    };
                    Mmodel.Add(list);
                }
            }
            return Mmodel;
        }
        public List<tryProjectViewModel> GetTryProjectDetails(long id, long mDepart)
        {
            List<tryProjectViewModel> Mmodel = new List<tryProjectViewModel>();
            IEnumerable<tbl_trn_tryProject> _isUserExdfxists = null;
            if (id != 0)
            {
                _isUserExdfxists = _entities.tbl_trn_tryProject.Where(x => x.IsActive == "Y" && x.ProjectId == id && x.ResponsibilityDepart == Convert.ToString(mDepart)).ToList();
            }
            else
            {
                _isUserExdfxists = _entities.tbl_trn_tryProject.Where(x => x.IsActive == "Y").ToList();
            }

            if (_isUserExdfxists != null)
            {
                foreach (tbl_trn_tryProject item in _isUserExdfxists)
                {
                    tryProjectViewModel list = new tryProjectViewModel
                    {
                        ProjectId = Convert.ToInt32(item.ProjectId),
                        TryPojectID = Convert.ToInt32(item.TryPojectID),
                        Template_type = item.Template_type,
                        TaskSlno = Convert.ToString(item.TaskSlno),
                        TaskDescription = item.TaskDescription,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Dependencies = item.Dependencies,
                        super_task_id = item.super_task_id,
                        Responsibility = Convert.ToInt32(item.Responsibility),
                        IsActive = item.IsActive,
                    };
                    Mmodel.Add(list);
                }

            }
            return Mmodel;
        }
        public List<tryProjectViewModel> ViewTryProjectDetails(long id)
        {
            List<tryProjectViewModel> Mmodel = new List<tryProjectViewModel>();
            IEnumerable<tbl_trn_tryProject> _isUserExdfxists = null;
            if (id != 0)
            {
                _isUserExdfxists = _entities.tbl_trn_tryProject.Where(x => x.IsActive == "Y" && x.TryPojectID == id).ToList();

            }
            else
            {
                _isUserExdfxists = _entities.tbl_trn_tryProject.Where(x => x.IsActive == "Y").ToList();
            }

            if (_isUserExdfxists != null)
            {
                foreach (tbl_trn_tryProject item in _isUserExdfxists)
                {
                    tryProjectViewModel list = new tryProjectViewModel
                    {
                        ProjectId = Convert.ToInt32(item.ProjectId),
                        TryPojectID = Convert.ToInt32(item.TryPojectID),
                        Template_type = item.Template_type,
                        TaskSlno = Convert.ToString(item.TaskSlno),
                        TaskDescription = item.TaskDescription,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        Dependencies = item.Dependencies,
                        super_task_id = item.super_task_id,
                        Responsibility = Convert.ToInt32(item.Responsibility),
                        IsActive = item.IsActive,
                    };
                    Mmodel.Add(list);
                }

            }
            return Mmodel;
        }
        public ProjectViewModel SaveProject(ProjectViewModel model)
        {
            using (DbContextTransaction dbTran = _entities.Database.BeginTransaction())
            {

                try
                {

                    tbl_mst_Project Eventexists = _entities.tbl_mst_Project.Where(x => x.ProjectId == model.ProjectId && x.IsActive == "Y").FirstOrDefault();
                    if (Eventexists == null)
                    {
                        tbl_mst_Project _Project = new tbl_mst_Project
                        {

                            ProjectId = 0,
                            ProjectName = model.ProjectName,
                            TemplateType = model.TemplateType,
                            CreatedBy = model.CreatedBy,
                            StartDate = model.StartDate,
                            RecInsTime = System.DateTime.Now,
                            TempleteId = 1,
                            CreateOn = System.DateTime.Now,
                            IsActive = "Y"
                        };
                        _entities.tbl_mst_Project.Add(_Project);
                        _entities.SaveChanges();
                        dbTran.Commit();
                        model.Message = "Saved";
                    }
                    else
                    {
                        Eventexists.ProjectId = model.ProjectId;
                        Eventexists.ProjectName = model.ProjectName;
                        Eventexists.TemplateType = model.TemplateType;
                        Eventexists.CreatedBy = model.CreatedBy;
                        Eventexists.StartDate = model.StartDate;

                        _entities.SaveChanges();
                        dbTran.Commit();

                        model.Message = "Update";
                    }


                }
                catch (DbEntityValidationException e)
                {
                    foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (DbValidationError ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    model.Message = "Not";
                    dbTran.Rollback();
                    throw;
                }
            }

            return model;
        }
        public tryProjectViewModel AssignOfTask(tryProjectViewModel model, string mType)
        {
            using (DbContextTransaction dbTran = _entities.Database.BeginTransaction())
            {

                try
                {
                    if (model.TryPojectID == 0)
                    {
                        model.TryPojectID = Convert.ToInt64(model.TaskDescription);
                    }

                    var Templateexists = _entities.tbl_trn_tryProject.Where(x => x.TryPojectID == model.TryPojectID).FirstOrDefault();

                    if (mType != "Save")
                    {
                        if (Templateexists != null)
                        {
                            Templateexists.Responsibility = Convert.ToString(model.Responsibility);
                            _entities.SaveChanges();


                        }
                    }
                    else
                    {
                        tbl_trn_tryProject _TryProject = new tbl_trn_tryProject
                        {
                            ProjectId = Templateexists.ProjectId,
                            Template_type = model.Template_type,
                            TaskSlno = Convert.ToInt32(model.TaskSlno),
                            TaskDescription = model.TaskDescription,
                            StartDate = model.StartDate,
                            EndDate = model.EndDate,
                            Dependencies = model.Dependencies,
                            super_task_id = model.super_task_id,
                            Responsibility = Convert.ToString(model.Responsibility),
                            ResponsibilityDepart = Convert.ToString(model.Responsibility),

                            IsActive = model.IsActive,

                        };
                        _entities.tbl_trn_tryProject.Add(_TryProject);
                        _entities.SaveChanges();

                    }

                    dbTran.Commit();

                    model.Message = "Saved";

                }
                catch (DbEntityValidationException e)
                {
                    foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (DbValidationError ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    model.Message = "Not";
                    dbTran.Rollback();
                    throw;
                }
            }

            return model;
        }
        //public UpdateStatusViewModel UpdateStatusTaskProject(UpdateStatusViewModel model)
        //{
        //    using (DbContextTransaction dbTran = _entities.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            tbl_trn_UpdateStausDummy _TryProject = new tbl_trn_UpdateStausDummy
        //            {
        //                ProjectId = model.ProjectId,
        //                TaskSlno = model.TaskSlno,
        //                //TryPojectID = Convert.ToInt64(model.TaskSlno),
        //                ActuaStartDate = model.ActuaStartDate,
        //                ActuaEndDate = model.ActuaEndDate,
        //                //UserID = model.UserID,
        //                //Remark = model.Remark,
        //                //RecTime = System.DateTime.Today,
        //            };
        //            _entities.tbl_trn_UpdateStausDummy.Add(_TryProject);
        //            _entities.SaveChanges();
        //            dbTran.Commit();
        //            model.Message = "Saved";

        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (DbValidationError ve in eve.ValidationErrors)
        //                {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            model.Message = "Not";
        //            dbTran.Rollback();
        //            throw;
        //        }
        //    }

        //    return model;
        //}
        //public ProjectViewModel SaveTryProject(ProjectViewModel model)
        //{
        //    using (DbContextTransaction dbTran = _entities.Database.BeginTransaction())
        //    {
        //        try
        //        {
        //            List<tbl_mst_Template> Templateexists = _entities.tbl_mst_Template.Where(x => x.IsActive == "Y").ToList();

        //            if (Templateexists != null)
        //            {
        //                tbl_mst_Project Projectexists = _entities.tbl_mst_Project.Where(x => x.ProjectName == model.ProjectName && x.IsActive == "Y").FirstOrDefault();

        //                foreach (tbl_mst_Template item in Templateexists)
        //                {
        //                    if (item.Dependencies != null)
        //                    {
        //                        tbl_trn_tryProject DependenciesProjectexists = _entities.tbl_trn_tryProject.Where(x => x.TaskSlno == Convert.ToInt32(item.Dependencies) && x.IsActive == "Y").FirstOrDefault();
        //                        if (DependenciesProjectexists != null)
        //                        {
        //                            Projectexists.StartDate = Convert.ToDateTime(DependenciesProjectexists.EndDate).AddDays(1);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Projectexists.StartDate = Projectexists.StartDate;
        //                    }
        //                    tbl_trn_tryProject _TryProject = new tbl_trn_tryProject
        //                    {
        //                        ProjectId = Convert.ToInt32(Projectexists.ProjectId),
        //                        Template_type = Projectexists.TemplateType,
        //                        TaskSlno = Convert.ToInt32(item.TaskSlno),
        //                        ShortName = item.ShortName,
        //                        TaskDescription = item.TaskDescription,
        //                        StartDate = Projectexists.StartDate,
        //                        EndDate = Convert.ToDateTime(Projectexists.StartDate).AddDays((Convert.ToDouble(item.Duration)) * 7),
        //                        Dependencies = item.Dependencies,
        //                        super_task_id = Convert.ToInt32(item.super_task_id),
        //                        Responsibility = Convert.ToString(item.Responsibility),
        //                        ResponsibilityDepart = Convert.ToString(item.Responsibility),
        //                        IsActive = item.IsActive,

        //                    };
        //                    _entities.tbl_trn_tryProject.Add(_TryProject);
        //                    _entities.SaveChanges();

        //                }

        //                dbTran.Commit();

        //            }

        //            model.Message = "Saved";

        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            foreach (DbEntityValidationResult eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (DbValidationError ve in eve.ValidationErrors)
        //                {
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            model.Message = "Not";
        //            dbTran.Rollback();
        //            throw;
        //        }
        //    }

        //    return model;
        //}

        // public DataTable DashBoardData()/// Get new deals list data
        //{
        //    SqlCommand cmd = new SqlCommand();

        //return Func_DataTable(cmd, "PROC_DASHBOARD_DATA");
        // }
        // public DataTable DashBoardData1(int id, string id2, string id5)/// Get new deals list data
        //  {
        //    SqlCommand cmd = new SqlCommand();
        // return Func_DataTable2(cmd, "PROC_PRODUCT_DESCRITPTION", id, id2, id5);
        // }
        //public DataTable Func_DataTable(SqlCommand cmd, string pro)
        //{
        //    SqlConnection cn = new SqlConnection(sqlString);
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = pro;
        //    //SqlParameter property_id = cmd.Parameters.Add("@outstanding", SqlDbType.Int);
        //    // property_id.Value = mvalue;
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        adp.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        var te = ex.Message;
        //    }
        //    finally
        //    {
        //        adp.Dispose();
        //        cmd.Dispose();
        //        dt.Dispose();
        //    }
        //    return dt;
        //}
        //public DataTable Func_DataTable(SqlCommand cmd, string pro, int id)
        //{
        //    SqlConnection cn = new SqlConnection(sqlString);
        //    cmd.Connection = cn;
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.Add(new SqlParameter("@stageid", id));
        //    cmd.CommandText = pro;
        //    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        adp.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        var te = ex.Message;
        //    }
        //    finally
        //    {
        //        adp.Dispose();
        //        cmd.Dispose();
        //        dt.Dispose();
        //    }
        //    return dt;
        //}

        ////public DataTable Func_DataTable2(SqlCommand cmd, string pro, int id, string mTaskSlno, string id5)
        ////{
        ////    SqlConnection cn = new SqlConnection(sqlString);
        ////    cmd.Connection = cn;
        ////    cmd.CommandType = CommandType.StoredProcedure;
        ////    //cmd.Parameters.Add(new SqlParameter("@mid", id));
        ////    cmd.Parameters.Add(new SqlParameter("@taskid", id));
        ////    cmd.Parameters.Add(new SqlParameter("@type", id5));
        ////    cmd.Parameters.Add(new SqlParameter("@stageid", ""));
        ////    cmd.Parameters.Add(new SqlParameter("@Date", "2020/01/01"));
        ////    cmd.CommandText = pro;
        ////    SqlDataAdapter adp = new SqlDataAdapter(cmd);
        ////    DataTable dt = new DataTable();
        ////    try
        ////    {
        ////        adp.Fill(dt);
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        var te = ex.Message;
        ////    }
        ////    finally
        ////    {
        ////        adp.Dispose();
        ////        cmd.Dispose();
        ////        dt.Dispose();
        ////    }
        ////    return dt;
        ////}
        ///

        //Code for grid2
        public List<Grid2> GetGrid2()
        {
            List<Grid2> Mmodel = new List<Grid2>();
            IEnumerable<vw_Grid2> _isUserExdfxists = null;
            _isUserExdfxists = _entities.vw_Grid2.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (vw_Grid2 item in _isUserExdfxists)
                {
                    Grid2 list = new Grid2
                    {
                        IdPrimaryKey = Convert.ToInt32(item.IdPrimaryKey),
                        stage = item.IdPrimaryKey,
                        army_projects = item.army_projects,
                        iaf_projects = item.iaf_projects,
                        navy_projects = item.navy_projects,
                        other_projects = item.other_projects,
                        TaskDescription = item.TaskDescription,
                    };
                    Mmodel.Add(list);
                }
            }
            return Mmodel;
        }
        public List<Grid1> GetGrid1()
        {
            List<Grid1> Mmodel1 = new List<Grid1>();
            IEnumerable<vw_GridFirst> _isUserExdfxists = null;

            _isUserExdfxists = _entities.vw_GridFirst.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (vw_GridFirst item in _isUserExdfxists)
                {
                    Grid1 list = new Grid1
                    {
                        IdPrimaryKey = item.IdPrimaryKey,
                        task_id = Convert.ToInt32(item.task_id),
                        avg_delay_days = Convert.ToInt32(item.avg_delay_days),
                        taskDescription = item.taskDescription,
                    };
                    Mmodel1.Add(list);
                }
            }
            return Mmodel1;
        }

        public List<DetailCharts> Chart12()
        {
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            IEnumerable<vw_ChartFirst> _isUserExdfxists = null;
            _isUserExdfxists = _entities.vw_ChartFirst.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (vw_ChartFirst item in _isUserExdfxists)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(item.label_text), Convert.ToDouble(item.delay_lt90)));
                }
            }
            return dataPoints;
        }
        public List<DetailCharts> Service_WiseReport()
        {
            List<DetailCharts> dataPoints = new List<DetailCharts>();
            IEnumerable<Service_WiseReport> _isUserExdfxists = null;
            _isUserExdfxists = _entities.Service_WiseReport.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (Service_WiseReport item in _isUserExdfxists)
                {
                    dataPoints.Add(new DetailCharts(Convert.ToString(item.pending_in_stage), Convert.ToDouble(item.n_projects)));
                }
            }
            return dataPoints;
        }

        public List<DetailCharts> Chart123()
        {
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            IEnumerable<vw_Chart2> _isUserExdfxists = null;
            _isUserExdfxists = _entities.vw_Chart2.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (vw_Chart2 item in _isUserExdfxists)
                {
                    dataPoints1.Add(new DetailCharts(Convert.ToString(item.Categorisation), Convert.ToDouble(item.no_of_projects)));
                }
            }
            return dataPoints1;
        }

        public List<WReports> GetReportGridId(string mitem_description)
        {
            List<WReports> Mmodel1 = new List<WReports>();
            IEnumerable<TimeLineReport1> _isUserExdfxists = null;

            _isUserExdfxists = _entities.TimeLineReport1.Where(x => x.item_description == mitem_description).ToList();
            if (_isUserExdfxists != null)
            {
                foreach (TimeLineReport1 item in _isUserExdfxists)
                {
                    WReports list = new WReports
                    {
                        Pkey = item.Pkey,
                        Service_Lead_Service = item.Service_Lead_Service,
                        item_description = Cipher.Decrypt(item.item_description, password),
                        Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN,
                        Cost = item.Cost,
                        Categorisation = item.Categorisation,
                        IC_percentage = item.IC_percentage,
                        Trials_Required = item.Trials_Required,
                        TaskDescription = item.TaskDescription,
                        completed_on = Convert.ToDateTime(item.completed_on),
                        no_of_weeks = item.no_of_weeks,
                        dap_timeline = item.dap_timeline,
                        TaskSlno = Convert.ToDecimal(item.TaskSlno),
                    };
                    Mmodel1.Add(list);
                }
            }
            return Mmodel1;
        }

        public List<WReports> GetReportGrid1()
        {
            List<WReports> Mmodel1 = new List<WReports>();
            IEnumerable<TimeLineReport1> _isUserExdfxists = null;
            _isUserExdfxists = _entities.TimeLineReport1.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (TimeLineReport1 item in _isUserExdfxists)
                {
                    WReports list = new WReports
                    {
                        Pkey = item.Pkey,
                        Service_Lead_Service = item.Service_Lead_Service,
                        item_description = Cipher.Decrypt(item.item_description, password),
                        Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN,
                        Cost = item.Cost,
                        Categorisation = item.Categorisation,
                        IC_percentage = item.IC_percentage,
                        Trials_Required = item.Trials_Required,
                        TaskDescription = item.TaskDescription,
                        completed_on = Convert.ToDateTime(item.completed_on),
                        no_of_weeks = item.no_of_weeks,
                        dap_timeline = item.dap_timeline,
                        TaskSlno = Convert.ToDecimal(item.TaskSlno),
                    };
                    Mmodel1.Add(list);
                }
            }
            return Mmodel1;
        }
        
        public List<DetailCharts> ChartMileStone()
        {
            string password = "p@SSword";
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            IEnumerable<ReportMileStonebyMonth> _isUserExdfxists = null;
            _isUserExdfxists = _entities.ReportMileStonebyMonths.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (ReportMileStonebyMonth item in _isUserExdfxists)
                {
                    dataPoints1.Add(new DetailCharts(Convert.ToString(Cipher.Decrypt(item.TaskDescription, password)), Convert.ToDouble(item.projects)));
                }
            }
            return dataPoints1;
        }
        public List<DetailCharts> ChartMileStone1()
        {
            string password = "p@SSword";
            List<DetailCharts> dataPoints1 = new List<DetailCharts>();
            IEnumerable<ReportMileStonebyMonth> _isUserExdfxists = null;
            _isUserExdfxists = _entities.ReportMileStonebyMonths.ToList();
            if (_isUserExdfxists != null)
            {
                foreach (ReportMileStonebyMonth item in _isUserExdfxists)
                {
                    dataPoints1.Add(new DetailCharts(Convert.ToString(item.TaskDescription), Convert.ToDouble(item.projects)));
                }
            }
            return dataPoints1;
        }
        public string SectionID(string mID)
        {
            string mService_Lead_Service = "";
            if (mID == "2" || mID == "5" || mID == "8")
            {
                mService_Lead_Service = "AirForce";
            }
            else if (mID == "3" || mID == "6" || mID == "9")
            {
                mService_Lead_Service = "Army";
            }
            else if (mID == "4" || mID == "7" || mID == "10")
            {
                mService_Lead_Service = "Navy";
            }
            else if (mID == "13")
            {
                mService_Lead_Service = "SuperAdmin";
            }
            else
            {
                mService_Lead_Service = "SuperAdmin";
            }
            return mService_Lead_Service;
        }
        public List<WReports> GetReportGridOnClickChart(string TaskDescription)
        {
            List<WReports> Mmodel1 = new List<WReports>();
            IEnumerable<ChartReportOnClick> Chart = null;
            Chart = _entities.ChartReportOnClicks.OrderBy(x => x.Service_Lead_Service).Where(x => x.TaskDescription == TaskDescription).ToList();
            if (Chart != null)
            {
                foreach (ChartReportOnClick item in Chart)
                {
                    WReports list = new WReports
                    {
                        Pkey = item.Pkey,
                        TaskDescription = item.TaskDescription,
                        Service_Lead_Service = item.Service_Lead_Service,
                        item_description = Cipher.Decrypt(item.item_description, password),
                        Date_of_Accord_of_AoN = item.Date_of_Accord_of_AoN,
                        Cost = item.Cost,
                        Categorisation = item.Categorisation,
                        IC_percentage = item.IC_percentage,
                        Trials_Required = item.Trials_Required,
                    };
                    Mmodel1.Add(list);
                }
            }
            return Mmodel1;
        }
    }
}



