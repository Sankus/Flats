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
using System.IO;
using System.Data.Linq;
using System.Collections;

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
        [ValidateInput(false)]
        public ActionResult CreatePage(Pages model)
        {
            dbDataContext db = new dbDataContext();

            db.Pages.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("PagesList");
        }
        public ActionResult DeletePage(int id)
        {
            dbDataContext db = new dbDataContext();
            Pages sett = db.Pages.SingleOrDefault(c => c.id == id);
            if (sett != null)
            {
                db.Pages.DeleteOnSubmit(sett);
                db.SubmitChanges();
            }
            return RedirectToAction("PagesList");
        }

        [HttpGet]
        public ActionResult EditPage(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("PagesList");
            dbDataContext db = new dbDataContext();
            Pages page = db.Pages.SingleOrDefault(c => c.id == id);
            if (page == null)
            {
                return RedirectToAction("PagesList");
            }
            return View(page);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditPage(Pages model)
        {
            dbDataContext db = new dbDataContext();
            Pages page = db.Pages.SingleOrDefault(c => c.id == model.id);
            if (page == null)
            {
                return RedirectToAction("PagesList");
            }

            page.Alias = model.Alias;
            page.Naim = model.Naim;
            page.Text = model.Text;

            db.SubmitChanges();
            return RedirectToAction("PagesList");
        }

        public ActionResult Objects()
        {
            return View();
        }

        public ActionResult LiveConditionsList()
        {
            dbDataContext db = new dbDataContext();
            List<LiveConditions> lst = db.LiveConditions.Select(c => c).OrderBy(c => c.lc_key).ToList();

            return View(lst);
        }

        [HttpGet]
        public ActionResult CreateLiveCondition()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateLiveCondition(LiveConditions model)
        {
            dbDataContext db = new dbDataContext();

            db.LiveConditions.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("LiveConditionsList");
        }
        public ActionResult DeleteLiveCondition(int id)
        {
            dbDataContext db = new dbDataContext();
            LiveConditions lc = db.LiveConditions.SingleOrDefault(c => c.id == id);
            if (lc != null)
            {
                db.LiveConditions.DeleteOnSubmit(lc);
                db.SubmitChanges();
            }
            return RedirectToAction("LiveConditionsList");
        }

        [HttpGet]
        public ActionResult EditLiveCondition(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("LiveConditionsList");
            dbDataContext db = new dbDataContext();
            LiveConditions lc = db.LiveConditions.SingleOrDefault(c => c.id == id);
            if (lc == null)
            {
                return RedirectToAction("LiveConditionsList");
            }
            return View(lc);
        }
        [HttpPost]
        public ActionResult EditLiveCondition(LiveConditions model)
        {
            dbDataContext db = new dbDataContext();
            LiveConditions lc = db.LiveConditions.SingleOrDefault(c => c.id == model.id);
            if (lc == null)
            {
                return RedirectToAction("LiveConditionsList");
            }

            lc.lc_key = model.lc_key;
            lc.lc_value = model.lc_value;

            db.SubmitChanges();
            return RedirectToAction("LiveConditionsList");
        }
        public ActionResult AttributesList()
        {
            List<String> lst_pics = new List<string>();
            dbDataContext db = new dbDataContext();
            List<Attributes> lst = db.Attributes.Select(c => c).OrderBy(c => c.attr_key).ToList();
            foreach (Attributes attrs in lst)
            {
                lst_pics.Add("data:image/jpeg;base64," + attrs.picture.ToString().Replace("\"", ""));
            }
            ViewBag.pics = lst_pics;
            return View(lst);
        }
        [HttpGet]
        public ActionResult CreateAttribute()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateAttribute(FormCollection coll)
        {
            dbDataContext db = new dbDataContext();

            Attributes attr = new Attributes();
            attr.attr_key = Request["attr_key"];
            
            HttpPostedFileBase file = Request.Files[0];
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromStream(file.InputStream);
            img.Save(ms, img.RawFormat);
            Binary bin = new Binary(ms.ToArray());
            attr.picture = bin;

            db.Attributes.InsertOnSubmit(attr);
            db.SubmitChanges();

            return RedirectToAction("AttributesList");
        }

        public ActionResult DeleteAttribute(int id)
        {
            dbDataContext db = new dbDataContext();
            Attributes item = db.Attributes.SingleOrDefault(c => c.id == id);
            if (item != null)
            {
                db.Attributes.DeleteOnSubmit(item);
                db.SubmitChanges();
            }
            return RedirectToAction("AttributesList");
        }

        [HttpGet]
        public ActionResult EditAttribute(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("AttributesList");
            dbDataContext db = new dbDataContext();
            Attributes attr = db.Attributes.SingleOrDefault(c => c.id == id);
            if (attr == null)
            {
                return RedirectToAction("AttributesList");
            }
            ViewBag.pic = "data:image/jpeg;base64," + attr.picture.ToString().Replace("\"", "");
            return View(attr);
        }
        [HttpPost]
        public ActionResult EditAttribute(FormCollection coll)
        {
            int id;
            try
            {
                id = Int32.Parse(coll["id"]);
            }
            catch
            {
                return RedirectToAction("AttributesList");
            }
            dbDataContext db = new dbDataContext();

            Attributes attr = db.Attributes.SingleOrDefault(c => c.id == id);
            attr.attr_key = Request["attr_key"];

            HttpPostedFileBase file = Request.Files[0];
            MemoryStream ms = new MemoryStream();
            Image img = Image.FromStream(file.InputStream);
            img.Save(ms, img.RawFormat);
            Binary bin = new Binary(ms.ToArray());
            attr.picture = bin;

            db.SubmitChanges();

            return RedirectToAction("AttributesList");
        }

        public ActionResult RegionList()
        {
            dbDataContext db = new dbDataContext();
            List<Flats.Views.Manage.Region> lst = db.Region.Select(c => c).OrderBy(c => c.ID).ToList();

            return View(lst);
        }

        [HttpGet]
        public ActionResult CreateRegion()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateRegion(Flats.Views.Manage.Region model)
        {
            dbDataContext db = new dbDataContext();

            db.Region.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("RegionList");
        }
        public ActionResult DeleteRegion(int id)
        {
            dbDataContext db = new dbDataContext();
            Flats.Views.Manage.Region ft = db.Region.SingleOrDefault(c => c.ID == id);
            if (ft != null)
            {
                db.Region.DeleteOnSubmit(ft);
                db.SubmitChanges();
            }
            return RedirectToAction("RegionList");
        }

        [HttpGet]
        public ActionResult EditRegion(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("RegionList");
            dbDataContext db = new dbDataContext();
            Flats.Views.Manage.Region ft = db.Region.SingleOrDefault(c => c.ID == id);
            if (ft == null)
            {
                return RedirectToAction("RegionList");
            }
            return View(ft);
        }
        [HttpPost]
        public ActionResult EditRegion(Flats.Views.Manage.Region model)
        {
            dbDataContext db = new dbDataContext();
            Flats.Views.Manage.Region ft = db.Region.SingleOrDefault(c => c.ID == model.ID);
            if (ft == null)
            {
                return RedirectToAction("RegionList");
            }

            ft.Naim = model.Naim;


            db.SubmitChanges();
            return RedirectToAction("RegionList");
        }

        public ActionResult ObjectsList() 
        {
            dbDataContext db = new dbDataContext();
            List<Objects> lst = db.Objects.Select(c => c).OrderBy(c => c.header).ToList();

            return View(lst);
        }
        [HttpGet]
        public ActionResult CreateObjects()
        {
            return View();
        }

    }
}