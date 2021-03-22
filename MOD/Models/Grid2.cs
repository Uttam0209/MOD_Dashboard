using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class Grid2
    {
        public int IdPrimaryKey { get; set; }
        public Nullable<decimal> stage { get; set; }
        public Nullable<int> army_projects { get; set; }
        public Nullable<int> iaf_projects { get; set; }
        public Nullable<int> navy_projects { get; set; }
        public Nullable<int> other_projects { get; set; }
        public string TaskDescription { get; set; }

    }
}