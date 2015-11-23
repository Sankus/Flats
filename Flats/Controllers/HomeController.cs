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
        public ActionResult Premium()
        {
            InitSettings();
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