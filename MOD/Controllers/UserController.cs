using ACQ.CoreAPI.Utility.Email;
using DDPAdmin.Services.Master;
using Ganss.XSS;
using Gantt_Chart.Models;
using MOD.Models;
using MOD.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
    
    [SessionExpire]
    [SessionExpireRefNo]
    public class UserController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        MODEntities _entities = new MODEntities();
        string password = "p@SSword";
        public UserController()
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
            BruteForceAttackss.bcontroller = "User";
            if (BruteForceAttackss.bcontroller != "")
            {
               
                if (BruteForceAttackss.bcontroller == "User")
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
                BruteForceAttackss.bcontroller = "User";
            }
        }
        [Route("Index")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "User Master".ToLower())
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
            List<UserListViewModel> list = new List<UserListViewModel>();
            UserListViewModel model = new UserListViewModel();
            var userList = _entities.tbl_tbl_User.Where(x => x.IsDeleted == false).ToList();
            if (userList != null)
            {
                foreach (var item in userList)
                {
                    UserListViewModel obj = new UserListViewModel();
                    obj.UserId = Cipher.Encrypt_Portal(item.UserId.ToString());
                    obj.UserName = sanitizer.Sanitize(item.UserName);
                    obj.UserEmail = sanitizer.Sanitize(item.InternalEmailID);
                    obj.Phone = sanitizer.Sanitize(item.Phone);
                    obj.DepartmentName = sanitizer.Sanitize(item.acq_department_master.deptt_description);
                    obj.UserTypeName = sanitizer.Sanitize(item.acq_section_master.section_descr);
                    obj.Designation = sanitizer.Sanitize(item.Designation);
                    obj.ValidFrom = item.ValidFrom;
                    obj.ValidTill = item.ValidTill;
                    //obj.IPAddress = item.IPAddress;
                    list.Add(obj);
                }
                model.UserList = list;
            }
            return View(model);
        }
        [Route("EscalationmatrixIndex")]
        public ActionResult EscalationmatrixIndex()
        {
            List<UserListViewModel> list = new List<UserListViewModel>();
            UserListViewModel model = new UserListViewModel();
            var userList = _entities.tbl_tbl_User.Where(x => x.IsDeleted == false && (x.DepartmentID == 2 || x.DepartmentID == 3 || x.DepartmentID == 4 || x.DepartmentID == 5 || x.DepartmentID == 6)).ToList();
            if (userList != null)
            {
                foreach (var item in userList)
                {
                    UserListViewModel obj = new UserListViewModel();
                    obj.UserId = item.UserId.ToString();
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
        [Route("UEscalationmatrix")]
        public ActionResult Escalationmatrix()
        {
            UserSaveViewModel model = new UserSaveViewModel();
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            return View(model);
        }
        [HttpPost]
        [Route("UEscalationmatrix")]
        public ActionResult Escalationmatrix(UserSaveViewModel model)
        {
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    string GenPwd1 = model.Password;
                    string GetSalt = GeneratedPassword.CreateSalt(10);
                    string hashString = GeneratedPassword.GenarateHash(GenPwd1, GetSalt);

                    tbl_tbl_User obj = new tbl_tbl_User();

                    obj.UserName = model.UserName;
                    obj.InternalEmailID = model.InternalEmailID;
                    obj.ExternalEmailID = model.ExternalEmailID;
                    obj.Password = null;
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
                    obj.Pswd_Salt = Encryption.Encrypt(GenPwd1);
                    obj.Flag = "Y";
                    obj.LoginCount = 0;
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
        [Route("UCreate")]
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {

                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "User Master".ToLower())
                    {
                      //  if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 1)
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
            UserSaveViewModel model = new UserSaveViewModel();
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            return View(model);
        }
        [HttpPost]
        [Route("UCreate")]
        public ActionResult Create(UserSaveViewModel model)
        {
            model.departmentList = _entities.acq_department_master.ToList();
            model.SectionMasterList = _entities.acq_section_master.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    string GenPwd1 = model.Password;
                    string GetSalt = GeneratedPassword.CreateSalt(10);
                    string hashString = GeneratedPassword.GenarateHash(GenPwd1, GetSalt);

                    tbl_tbl_User obj = new tbl_tbl_User();

                    obj.UserName = model.UserName;
                    obj.InternalEmailID = model.InternalEmailID;
                    obj.ExternalEmailID = model.ExternalEmailID;
                    obj.Password = null;
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
                    obj.Pswd_Salt = Encryption.Encrypt(GenPwd1);
                    obj.Flag = "Y";
                    obj.LoginCount = 0;
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
        [Route("UEdit")]
        public ActionResult Edit(string ID)
        {
            int U_ID = 0;
            ID = Cipher.Decrypt_Portal(ID);
            U_ID = Convert.ToInt32(ID);
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "User Master".ToLower())
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
                var _editUser = _entities.tbl_tbl_User.Where(x => x.UserId == U_ID).FirstOrDefault();
                UserSaveViewModel model = new UserSaveViewModel();
                model.UserId = _editUser.UserId;
                model.UserName = _editUser.UserName;
                model.InternalEmailID = _editUser.InternalEmailID;
                model.ExternalEmailID = _editUser.ExternalEmailID;
                model.Password = null;
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
        // [Route("UUpdate")]
        public ActionResult Update(UserSaveViewModel model)
        {
            string GenPwd1 = model.Password;
            string GetSalt = GeneratedPassword.CreateSalt(10);
            string hashString = GeneratedPassword.GenarateHash(GenPwd1, GetSalt);

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
                    _updateUser.Pswd_Salt = hashString;
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

        [Route("UDelete")]
        public ActionResult Delete(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "User Master".ToLower())
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