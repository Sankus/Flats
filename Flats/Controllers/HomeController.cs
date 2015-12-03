using Flats.Views.Manage;
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
    public class HomeController : Controller
    {
        public ActionResult GetPictureFL1(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.Where(c => c.ID == Int32.Parse(id)).Single();

            return File(rec.pic1large.ToArray(), "image/jpeg");
        }

        public ActionResult GetAttrPic(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Attributes rec = db.Attributes.Where(c => c.id == Int32.Parse(id)).Single();

            return File(rec.picture.ToArray(), "image/jpeg");
        }

        public ActionResult Premium()
        {
            InitSettings();
            dbDataContext db = new dbDataContext();
            List<Objects> lst_obj = db.Objects.Select(c => c).Where(c=>c.type == 1).ToList<Objects>();
            ViewBag.list = lst_obj;

            List<Objects_Attributes> obj_attr_list = db.Objects_Attributes.Select(c => c).Where(c => c.Objects.type == 1).ToList<Objects_Attributes>();
            ViewBag.obj_attr_list = obj_attr_list;
            
            return View();
        }

        public ActionResult Standart()
        {
            InitSettings();
            dbDataContext db = new dbDataContext();
            List<Objects> lst_obj = db.Objects.Select(c => c).Where(c => c.type == 2).ToList<Objects>();
            ViewBag.list = lst_obj;

            List<Objects_Attributes> obj_attr_list = db.Objects_Attributes.Select(c => c).Where(c => c.Objects.type == 2).ToList<Objects_Attributes>();
            ViewBag.obj_attr_list = obj_attr_list;

            return View();
        }

        public ActionResult Econom()
        {
            InitSettings();
            dbDataContext db = new dbDataContext();
            List<Objects> lst_obj = db.Objects.Select(c => c).Where(c => c.type == 3).ToList<Objects>();
            ViewBag.list = lst_obj;

            List<Objects_Attributes> obj_attr_list = db.Objects_Attributes.Select(c => c).Where(c => c.Objects.type == 3).ToList<Objects_Attributes>();
            ViewBag.obj_attr_list = obj_attr_list;

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
            ViewBag.isMainPage = true;
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
            if ((xnode.ParentNode).ChildNodes[6].InnerText.Substring(0,1)=="-")
                ViewBag.UsdDir = "-";
            else
                ViewBag.UsdDir = "+";

            //евро
            xnode = xRoot.SelectSingleNode(".//code[text()='978']");
            String EurRate = (xnode.ParentNode).ChildNodes[5].InnerText.Split('.')[0];
            ViewBag.EurRate = EurRate.Substring(0, 2) + "," + EurRate.Substring(2, 2);
            if ((xnode.ParentNode).ChildNodes[6].InnerText.Substring(0, 1) == "-")
                ViewBag.EurDir = "-";
            else
                ViewBag.EurDir = "+";

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