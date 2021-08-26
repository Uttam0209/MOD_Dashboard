using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Gantt_Chart.Models
{
    [DataContract]
    public class DetailCharts
    {
        public DetailCharts(string label, double y, double z, string aonid = "")
        {
            this.Label = label;
            this.Y = y;
            this.Z = z;
            this.aonid = aonid.ToString();
        }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "label")]
        public string Label = "";
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> Y = null;

        [DataMember(Name = "z")]
        public Nullable<double> Z = null;

        [DataMember(Name = "aonid")]
        public string aonid = null;


        public DetailCharts(string label, double y)
        {
            this.Label = label;
            this.Y = y;
        }
    }
    public partial class AoNsGranted_WiseReport
    {
        public long Pkey { get; set; }
        public string Financial_year { get; set; }
        public Nullable<int> no_of_aons { get; set; }
        public Nullable<decimal> total_cost_in_crs { get; set; }
        public string Categorisation { get; set; }
    }

}