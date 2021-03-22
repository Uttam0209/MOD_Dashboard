using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class tryProjectViewModel
    {
        public long TryPojectID { get; set; }
        public Nullable<long> ProjectId { get; set; }
        public string Template_type { get; set; }
        public string TaskSlno { get; set; }
        public string TaskDescription { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Dependencies { get; set; }
        public int super_task_id { get; set; }
        public Nullable<long> Responsibility { get; set; }
        public string IsActive { get; set; }
        public string Message { get; set; }
    }
}