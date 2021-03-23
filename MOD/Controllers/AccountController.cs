using MOD.Models;
using MOD.Service;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MOD.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Login(UserLogin login)
        {
            using (var _context = new MODEntities())
            {

                var isValid = _context.tbl_tbl_User.Where(x => x.InternalEmailID == login.InternalEmailID && x.Password == login.Password).FirstOrDefault();
                if (isValid != null)
                {
                    FormsAuthentication.SetAuthCookie(login.InternalEmailID, false);
                    Session["UserID"] = isValid.UserId;
                    Session["UserName"] = isValid.UserName;
                    Session["SectionID"] = isValid.SectionID;
                    EmailHelper.SendEmail("no-replyacqdashboard@mod.in", "Hi test email");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    //ModelState.AddModelError("", "Invalid UserName and Password");
                    TempData["Message"] = "Login Failed.User Name or Password Doesn't Exist.";
                    return View();
                }
            }

            
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                   
                    using (var _context = new MODEntities())
                    {
                        model.UserId = Convert.ToInt32(Session["UserID"]);
                        var changePassword = _context.tbl_tbl_User.Where(x => x.Password == model.OldPassword && x.UserId == model.UserId).FirstOrDefault();
                        if(changePassword != null)
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
    }
}