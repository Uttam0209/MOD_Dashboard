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
    public class VendorController : Controller
    {
        MODEntities _entities = new MODEntities();
        public ActionResult Index()
        {
            var password = "p@SSword";
            VendorListViewModel model = new VendorListViewModel();
            List<VendorListViewModel> list= new List<VendorListViewModel>();
            var vendorList=_entities.tbl_tblVendor.Where(x=>x.IsDeleted == false).ToList();
            foreach(var item in vendorList)
            {
                VendorListViewModel obj = new VendorListViewModel();
                obj.VendorId = item.VendorId;
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

        public ActionResult Create()
        {
            VendorSaveViewModel model = new VendorSaveViewModel();
            model.VendorCategories =  _entities.tbl_tblVendorCategory.ToList();
            return View(model);
        }

        [HttpPost]
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
                    obj.VendorAddress = model.VendorAddress;
                    obj.VendorCategory = model.VendorCategory;
                    obj.VendorContactName = model.VendorContactName;
                    obj.VendorPhone = model.VendorPhone;
                    obj.VendorEmail = model.VendorEmail;
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

        public ActionResult Edit(int ID)
        {
            try
            {
                var password = "p@SSword";
                var _editVendor = _entities.tbl_tblVendor.Where(x => x.VendorId == ID).FirstOrDefault();
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
                    _updateVendor.VendorAddress = model.VendorAddress;
                    _updateVendor.VendorContactName = model.VendorContactName;
                    _updateVendor.VendorPhone = model.VendorPhone;
                    _updateVendor.VendorEmail = model.VendorEmail;
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
            return Redirect("Index");
        }

        public ActionResult Delete(int ID)
        {
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