using MOD.Models;
using MOD.Service;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using static MOD.MvcApplication;
using ACQ.CoreAPI.Utility.Email;
using DDPAdmin.Services.Master;
using System.Collections.Generic;
using System.Configuration;

namespace MOD.Controllers
{
    //[Route(Account")]
    public class AccountController : Controller
    {
        // GET: Account

        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        private static string WebPortalUrlLogout = ConfigurationManager.AppSettings["WebPortalUrlLogout"].ToString();
        private readonly MODEntities _entities;
        //[Route("Acq")]
        [HandleError]
        public ActionResult Login()
        {
            return Redirect(WebPortalUrl);
            //return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(UserLogin login)
        {
            //string Get_Salt = GeneratedPassword.CreateSalt(10);
            //string hash_String = GeneratedPassword.GenarateHash(login.Password, Get_Salt);
            //using (var _context = new MODEntities())
            //{
            //    string EncryptPass = Cipher.Encrypt_Portal(login.Password);
            //    var isValid = _context.tbl_tbl_User.Where(x => x.InternalEmailID == login.InternalEmailID && x.Password == login.Password).FirstOrDefault();
            //   // var isValid = _context.tbl_tbl_User.Where(x => x.InternalEmailID == login.InternalEmailID && x.Pswd_Salt == EncryptPass).FirstOrDefault();
            //    if (isValid != null)
            //    {
            //        //if (isValid.Pswd_Salt == null)
            //        //{
            //        //    string GenPwd = login.Password;
            //        //    string GetSalt = GeneratedPassword.CreateSalt(10);
            //        //    string hashString = GeneratedPassword.GenarateHash(GenPwd, GetSalt);
            //        //    isValid.Pswd_Salt = hashString;
            //        //    isValid.Flag = "Y";
            //        //    _context.SaveChanges()
            //        //}
            //        FormsAuthentication.SetAuthCookie(login.InternalEmailID, false);
            //        Session["UserID"] = isValid.UserId;
            //        Session["UserName"] = isValid.UserName;
            //        Session["SectionID"] = isValid.SectionID;
            //        //List<tbl_Master_Role> list = (from s in _context.tbl_Master_Role
            //        //            join sl in _context.tbl_Master_Role on s.UserID equals sl.UserID
            //        //                              where sl.UserID == isValid.UserId
            //        //            select s).ToList();
            //        List<tbl_Master_Role> list = _context.tbl_Master_Role.Where(x => x.UserID== isValid.UserId)  .ToList();
            //        Session["RoleList"] = list;
            //        EmailHelper.SendEmail("no-replyacqdashboard@mod.in", "Hi test email");
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //    {
            //        //ModelState.AddModelError("", "Invalid UserName and Password");
            //        TempData["Message"] = "Login Failed.User Name or Password Doesn't Exist.";
            return Redirect(WebPortalUrl);
            //    }
            //}


        }

        [SessionExpire]
        [SessionExpireRefNo]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [SessionExpire]
        [SessionExpireRefNo]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try

                {

                    using (var _context = new MODEntities())
                    {
                        model.UserId = Convert.ToInt32(Session["UserID"]);
                        var changePassword = _context.tbl_tbl_User.Where(x => x.Password == model.OldPassword && x.UserId == model.UserId).FirstOrDefault();
                        if (changePassword != null)
                        {
                            changePassword.Password = model.NewPassword;
                            _context.SaveChanges();
                            TempData["ChangePassword"] = "Change Password Successfully";
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            return Redirect(WebPortalUrlLogout);

        }
        [Route("LoginBlockMsg")]
        public ActionResult LoginBlockMsg()
        {
            Session.Abandon();
            Session.RemoveAll();
            return View();
        }
        [HttpGet]
        [Route("Blockuser")]
        public string Blockuser(string EmailID)
        {
            string message = string.Empty;
            using (var _context = new MODEntities())
            {
                var _isUser = _context.tbl_trn_OTP.Where(x => x.EmailID == EmailID).FirstOrDefault();

                if (_isUser != null)
                {
                    if (_isUser.OtpCount >= 5)
                    {
                        message = "Blocked";
                    }
                    else
                    {
                        var _isUserDetail = _context.tbl_tbl_User.Where(x => x.InternalEmailID == EmailID).FirstOrDefault();
                        if (_isUserDetail.LoginCount >= 5)
                        {
                            message = "Blocked";
                        }
                        else
                        {
                            message = "Allow";
                        }
                    }
                }
            }

            return message;
        }
    }
}