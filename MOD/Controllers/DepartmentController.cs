using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOD.Models;

namespace MOD.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        MODEntities _entities = new MODEntities();
        public ActionResult Index()
        {
            DepartmentListViewModel model = new DepartmentListViewModel();
            model.Departments = _entities.acq_department_master.ToList();
            return View(model);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
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