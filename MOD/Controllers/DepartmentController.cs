using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gantt_Chart.Models;
using MOD.Models;
using MOD.Service;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    [Authorize]
    [SessionExpire]
    [SessionExpireRefNo]
    public class DepartmentController : Controller
    {
        MODEntities _entities = new MODEntities();

        public DepartmentController()
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
                if (BruteForceAttackss.bcontroller == "Department")
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
                BruteForceAttackss.bcontroller = "Department";
            }
        }

        [Route("Department")]
        public ActionResult Index()
        {
            DepartmentListViewModel model = new DepartmentListViewModel();
            model.Departments = _entities.acq_department_master.ToList();
            return View(model);
        }
        [Route("DCreate")]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("DCreate")]
        public ActionResult Create(DepartmentSaveViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    acq_department_master obj = new acq_department_master();
                    obj.deptt_description = model.DepartmentName;
                    _entities.acq_department_master.Add(obj);
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
        [Route("DEdit")]
        public ActionResult Edit(int ID)
        {
            try
            {
                var _editAonData = _entities.acq_department_master.Where(x => x.deptt_id == ID).FirstOrDefault();
                DepartmentSaveViewModel model = new DepartmentSaveViewModel();
                model.DeptId = _editAonData.deptt_id;
                model.DepartmentName = _editAonData.deptt_description;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("DUpdate")]
        public ActionResult Update(DepartmentSaveViewModel model)
        {
            try
            {
                var _updateAon = _entities.acq_department_master.Where(x => x.deptt_id == model.DeptId).FirstOrDefault();
                if (_updateAon != null)
                {
                    _updateAon.deptt_description = model.DepartmentName;
                    //_updateAon.Location = model.Location;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("Index");
        }

        [Route("DDelete")]
        public ActionResult Delete(int ID)
        {
            try
            {
                var _deleteDept = _entities.acq_department_master.Where(x => x.deptt_id == ID).FirstOrDefault();
                if (_deleteDept != null)
                {
                    _entities.acq_department_master.Remove(_deleteDept);
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