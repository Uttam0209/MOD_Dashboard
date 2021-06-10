using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class UserListViewModel
    {
        //public List<tbl_tbl_User> UserList { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTill { get; set; }
        public string IPAddress { get; set; }
        public string Designation { get; set; }
        public List<UserListViewModel> UserList { get; set; }
        public virtual acq_department_master tbl_tblDepartment { get; set; }
    }

    public class UserSaveViewModel
    {
        public int UserId { get; set; }
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Enter  Internal EmailID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string InternalEmailID { get; set; }
        [Required(ErrorMessage = "Enter  External EmailID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string ExternalEmailID { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Select Department")]
        public int? DepartmentID { get; set; }
        [Required(ErrorMessage = "Enter ValidFrom")]
        public DateTime? ValidFrom { get; set; }
        [Required(ErrorMessage = "Enter ValidTill")]
        public DateTime? ValidTill { get; set; }
        //[Required(ErrorMessage = "Enter IP Address")]
        public string IPAddress { get; set; }
        //[Required(ErrorMessage = "Enter Mac Address")]
        public string MacAddress { get; set; }
        public int? UserTypeId { get; set; }
        public int? ReportingTo { get; set; }
        [Required(ErrorMessage = "Enter Designation")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Enter Rank User")]
        public string RankUser { get; set; }
        [Required(ErrorMessage = "Select Login Allowed")]
        public string LoginAllowed { get; set; }
        [Required(ErrorMessage = "Select Section")]
        public int? SectionID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsDeleted { get; set; }
        public string Pswd_Salt { get; set; }
        public string Flag { get; set; }
        public string Temp_Number { get; set; }
        public int? LoginCount { get; set; }
        public DateTime? ExpiryTime { get; set; }
        public string TokenId { get; set; }

        public List<acq_department_master> departmentList { get; set; }
        public List<acq_section_master> SectionMasterList { get; set; }
        public string Message { get; set; }
    }

    public class UserLogin
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Enter EmailID")]
        [EmailAddress(ErrorMessage = "Enter Valid Email")]
        public string InternalEmailID { get; set; }
        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class ChangePasswordViewModel
    {

        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
  

