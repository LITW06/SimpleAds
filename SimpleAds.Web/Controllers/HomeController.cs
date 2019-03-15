using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleAds.Data;
using System.IO;
using SimpleAds.Web.Models;

namespace SimpleAds.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SimpleAdDb db = new SimpleAdDb(Properties.Settings.Default.ConStr);
            IEnumerable<SimpleAd> ads = db.GetAds();
            List<string> ids = new List<string>();
            if (Request.Cookies["AdIds"] != null)
            {
                ids = Request.Cookies["AdIds"].Value.Split(',').ToList();
            }

            var vm = new HomePageViewModel
            {
                Ads = ads.Select(ad =>
                {
                    return new AdViewModel
                    {
                        Ad = ad,
                        CanDelete = ids.Contains(ad.Id.ToString())
                    };
                })
            };
            if (TempData["message"] != null)
            {
                vm.Message = (string) TempData["Message"];
            }


            return View(vm);
        }

        public ActionResult NewAd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewAd(SimpleAd ad)
        {
            SimpleAdDb db = new SimpleAdDb(Properties.Settings.Default.ConStr);
            db.AddSimpleAd(ad);
            string ids = "";
            HttpCookie cookie = Request.Cookies["AdIds"];
            if (cookie != null)
            {
                ids = $"{cookie.Value},";
            }
            ids += ad.Id;
            Response.Cookies.Add(new HttpCookie("AdIds", ids));

            TempData["message"] = $"New Listing Added Id - {ad.Id}";

            return Redirect("/");
        }

        [HttpPost]
        public ActionResult DeleteAd(int id)
        {
            SimpleAdDb db = new SimpleAdDb(Properties.Settings.Default.ConStr);
            db.Delete(id);
            return Redirect("/");
        }
    }
}