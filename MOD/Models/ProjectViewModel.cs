using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class ProjectViewModel
    {
        public long ProjectId { get; set; }
        public Nullable<long> TempleteId { get; set; }
        public string ProjectName { get; set; }
        public string TemplateType { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CreateOn { get; set; }
        public string CreatedBy { get; set; }
        public string Message { get; set; }
        public string IsActive { get; set; }
        public Nullable<System.DateTime> RecInsTime { get; set; }
    }
}