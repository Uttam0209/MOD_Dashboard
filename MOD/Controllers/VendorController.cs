using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DDPAdmin.Services.Master;
using Ganss.XSS;
using Gantt_Chart.Models;
using MOD.Models;
using MOD.Service;
using static MOD.MvcApplication;

namespace MOD.Controllers
{
   
    [SessionExpire]
    [SessionExpireRefNo]
    public class VendorController : Controller
    {
        private static string WebPortalUrl = ConfigurationManager.AppSettings["WebPortalUrl"].ToString();
        HtmlSanitizer sanitizer = new HtmlSanitizer();
        MODEntities _entities = new MODEntities();

        public VendorController()
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
            BruteForceAttackss.bcontroller = "Vendor";
            if (BruteForceAttackss.bcontroller != "")
            {
                if (BruteForceAttackss.bcontroller == "Vendor")
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
                BruteForceAttackss.bcontroller = "Vendor";
            }
        }
        //[Route("VIndex")]
        public ActionResult Index()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Vendor Master".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
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
            var password = "p@SSword";
            VendorListViewModel model = new VendorListViewModel();
            List<VendorListViewModel> list= new List<VendorListViewModel>();
            var vendorList=_entities.tbl_tblVendor.Where(x=>x.IsDeleted == false).ToList();
            foreach(var item in vendorList)
            {
                VendorListViewModel obj = new VendorListViewModel();
                obj.VendorId = Cipher.Encrypt_Portal(item.VendorId.ToString());
                obj.VendorName = Cipher.Decrypt(item.VendorName, password);
                //obj.VendorName = Encryption.Decrypt(item.VendorName);
                obj.VendorAddress = item.VendorAddress;
                obj.VendorContactName = item.VendorContactName;
                obj.VendorPhone = item.VendorPhone;
                obj.VendorEmail = item.VendorEmail;
                obj.CategoryName = item.tbl_tblVendorCategory.VendorCategoryName;
               
                list.Add(obj);
            }
            ViewBag.msg = "";
            model.VendorList = list;
            if (TempData["ID"] != null)
            {
                ViewBag.msg = TempData["ID"].ToString();
            }
            return View(model);
        }

        [Route("VCreate")]
        public ActionResult Create()
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Vendor Master".ToLower())
                    {
                       // if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
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
            VendorSaveViewModel model = new VendorSaveViewModel();
            model.VendorCategories =  _entities.tbl_tblVendorCategory.ToList();
            return View(model);
        }

       
        [HttpPost]
        [Route("VCreate")]
        public ActionResult Create(VendorSaveViewModel model)
        {
            model.VendorCategories = _entities.tbl_tblVendorCategory.ToList();
            if (ModelState.IsValid)
            {
                var password = "p@SSword";
                try
                {
                    tbl_tblVendor obj = new tbl_tblVendor();

                    //Encrypted SHA Vendor Name
                    string vendorName =Cipher.Encrypt(model.VendorName, password);
                    obj.VendorName = vendorName;
                    ////Encrypted Vendor Name
                    //string vendorName = Encryption.Encrypt(model.VendorName);
                    //obj.VendorName = vendorName;
                    obj.VendorAddress = sanitizer.Sanitize(model.VendorAddress);
                    obj.VendorCategory = model.VendorCategory;
                    obj.VendorContactName = sanitizer.Sanitize(model.VendorContactName);
                    obj.VendorPhone = sanitizer.Sanitize(model.VendorPhone);
                    obj.VendorEmail = sanitizer.Sanitize(model.VendorEmail);
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.IsDeleted = false;
                    _entities.tbl_tblVendor.Add(obj);
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

        [Route("VEdit")]
        public ActionResult Edit(string ID)
        {
            int VId = 0;
            ID = Cipher.Decrypt_Portal(ID);
            VId = Convert.ToInt32(ID);
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Vendor Master".ToLower())
                    {
                        //if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
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
                var password = "p@SSword";
                var _editVendor = _entities.tbl_tblVendor.Where(x => x.VendorId == VId).FirstOrDefault();
                VendorSaveViewModel model = new VendorSaveViewModel();
                model.VendorId = _editVendor.VendorId;
                model.VendorName = Cipher.Decrypt(_editVendor.VendorName, password);
                model.VendorCategory = _editVendor.VendorCategory;
                //model.VendorName = Encryption.Decrypt(_editVendor.VendorName);
                model.VendorAddress = _editVendor.VendorAddress;
                model.VendorContactName = _editVendor.VendorContactName;
                model.VendorPhone = _editVendor.VendorPhone;
                model.VendorEmail = _editVendor.VendorEmail;
                model.VendorCategories = _entities.tbl_tblVendorCategory.ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("VUpdate")]
        [HttpPost]
        public ActionResult Update(VendorSaveViewModel model)
        {
            try
            {
                var password = "p@SSword";
                var _updateVendor = _entities.tbl_tblVendor.Where(x => x.VendorId == model.VendorId).FirstOrDefault();
                if (_updateVendor != null)
                {
                    _updateVendor.VendorCategory = model.VendorCategory;
                    //_updateVendor.VendorName = Encryption.Encrypt(model.VendorName);
                    _updateVendor.VendorName = Cipher.Encrypt(model.VendorName, password);
                    _updateVendor.VendorAddress = sanitizer.Sanitize(model.VendorAddress);
                    _updateVendor.VendorContactName = sanitizer.Sanitize(model.VendorContactName);
                    _updateVendor.VendorPhone = sanitizer.Sanitize(model.VendorPhone);
                    _updateVendor.VendorEmail = sanitizer.Sanitize(model.VendorEmail);
                    _updateVendor.CreatedBy = Convert.ToInt32(Session["UserID"]); ;
                    _updateVendor.CreatedOn = System.DateTime.Now;
                    _updateVendor.IsDeleted = false;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
           
        }
        [Route("VDelete")]
        public ActionResult Delete(int ID)
        {
            if (Convert.ToInt32(Session["SectionID"]) != 13)
            {
                List<tbl_Master_Role> RoleList = (List<tbl_Master_Role>)Session["RoleList"];
                bool isAccessible = false;
                foreach (var item in RoleList)
                {
                    if (item.FormName.ToLower() == "Vendor Master".ToLower())
                    {
                        //if (Convert.ToInt32(Session["SectionID"]) == 13 || Convert.ToInt32(Session["SectionID"]) == 8 || Convert.ToInt32(Session["SectionID"]) == 9 || Convert.ToInt32(Session["SectionID"]) == 10)
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
                var isExit = _entities.acq_rfp_VendorsDetails.Where(x => x.vendor_id == ID).FirstOrDefault();
                if (isExit == null)
                {
                    var _deleteVendor = _entities.tbl_tblVendor.Where(x => x.VendorId == ID).FirstOrDefault();
                    if (_deleteVendor != null)
                    {
                        _deleteVendor.IsDeleted = true;
                        _deleteVendor.DeletedBy = Convert.ToInt32(Session["UserID"]); ;
                        _deleteVendor.DeletedOn = System.DateTime.Now;
                        _entities.SaveChanges();
                    }
                }
                else
                {
                    TempData["ID"] = "Vendor can not be deleted if already present in an RFP";
                    
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