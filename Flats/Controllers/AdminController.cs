using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Flats.Models;
using Flats.Views;
using System.Web;
using System.Drawing;
using Flats.Views.Manage;

namespace Flats.Controllers
{
    [Authorize(Roles="Administrator")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            dbDataContext db = new dbDataContext();
            List<Settings> lst = db.Settings.Select(c => c).OrderBy(c => c.setting_key).ToList();
            
            
            return View(lst);
        }

        [HttpGet]
        public ActionResult CreateSetting()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateSetting(Settings model)
        {
            dbDataContext db = new dbDataContext();

            db.Settings.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditSetting(int id=0)
        {
            if (id == 0)
                return RedirectToAction("Index");
            dbDataContext db = new dbDataContext();
            Settings sett = db.Settings.SingleOrDefault(c => c.id == id);
            if (sett == null)
            {
                return RedirectToAction("Index");
            }
            return View(sett);
        }
        [HttpPost]
        public ActionResult EditSetting(Settings model)
        {
            dbDataContext db = new dbDataContext();
            Settings sett = db.Settings.SingleOrDefault(c => c.id == model.id);
            if (sett == null)
            {
                return RedirectToAction("Index");
            }
            sett.setting_key = model.setting_key;
            sett.setting_value = model.setting_value;

            db.SubmitChanges();
            return RedirectToAction("Index");
        }
        public ActionResult DeleteSetting(int id) 
        {
            dbDataContext db = new dbDataContext();
            Settings sett = db.Settings.SingleOrDefault(c => c.id == id);
            if (sett != null)
            {
                db.Settings.DeleteOnSubmit(sett);
                db.SubmitChanges();
            }
            return RedirectToAction("Index");    
        }

        public ActionResult PagesList()
        {
            dbDataContext db = new dbDataContext();
            List<Pages> lst = db.Pages.Select(c => c).OrderBy(c => c.Naim).ToList();

            return View(lst);
        }

        [HttpGet]
        public ActionResult CreatePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePage(Pages model)
        {
            dbDataContext db = new dbDataContext();

            db.Pages.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("PagesList");
        }
    }
}