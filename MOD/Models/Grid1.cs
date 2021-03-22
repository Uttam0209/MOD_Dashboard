using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class Grid1
    {
        public long IdPrimaryKey { get; set; }
        public Nullable<int> task_id { get; set; }
        public Nullable<int> avg_delay_days { get; set; }
        public string taskDescription { get; set; }
        public string dap_timeline { get; set; }

    }
    public class Grid3
    {
        public Nullable<int> no_of_weeks_airforce { get; set; }
        public Nullable<int> no_of_weeks_army { get; set; }
        public Nullable<int> no_of_weeks_navy { get; set; }
        public string stage_name { get; set; }
        public string stage { get; set; }
        public string dap_timeline { get; set; }

    }
    public class Grid4
    {
        public Nullable<int> no_of_weeks { get; set; }
        public string stage_name { get; set; }
        public string stage { get; set; }
        public string Categorisation { get; set; }
        public string item_description { get; set; }

    }
}