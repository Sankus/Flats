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
            var recs = from p in db.Objects select new { 
                id=p.ID,
                type=p.type,
                region_id = p.region_id,
                region_name = p.Region.Naim
            };
            return View(lst);
        }
        [HttpGet]
        public ActionResult CreateObjects()
        {
            dbDataContext db = new dbDataContext();
            List<Flats.Views.Manage.Region> lst = db.Region.Select(c => c).OrderBy(c => c.Naim).ToList<Flats.Views.Manage.Region>();
            ViewBag.regions = lst;
            List<LiveConditions> lc_lst = db.LiveConditions.Select(c => c).OrderBy(c => c.lc_key).ToList<LiveConditions>();
            ViewBag.lc_lst = lc_lst;
            List<Attributes> attrs = db.Attributes.Select(c => c).OrderBy(c => c.attr_key).ToList<Attributes>();
            ViewBag.attrs = attrs;
            List<String> lst_pics = new List<string>();
            foreach (Attributes attr in attrs)
            {
                lst_pics.Add("data:image/jpeg;base64," + attr.picture.ToString().Replace("\"", ""));
            }
            ViewBag.pics = lst_pics;

            return View();
        }

        public ActionResult EditObjects(int id)
        {
            if (id == 0)
                return RedirectToAction("ObjectsList");
            dbDataContext db = new dbDataContext();
            Objects ft = db.Objects.SingleOrDefault(c => c.ID == id);
            if (ft == null)
            {
                return RedirectToAction("ObjectsList");
            }
            List<Flats.Views.Manage.Region> lst = db.Region.Select(c => c).OrderBy(c => c.Naim).ToList<Flats.Views.Manage.Region>();
            ViewBag.regions = lst;


            List<LiveConditions> lc_lst = db.LiveConditions.Select(c => c).OrderBy(c => c.lc_key).ToList<LiveConditions>();
            ViewBag.lc_lst = lc_lst;
            List<Objects_LiveConditions> used_lc_list = db.Objects_LiveConditions.Select(c => c).Where(c => c.object_id == id).ToList<Objects_LiveConditions>();
            ViewBag.used_lc_list = used_lc_list;

            List<Attributes> attrs = db.Attributes.Select(c => c).OrderBy(c => c.attr_key).ToList<Attributes>();
            ViewBag.attrs = attrs;

            List<Objects_Attributes> used_attr = db.Objects_Attributes.Select(c => c).Where(c => c.object_id == id).ToList<Objects_Attributes>();
            List<int> ua = new List<int>();
            foreach (Objects_Attributes oa in used_attr)
            {
                ua.Add(oa.attribute_id);
            }
            ViewBag.ua_list = ua;

            List<String> lst_pics = new List<string>();
            foreach (Attributes attr in attrs)
            {
                lst_pics.Add("data:image/jpeg;base64," + attr.picture.ToString().Replace("\"", ""));
            }
            ViewBag.pics = lst_pics;
            return View(ft);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateObjects(FormCollection coll)
        {
            dbDataContext db = new dbDataContext();
            Objects obj = new Objects();
            obj.type = Int32.Parse(coll["FlatType"]);
            obj.region_id = Int32.Parse(coll["Region"]);
            obj.header = coll["header"];
            obj.address = coll["address"];
            if (coll["rooms_count"] != String.Empty)
                obj.rooms_count = Int32.Parse(coll["rooms_count"]);
            else
                obj.rooms_count = 0;
            if (coll["guests_count"] != String.Empty)
                obj.guests_count = Int32.Parse(coll["guests_count"]);
            else
                obj.guests_count = 0;

            if (coll["price1"] != String.Empty)
                obj.price1 = Decimal.Parse(coll["price1"]);
            else obj.price1 = 0;

            if (coll["price2"] != String.Empty)
                obj.price2 = Decimal.Parse(coll["price2"]);
            else
                obj.price2 = 0;

            if (coll["price5"] != String.Empty)
                obj.price5 = Decimal.Parse(coll["price5"]);
            else obj.price5 = 0;
            if (coll["price14"] != String.Empty)
                obj.price14 = Decimal.Parse(coll["price14"]);
            else
                obj.price14 = 0;

            obj.desc_body = coll["desc"];

            //Записываем картинки

            HttpPostedFileBase file = Request.Files["large_foto1"];
            MemoryStream ms;
            Image img;
            Binary bin;

            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic1large = bin;
            }

            file = Request.Files["large_foto2"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic2large = bin;
            }

            file = Request.Files["foto1"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic1 = bin;
            }

            file = Request.Files["foto2"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic2 = bin;
            }


            file = Request.Files["foto3"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic3 = bin;
            }

            file = Request.Files["foto4"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic4 = bin;
            }
            //--Записываем картинки

            db.Objects.InsertOnSubmit(obj);
            db.SubmitChanges();

            //Записываем жизненные условия
            var lcs = coll.AllKeys.Where(c => (c.StartsWith("lc_")&(!c.Contains("val"))));

            foreach (var attr in lcs)
            {
                string temp_key = attr.Replace("lc_","lc_val_"); 
                string temp_value = coll[temp_key];
                Objects_LiveConditions olc = new Objects_LiveConditions();
                olc.lc_id = Int32.Parse(attr.Replace("lc_", ""));
                olc.object_id = obj.ID;
                olc._value = temp_value;
                db.Objects_LiveConditions.InsertOnSubmit(olc);
            }


            //Записываем атрибуты
            var attrs = coll.AllKeys.Where(c => (c.StartsWith("attr_")));

            foreach (var attr in attrs)
            {
                string temp_id = attr.Replace("attr_", "");
                Objects_Attributes oattr = new Objects_Attributes();
                oattr.attribute_id = Int32.Parse(temp_id);
                oattr.object_id = obj.ID;
                db.Objects_Attributes.InsertOnSubmit(oattr);
            }

            db.SubmitChanges();

            return RedirectToAction("ObjectsList");
        }

        public ActionResult DeleteObject(int id)
        {
            dbDataContext db = new dbDataContext();
            Objects ft = db.Objects.SingleOrDefault(c => c.ID == id);
            if (ft != null)
            {
                db.Objects.DeleteOnSubmit(ft);

                List<Objects_LiveConditions> olc_list = db.Objects_LiveConditions.Select(p => p).Where(p => p.object_id == id).ToList<Objects_LiveConditions>();
                foreach (Objects_LiveConditions olc in olc_list)
                {
                    db.Objects_LiveConditions.DeleteOnSubmit(olc);
                }

                List<Objects_Attributes> oattr_list = db.Objects_Attributes.Select(p => p).Where(p => p.object_id == id).ToList<Objects_Attributes>();
                foreach (Objects_Attributes olc in oattr_list)
                {
                    db.Objects_Attributes.DeleteOnSubmit(olc);
                }

                db.SubmitChanges();
            }
            return RedirectToAction("ObjectsList");
        }

        public ActionResult GetPictureFL1(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic1large == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }
            return File(rec.pic1large.ToArray(), "image/jpeg");
        }

        public ActionResult GetPictureFL2(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic2large == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }

            return File(rec.pic2large.ToArray(), "image/jpeg");
        }
        public ActionResult GetPictureF1(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic1 == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }

            return File(rec.pic1.ToArray(), "image/jpeg");
        }

        public ActionResult GetPictureF2(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic2 == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }

            return File(rec.pic2.ToArray(), "image/jpeg");
        }
        public ActionResult GetPictureF3(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic3 == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }

            return File(rec.pic3.ToArray(), "image/jpeg");
        }
        public ActionResult GetPictureF4(String id)
        {
            id = id.Replace(".jpg", "");
            dbDataContext db = new dbDataContext();
            Objects rec = db.Objects.SingleOrDefault(c => c.ID == Int32.Parse(id));
            if (rec.pic4 == null)
            {
                byte[] arr = new byte[0];
                return File(arr, "image/jpeg");
            }

            return File(rec.pic4.ToArray(), "image/jpeg");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditObjects(FormCollection coll)
        {
            dbDataContext db = new dbDataContext();
            int obj_id;
            try
            {
                obj_id = Int32.Parse(coll["obj_id"]);
            }
            catch
            {
                return RedirectToAction("ObjectsList");
            }

            Objects obj = db.Objects.SingleOrDefault(c=>c.ID==obj_id);
            if (obj == null)
            {
                return RedirectToAction("ObjectsList");
            }

            obj.type = Int32.Parse(coll["FlatType"]);
            obj.region_id = Int32.Parse(coll["Region"]);
            obj.header = coll["header"];
            obj.address = coll["address"];
            if (coll["rooms_count"] != String.Empty)
                obj.rooms_count = Int32.Parse(coll["rooms_count"]);
            else
                obj.rooms_count = 0;
            if (coll["guests_count"] != String.Empty)
                obj.guests_count = Int32.Parse(coll["guests_count"]);
            else
                obj.guests_count = 0;

            if (coll["price1"] != String.Empty)
                obj.price1 = Decimal.Parse(coll["price1"]);
            else obj.price1 = 0;

            if (coll["price2"] != String.Empty)
                obj.price2 = Decimal.Parse(coll["price2"]);
            else
                obj.price2 = 0;

            if (coll["price5"] != String.Empty)
                obj.price5 = Decimal.Parse(coll["price5"]);
            else obj.price5 = 0;
            if (coll["price14"] != String.Empty)
                obj.price14 = Decimal.Parse(coll["price14"]);
            else
                obj.price14 = 0;

            obj.desc_body = coll["desc"];

            //Записываем картинки

            HttpPostedFileBase file = Request.Files["large_foto1"];
            MemoryStream ms;
            Image img;
            Binary bin;

            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic1large = bin;
            }

            file = Request.Files["large_foto2"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic2large = bin;
            }

            file = Request.Files["foto1"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic1 = bin;
            }

            file = Request.Files["foto2"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic2 = bin;
            }
            
            file = Request.Files["foto3"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic3 = bin;
            }

            file = Request.Files["foto4"];
            if (file.ContentLength != 0)
            {
                ms = new MemoryStream();
                img = Image.FromStream(file.InputStream);
                img.Save(ms, img.RawFormat);
                bin = new Binary(ms.ToArray());
                obj.pic4 = bin;
            }//--Записываем картинки

            db.SubmitChanges();

            //Чистим таблицы- связки
            List<Objects_LiveConditions> olc_list = db.Objects_LiveConditions.Select(p => p).Where(p => p.object_id == obj.ID).ToList<Objects_LiveConditions>();
            foreach (Objects_LiveConditions olc in olc_list)
            {
                db.Objects_LiveConditions.DeleteOnSubmit(olc);
            }
            List<Objects_Attributes> oattr_list = db.Objects_Attributes.Select(p => p).Where(p => p.object_id == obj.ID).ToList<Objects_Attributes>();
            foreach (Objects_Attributes olc in oattr_list)
            {
                db.Objects_Attributes.DeleteOnSubmit(olc);
            }


            db.SubmitChanges();

            //Записываем жизненные условия
            var lcs = coll.AllKeys.Where(c => (c.StartsWith("lc_") & (!c.Contains("val"))));

            foreach (var attr in lcs)
            {
                string temp_key = attr.Replace("lc_", "lc_val_");
                string temp_value = coll[temp_key];
                Objects_LiveConditions olc = new Objects_LiveConditions();
                olc.lc_id = Int32.Parse(attr.Replace("lc_", ""));
                olc.object_id = obj.ID;
                olc._value = temp_value;
                db.Objects_LiveConditions.InsertOnSubmit(olc);
            }
            
            
            var attrs = coll.AllKeys.Where(c => (c.StartsWith("attr_")));

            foreach (var attr in attrs)
            {
                string temp_id = attr.Replace("attr_", "");
                Objects_Attributes oattr = new Objects_Attributes();
                oattr.attribute_id = Int32.Parse(temp_id);
                oattr.object_id = obj.ID;
                db.Objects_Attributes.InsertOnSubmit(oattr);
            }

            
            db.SubmitChanges();

            return RedirectToAction("ObjectsList");
        }

        public ActionResult Languages()
        {
            return View();
        }
        public ActionResult LangugeList()
        {
            dbDataContext db = new dbDataContext();
            List<languages> list = db.languages.Select(c => c).OrderBy(c => c.short_code).ToList<languages>();
            return View(list);
        }

        public ActionResult CreateLanguage()
        {
            return View();        
        }

        [HttpPost]
        public ActionResult CreateLanguage(languages model)
        {
            dbDataContext db = new dbDataContext();

            db.languages.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("LangugeList");
        }

        public ActionResult DeleteLanguage(int id)
        {
            dbDataContext db = new dbDataContext();
            languages rec = db.languages.SingleOrDefault(c => c.id == id);
            if (rec==null)
                return RedirectToAction("LangugeList");

            db.languages.DeleteOnSubmit(rec);
            db.SubmitChanges();
            return RedirectToAction("LangugeList");
        }

        public ActionResult EditLanguage(int id)
        {
            dbDataContext db = new dbDataContext();
            languages rec = db.languages.SingleOrDefault(c => c.id == id);
            if (rec == null)
                return RedirectToAction("LangugeList");
            return View(rec);
        }

        [HttpPost]
        public ActionResult EditLanguage(languages model)
        {
            dbDataContext db = new dbDataContext();

            languages lang = db.languages.SingleOrDefault(c => c.id == model.id);
            if (lang == null)
            {
                return RedirectToAction("LangugeList");
            }
            lang.language = model.language;
            lang.short_code = model.short_code;

            db.SubmitChanges();

            return RedirectToAction("LangugeList");
        }

        public ActionResult PhrasesList()
        {
            dbDataContext db = new dbDataContext();
            List<Phrase> list = db.Phrase.Select(c=>c).OrderBy(c=>c.phrase_key).ToList<Phrase>();
            return View(list);
        }

        public ActionResult CreatePhrase()
        {
            return View();        
        }

        [HttpPost]
        public ActionResult CreatePhrase(Phrase model)
        {
            dbDataContext db = new dbDataContext();

            db.Phrase.InsertOnSubmit(model);
            db.SubmitChanges();

            return RedirectToAction("PhrasesList");
        }

        public ActionResult DeletePhrase(int id)
        {
            dbDataContext db = new dbDataContext();
            Phrase rec = db.Phrase.SingleOrDefault(c => c.id == id);
            if (rec == null)
                return RedirectToAction("PhrasesList");

            db.Phrase.DeleteOnSubmit(rec);
            db.SubmitChanges();
            return RedirectToAction("PhrasesList");
        }

        public ActionResult EditPhrase(int id)
        {
            dbDataContext db = new dbDataContext();
            Phrase rec = db.Phrase.SingleOrDefault(c => c.id == id);
            if (rec == null)
                return RedirectToAction("PhrasesList");
            return View(rec);
        }

        [HttpPost]
        public ActionResult EditPhrase(Phrase model)
        {
            dbDataContext db = new dbDataContext();

            Phrase ph = db.Phrase.SingleOrDefault(c => c.id == model.id);
            if (ph == null)
            {
                return RedirectToAction("PhrasesList");
            }
            ph.phrase_key = model.phrase_key;

            db.SubmitChanges();

            return RedirectToAction("PhrasesList");
        }


        public ActionResult TranslateList()
        {
            dbDataContext db = new dbDataContext();
            List<Translate> list = db.Translate.Select(c => c).OrderBy(c => c.Phrase.phrase_key).ToList<Translate>();
            return View(list);
        }

        public ActionResult DeleteTranslate(int id)
        {
            dbDataContext db = new dbDataContext();
            Translate rec = db.Translate.SingleOrDefault(c => c.ID == id);
            if (rec == null)
                return RedirectToAction("TranslateList");

            db.Translate.DeleteOnSubmit(rec);
            db.SubmitChanges();
            return RedirectToAction("TranslateList");
        }

        public ActionResult CreateTranslate()
        {
            dbDataContext db = new dbDataContext();
            ViewBag.phrase_list = db.Phrase.Select(c => c).OrderBy(c => c.phrase_key).ToList<Phrase>();
            ViewBag.lang_list = db.languages.Select(c => c).OrderBy(c => c.short_code).ToList<languages>();

            return View();
        }

        [HttpPost]
        public ActionResult CreateTranslate(Translate model)
        {
            dbDataContext db = new dbDataContext();

            Translate rec = new Translate();
            rec.LanguageID = Int32.Parse(Request["languages"]);
            rec.PhraseID = Int32.Parse(Request["phrases"]);
            rec.Translation = model.Translation;
                
            db.Translate.InsertOnSubmit(rec);
            db.SubmitChanges();

            return RedirectToAction("TranslateList");
        }


        public ActionResult EditTranslate(int id)
        {
            dbDataContext db = new dbDataContext();
            Translate rec = db.Translate.SingleOrDefault(c => c.ID == id);
            if (rec == null)
                return RedirectToAction("TranslateList");
            ViewBag.phrase_list = db.Phrase.Select(c => c).OrderBy(c => c.phrase_key).ToList<Phrase>();
            ViewBag.lang_list = db.languages.Select(c => c).OrderBy(c => c.short_code).ToList<languages>();
            return View(rec);
        }

        [HttpPost]
        public ActionResult EditTranslate(Translate model)
        {
            dbDataContext db = new dbDataContext();

            Translate rec = db.Translate.SingleOrDefault(c => c.ID == model.ID);
            if (rec == null)
                return RedirectToAction("TranslateList");


            rec.LanguageID = Int32.Parse(Request["Lang"]);
            rec.PhraseID = Int32.Parse(Request["Phrase"]);
            rec.Translation = model.Translation;

            db.SubmitChanges();

            return RedirectToAction("TranslateList");
        }

    }
}