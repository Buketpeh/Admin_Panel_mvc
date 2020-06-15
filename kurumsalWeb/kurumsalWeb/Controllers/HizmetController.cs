using kurumsalWeb.Models.DataContext;
using kurumsalWeb.Models.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace kurumsalWeb.Controllers
{
    public class HizmetController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Hizmet
        public ActionResult Index()
        {
            return View(db.Hizmet.ToList());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Hizmet hizmet, HttpPostedFileBase ResimURL)
        {

            if (ModelState.IsValid)
            {
                if (ResimURL != null)
                {

                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName); //ismini alıyoruz

                    string logoname = Guid.NewGuid().ToString() + imginfo.Extension; //burada extension uzantı demek uzantı değerini de almak gerekiyor
                    img.Resize(500, 500); //logonun boyutunu belirliyor(genişlik,yükseklik)
                    img.Save("~/Uploads/Hizmet/" + logoname); //bu logoyu nereye kaydedicez onu belirliyoruz,logoname diyerek ismiyle birlikte kaydedilmesini sağlıyorum
                    hizmet.ResimURL = "/Uploads/Hizmet/" + logoname;

                }
                db.Hizmet.Add(hizmet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hizmet);


        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.Uyari = "Güncellenecek hizmet bulunamadi..";
            }
            var hizmet = db.Hizmet.Find(id);
            if (hizmet == null)
            {
                return HttpNotFound();
            }
            return View(hizmet);

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(int? id, Hizmet hizmet, HttpPostedFileBase ResimURL)
        {
            
            if (ModelState.IsValid)
            {
               var h = db.Hizmet.Where(x => x.HizmetId == id).SingleOrDefault();
                if (ResimURL != null)
                {

                    if (System.IO.File.Exists(Server.MapPath(h.ResimURL))) //k dan gelen logo url i bul
                    {
                        System.IO.File.Delete(Server.MapPath(h.ResimURL)); //k dan gelen logourl i sil upload dosyasından yer kaplamasın 
                    }
                    WebImage img = new WebImage(ResimURL.InputStream);
                    FileInfo imginfo = new FileInfo(ResimURL.FileName); //ismini alıyoruz

                    string hizmetname = Guid.NewGuid().ToString() + imginfo.Extension; //burada extension uzantı demek uzantı değerini de almak gerekiyor
                    img.Resize(300, 200); //logonun boyutunu belirliyor(genişlik,yükseklik)
                    img.Save("~/Uploads/Hizmet/" + hizmetname); //bu logoyu nereye kaydedicez onu belirliyoruz,logoname diyerek ismiyle birlikte kaydedilmesini sağlıyorum
                    h.ResimURL = "/Uploads/Hizmet/" + hizmetname;

                }

                h.Baslik = hizmet.Baslik;
                h.Aciklama = hizmet.Aciklama;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            if(id == null)
            {
                return HttpNotFound();
            }
            var h = db.Hizmet.Find(id);
            if(h == null)
            {
                return HttpNotFound();
            }
            db.Hizmet.Remove(h);
            db.SaveChanges();
            return RedirectToAction("Index");
            
        }

    }

}