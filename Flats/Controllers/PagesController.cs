﻿using Flats.Views.Manage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

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

            List<Region> list_region = db.Region.Select(c => c).OrderBy(c => c.Naim).ToList<Region>();
            ViewBag.regions = list_region;

            //Подтягиваем курсы валют
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://bank-ua.com/export/currrate.xml");
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.GetEncoding(1251));
            String doc = reader.ReadToEnd();
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml(doc);
            XmlElement xRoot = xDoc.DocumentElement;
            //доллары
            XmlNode xnode = xRoot.SelectSingleNode(".//code[text()='840']");
            String UsdRate = (xnode.ParentNode).ChildNodes[5].InnerText.Split('.')[0];
            ViewBag.UsdRate = UsdRate.Substring(0, 2) + "," + UsdRate.Substring(2, 2);
            if ((xnode.ParentNode).ChildNodes[6].InnerText.Substring(0, 1) == "-")
                ViewBag.UsdDir = "˅";
            else
                ViewBag.UsdDir = "˄";

            //евро
            xnode = xRoot.SelectSingleNode(".//code[text()='978']");
            String EurRate = (xnode.ParentNode).ChildNodes[5].InnerText.Split('.')[0];
            ViewBag.EurRate = EurRate.Substring(0, 2) + "," + EurRate.Substring(2, 2);
            if ((xnode.ParentNode).ChildNodes[6].InnerText.Substring(0, 1) == "-")
                ViewBag.EurDir = "˅";
            else
                ViewBag.EurDir = "˄";

            //Подтягиваем погоду
            req = (HttpWebRequest)HttpWebRequest.Create("https://export.yandex.ru/weather-ng/forecasts/33837.xml");
            resp = (HttpWebResponse)req.GetResponse();
            reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
            doc = reader.ReadToEnd();
            xDoc = new XmlDocument();
            xDoc.LoadXml(doc);
            xRoot = xDoc.DocumentElement;

            ViewBag.temperature_value = xRoot.ChildNodes[0].ChildNodes[4].InnerText;
            ViewBag.temperature_color = xRoot.ChildNodes[0].ChildNodes[4].Attributes[0].Value;

            xnode = xRoot.ChildNodes[0].ChildNodes[8];
            ViewBag.temperature_pic = "http://yandex.st/weather/1.1.78/i/icons/48x48/" + xnode.InnerText + ".png";

            xRoot = xDoc.DocumentElement;
            ViewBag.isMainPage = false;

            //languages
            if (Session.IsNewSession)
            {
                Session.Add("lang", "ru");
            }
            string lang = Session["lang"].ToString();

            List<Translate> lang_list = db.Translate.Select(c => c).Where(c => c.languages.short_code.ToLower().Trim() == lang.ToLower().Trim()).ToList<Translate>();
            Dictionary<String, String> lang_arr = new Dictionary<string, string>();
            foreach (Translate tran in lang_list)
            {
                if (!lang_arr.ContainsKey(tran.Phrase.phrase_key))
                    lang_arr.Add(tran.Phrase.phrase_key, tran.Translation);
            }

            foreach (Phrase ph in db.Phrase.Select(c => c))
            {
                if (!lang_arr.ContainsKey(ph.phrase_key))
                    lang_arr.Add(ph.phrase_key, ph.phrase_key);
            }

            ViewBag.Translation = lang_arr;
            //--languages
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