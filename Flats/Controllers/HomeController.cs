using Flats.Views.Manage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flats.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult GetPictureFL1(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.Where(c => c.ID == Int32.Parse(id)).Single();

            return File(rec.pic1large.ToArray(), "image/jpeg");
        }

        public ActionResult Premium()
        {
            InitSettings();
            dbDataContext db = new dbDataContext();
            List<Objects> lst_obj = db.Objects.Select(c => c).Where(c=>c.type == 1).ToList<Objects>();
            ViewBag.list = lst_obj;
            return View();
        }

        public ActionResult Standart()
        {
            InitSettings();
            return View();
        }

        public ActionResult Econom()
        {
            InitSettings();
            return View();
        }

        public ActionResult ReViews()
        {
            InitSettings();
            return View();
        }

        public ActionResult Contacts()
        {
            InitSettings();
            return View();
        }
        public ActionResult Index()
        {
            InitSettings();
            return View();
        }

        private void InitSettings()
        {
            dbDataContext db = new dbDataContext();
            List<Settings> Sett = db.Settings.Select(c => c).OrderBy(c => c.setting_key).ToList();
            Hashtable arr = new Hashtable();

            foreach (Settings item in Sett)
                arr.Add(item.setting_key, item.setting_value);
            ViewBag.settings = arr;

            List<Pages> pages_list = db.Pages.Select(c => c).OrderBy(c => c.Naim).ToList<Pages>();
            ViewBag.pages_list = pages_list;

            List<Region> list_region = db.Region.Select(c=> c).OrderBy(c=>c.Naim).ToList<Region>();
            ViewBag.regions = list_region;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.some = "some";

            return View();
        }
    }
}