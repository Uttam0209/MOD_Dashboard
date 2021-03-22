using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class applicabilityViewModel
    {
        public int Id { get; set; }
        public Nullable<int> AoN { get; set; }
        public string TaskSlno { get; set; }
        public string applicable { get; set; }
        public string Remarks { get; set; }
        public string item_description { get; set; }
        public List<applicabilityViewModel> ProjectList { get; set; }
        public List<MODListViewModel> StageList { get; set; }
    }
}