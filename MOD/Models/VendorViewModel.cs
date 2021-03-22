using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MOD.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MOD.Models
{
    public class VendorListViewModel
    {
        public List<tbl_tblVendor> Vendors { get; set; }
        public int VendorId { get; set; }
        public Nullable<int> VendorCategory { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorContactName { get; set; }
        public string VendorPhone { get; set; }
        public string VendorEmail { get; set; }
        public string CategoryName { get; set; }
        public virtual tbl_tblVendorCategory tbl_tblVendorCategory { get; set; }

        public List<VendorListViewModel> VendorList { get; set; }
    }

    public class VendorSaveViewModel
    {
        public VendorSaveViewModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }
        public int VendorId { get; set; }

        [Required(ErrorMessage = "Enter Vendor Category")]
        public int VendorCategory { get; set; }
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Enter Vendor Name")]
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorContactName { get; set; }
        public string VendorPhone { get; set; }
        [EmailAddress (ErrorMessage ="Enter Valid Email")]
        public string VendorEmail { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public List<tbl_tblVendorCategory> VendorCategories { get; set; }
        public virtual tbl_tblVendorCategory tbl_tblVendorCategory { get; set; }
        public List<SelectListItem> AvailableCategories { get; set; }
    }
}