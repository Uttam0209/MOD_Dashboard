using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDPAdmin.Services.Master;
using MOD.Models;

namespace MOD.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        MODEntities _entities = new MODEntities();
        string password = "p@SSword";
        public ActionResult Index()
        {
            List<UserListViewModel> list = new List<UserListViewModel>();
            UserListViewModel model = new UserListViewModel();
            var userList = _entities.tbl_tbl_User.Where(x => x.IsDeleted == false).ToList();
            if (userList != null)
            {
                foreach (var item in userList)
                {
                    UserListViewModel obj = new UserListViewModel();
                    obj.UserId = item.UserId;
                    obj.UserName = item.UserName;
                    obj.UserEmail = item.InternalEmailID;
                    obj.Phone = item.Phone;
                    obj.DepartmentName = item.acq_department_master.deptt_description;
                    obj.UserTypeName = item.acq_section_master.section_descr;
                    obj.Designation = item.Designation;
                    obj.ValidFrom = item.ValidFrom;
                    obj.ValidTill = item.ValidTill;
                    //obj.IPAddress = item.IPAddress;
                    list.Add(obj);
                }
                model.UserList = list;
            }
            return View(model);
        }

        public ActionResult Create()
        {
            UserSaveViewModel model = new UserSaveViewModel();
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(UserSaveViewModel model)
        {
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    tbl_tbl_User obj = new tbl_tbl_User();
                    obj.UserName = model.UserName;
                    obj.InternalEmailID = model.InternalEmailID;
                    obj.ExternalEmailID = model.ExternalEmailID;
                    obj.Password = model.Password;
                    obj.RankUser = model.RankUser;
                    obj.Phone = model.Phone;
                    obj.DepartmentID = model.DepartmentID;
                    obj.SectionID = model.SectionID;
                    obj.ValidFrom = Convert.ToDateTime(model.ValidFrom);
                    obj.ValidTill = Convert.ToDateTime(model.ValidTill);
                    obj.IPAddress = model.IPAddress;
                    obj.MacAddress = model.MacAddress;
                    obj.Designation = model.Designation;
                    obj.LoginAllowed = model.LoginAllowed;
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.IsDeleted = false;
                    _entities.tbl_tbl_User.Add(obj);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(model);
        }
        public ActionResult Edit(int ID)
        {
            try
            {
                var _editUser = _entities.tbl_tbl_User.Where(x => x.UserId == ID).FirstOrDefault();
                UserSaveViewModel model = new UserSaveViewModel();
                model.UserId = _editUser.UserId;
                model.UserName = _editUser.UserName;
                model.InternalEmailID = _editUser.InternalEmailID;
                model.ExternalEmailID = _editUser.ExternalEmailID;
                model.Password = _editUser.Password; 
                model.RankUser = _editUser.RankUser;
                model.Phone = _editUser.Phone;
                model.SectionID = _editUser.SectionID;
                model.DepartmentID = _editUser.DepartmentID;
                model.ValidFrom = _editUser.ValidFrom;
                //model.ValidFrom =Convert.ToDateTime(model.ValidFrom.Value.ToString("yyyy-MM-dd"));
                model.ValidTill = _editUser.ValidTill;
                model.Designation = _editUser.Designation;
                model.IPAddress = _editUser.IPAddress;
                model.MacAddress = _editUser.MacAddress;
                model.LoginAllowed = _editUser.LoginAllowed;
                model.departmentList = _entities.acq_department_master.ToList();
                model.SectionMasterList = _entities.acq_section_master.ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Update(UserSaveViewModel model)
        {
            try
            {
                var _updateUser = _entities.tbl_tbl_User.Where(x => x.UserId == model.UserId).FirstOrDefault();
                if (_updateUser != null)
                {
                    _updateUser.UserName = model.UserName;
                    _updateUser.InternalEmailID = model.InternalEmailID;
                    _updateUser.ExternalEmailID = model.ExternalEmailID;
                    _updateUser.Password = model.Password;
                    _updateUser.RankUser = model.RankUser;
                    _updateUser.IPAddress = model.IPAddress;
                    _updateUser.MacAddress = model.MacAddress;
                    _updateUser.Phone = model.Phone;
                    _updateUser.SectionID = model.UserTypeId;
                    _updateUser.DepartmentID = model.DepartmentID;
                    _updateUser.ValidFrom = Convert.ToDateTime(model.ValidFrom);
                    _updateUser.ValidTill = Convert.ToDateTime(model.ValidTill);
                    _updateUser.Designation = model.Designation;
                    _updateUser.LoginAllowed = model.LoginAllowed;
                    _updateUser.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    _updateUser.CreatedOn = System.DateTime.Now;
                    _updateUser.IsDeleted = false;
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
                var _deleteUser = _entities.tbl_tbl_User.Where(x => x.UserId == ID).FirstOrDefault();
                if (_deleteUser != null)
                {
                    _deleteUser.IsDeleted = true;
                    _deleteUser.DeletedBy = Convert.ToInt32(Session["UserID"]);
                    _deleteUser.DeletedOn = System.DateTime.Now;
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