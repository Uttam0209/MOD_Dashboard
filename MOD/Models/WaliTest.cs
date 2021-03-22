using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class WaliTest
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Template_type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime AoNDate { get; set; }
        public string CreatedBy { get; set; }
        public string IsActive { get; set; }
        public string categorisation { get; set; }
        public string item_description { get; set; }
        public string service { get; set; }
        public string  type { get; set; }
        public Nullable<System.DateTime> scheduled_date_of_completion { get; set; }
        public Nullable<int> actual_no_of_days { get; set; }
        public Nullable<int> scheduled_no_of_days { get; set; }
        public Nullable<decimal> percent_delay { get; set; }


    }
}