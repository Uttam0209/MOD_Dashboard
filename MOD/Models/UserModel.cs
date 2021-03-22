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
}