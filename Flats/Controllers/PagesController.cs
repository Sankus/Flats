using Flats.Views.Manage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Flats.Controllers
{
    public class PagesController : Controller
    {
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
        }
        // GET: Pages
        public ActionResult Index(string id="")
        {
            InitSettings();
            if (id == "")
            {
                return RedirectToAction("Index", "Home");
            }
            dbDataContext db = new dbDataContext();

            var page = db.Pages.SingleOrDefault(c => c.Alias == id);
            if (page==null)
                return RedirectToAction("Index", "Home");
            ViewBag.id = page.id;
            ViewBag.name = page.Naim;
            ViewBag.alias = page.Alias;
            ViewBag.html = page.Text;

            return View();
        }
    }
}