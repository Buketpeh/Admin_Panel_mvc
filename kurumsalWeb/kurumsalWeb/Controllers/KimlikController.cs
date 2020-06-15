using kurumsalWeb.Models.DataContext;

using kurumsalWeb.Models.Model;
using Sitecore.FakeDb;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace kurumsalWeb.Controllers
{
    public class KimlikController : Controller
    {
        KurumsalDBContext db = new KurumsalDBContext();
        // GET: Kimlik
        public ActionResult Index()
        {
            return View(db.Kimlik.ToList());
        }





        // GET: Kimlik/Edit/5
        public ActionResult Edit(int id)
        {
            var kimlik = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();//Böyle bir bilgi geliyor mu geliyorsa kontrolü sağla
            return View(kimlik);
        }

        // POST: Kimlik/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Kimlik kimlik, HttpPostedFileBase LogoURL)
        {
            if (ModelState.IsValid)
            {
                var k = db.Kimlik.Where(x => x.KimlikId == id).SingleOrDefault();

                if (LogoURL != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(k.LogoURL))) //k dan gelen logo url i bul
                    {
                        System.IO.File.Delete(Server.MapPath(k.LogoURL)); //k dan gelen logourl i sil upload dosyasından yer kaplamasın 
                    }
                    WebImage img = new WebImage(LogoURL.InputStream);
                    FileInfo imginfo = new FileInfo(LogoURL.FileName); //ismini alıyoruz

                    string logoname = LogoURL.FileName + imginfo.Extension; //burada extension uzantı demek uzantı değerini de almak gerekiyor
                    img.Resize(300, 200); //logonun boyutunu belirliyor(genişlik,yükseklik)
                    img.Save("~/Uploads/Kimlik/" + logoname); //bu logoyu nereye kaydedicez onu belirliyoruz,logoname diyerek ismiyle birlikte kaydedilmesini sağlıyorum
                    k.LogoURL = "/Uploads/Kimlik/" + logoname;

                }
                k.Title = kimlik.Title;
                k.keywords = kimlik.keywords;
                k.Description = kimlik.Description;
                k.Unvan = kimlik.Unvan;
               
                db.SaveChanges();
                return RedirectToAction("Index");

            }

           
        return View();

        }

}
}

