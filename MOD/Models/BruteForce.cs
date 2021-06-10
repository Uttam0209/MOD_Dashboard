using Gantt_Chart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOD.Models
{
    public class BruteForce
    {
        MODEntities _entities = new MODEntities();
        public List<UserViewModel> GetUserLoginBlock(string emailId)
        {
            List<UserViewModel> model = new List<UserViewModel>();
            List<vw_userDetail> getUserDetail = new List<vw_userDetail>();

            UserViewModel list = new UserViewModel();


            var _isUser = _entities.tbl_tbl_User.Where(x => x.InternalEmailID == emailId).FirstOrDefault();

            if (_isUser != null)
            {
                _isUser.LoginCount = 5;
                _entities.SaveChanges();
                list.Message = "Blocked";
            }

            model.Add(list);

            return model;

        }
    }
}