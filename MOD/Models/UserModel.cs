using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class UserModel
    {
        public long UserID { get; set; }
        public long DepartID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public bool IsFirst { get; set; }
        public string Category { get; set; }
        public string IsActive { get; set; }
        public Nullable<int> LoginCount { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string TempRefNo { get; set; }
        public Nullable<System.DateTime> RecInsTime { get; set; }
    }

    public class UserViewModel
    {
        //public List<tbl_tbl_User> UserList { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public Nullable<int> DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int? UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public string InternalEmailID { get; set; }
        public string ExternalEmailID { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTill { get; set; }
        public string IPAddress { get; set; }
        public string Designation { get; set; }
        public string Emailotp { get; set; }
        public string Pswd_Salt { get; set; }
        public string Flag { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string deptt_description { get; set; }
        public int? SectionID { get; set; }

        public Nullable<int> SrNo { get; set; }
        public string IsActive { get; set; }
        public string Message { get; set; }

        public List<UserViewModel> UserList { get; set; }
        //public virtual acq_department_master tbl_tblDepartment { get; set; }
    }
}