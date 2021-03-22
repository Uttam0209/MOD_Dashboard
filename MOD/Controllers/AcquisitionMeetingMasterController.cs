using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MOD.Models;

namespace MOD.Controllers
{
    [Authorize]
    public class AcquisitionMeetingMasterController : Controller
    {
        MODEntities _entities = new MODEntities();
        public ActionResult Index()
        {
            AcquisitionMeetingMasterListViewModel model = new AcquisitionMeetingMasterListViewModel();
            model.Meeting_MasterList = _entities.acq_meeting_master.Where(x=>x.Deleted == false).ToList();
            return View(model);
        }

        public ActionResult Create()
        {
            AcquisitionCreateMasterViewModel model = new AcquisitionCreateMasterViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult Create(AcquisitionCreateMasterViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    acq_meeting_master obj = new acq_meeting_master();
                    obj.dac_dpb = model.dac_dpb;
                    obj.meeting_date = model.meeting_date;
                    if(model != null)
                    {
                        obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    }
                    else
                    {
                        obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    }
                    obj.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    obj.Remarks = model.Remarks;
                    obj.CreatedBy = Convert.ToInt32(Session["UserID"]);
                    obj.CreatedOn = System.DateTime.Now;
                    obj.Deleted = false;
                    _entities.acq_meeting_master.Add(obj);
                    _entities.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View();
        }

        public ActionResult Edit(int ID)
        {
            try
            {
                var _editAcqMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == ID).FirstOrDefault();
                AcquisitionCreateMasterViewModel model = new AcquisitionCreateMasterViewModel();
                model.meeting_id = _editAcqMeeting.meeting_id;
                model.dac_dpb = _editAcqMeeting.dac_dpb;
                model.meeting_date = _editAcqMeeting.meeting_date;
                model.Date_of_Issue_of_Minutes = _editAcqMeeting.Date_of_Issue_of_Minutes;
                model.Remarks = _editAcqMeeting.Remarks;
                return View(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Update(AcquisitionCreateMasterViewModel model)
        {
            try
            {
                var _updateAcqMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == model.meeting_id).FirstOrDefault();
                if (_updateAcqMeeting != null)
                {

                    _updateAcqMeeting.dac_dpb = model.dac_dpb;
                    _updateAcqMeeting.meeting_date = model.meeting_date;
                    _updateAcqMeeting.Date_of_Issue_of_Minutes = model.Date_of_Issue_of_Minutes;
                    _updateAcqMeeting.Remarks = model.Remarks;
                    _updateAcqMeeting.CreatedBy = Convert.ToInt32(Session["UserID"]); ;
                    _updateAcqMeeting.CreatedOn = System.DateTime.Now;
                    _updateAcqMeeting.Deleted = false;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int ID)
        {
            try
            {
                var _deleteMeeting = _entities.acq_meeting_master.Where(x => x.meeting_id == ID).FirstOrDefault();
                if(_deleteMeeting != null)
                {
                    _deleteMeeting.DeletedBy = Convert.ToInt32(Session["UserID"]);
                    _deleteMeeting.DeletedOn = System.DateTime.Now;
                    _deleteMeeting.Deleted = true;
                    _entities.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RedirectToAction("Index");
        }
    }
}