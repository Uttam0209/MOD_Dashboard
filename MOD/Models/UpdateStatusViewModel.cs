using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class UpdateStatusViewModel
    {
        public long UpdateStatusTaskId { get; set; }
        public Nullable<long> TryPojectID { get; set; }
        public Nullable<long> ProjectId { get; set; }
        public string TaskSlno { get; set; }
        public Nullable<System.DateTime> ActuaStartDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> ActuaEndDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<long> UserID { get; set; }
        public string Remark { get; set; }
        public string TaskDescription { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> RecTime { get; set; }
    }
}