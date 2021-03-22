using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MOD.Models
{
    public class DepartmentListViewModel
    {
        public List<acq_department_master> Departments { get; set; }
    }

    public class DepartmentSaveViewModel
    {
        public int DeptId { get; set; }
        [Required(ErrorMessage ="Enter Department Name")]
        public string DepartmentName { get; set; }
        [Required(ErrorMessage = "Enter Location")]
        public string Location { get; set; }
    }
}