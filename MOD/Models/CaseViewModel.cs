using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gantt_Chart.Models
{
    public class CaseViewModel
    {
        public int id { get; set; }
        public int BF { get; set; }
        public int Cases { get; set; }
        public int Casesdisposed { get; set; }
        public int Outstanding { get; set; }
        public string mtaskid { get; set; }
        //  public string stageid { get; set; }
    }
}