using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleAds.Data;
using SimpleAds.Web.Models;

namespace SimpleAds.Web.Controllers
{
    public class HomeSessionController : Controller
    {
        public ActionResult Index()
        {
            SimpleAdDb db = new SimpleAdDb(Properties.Settings.Default.ConStr);
            IEnumerable<SimpleAd> ads = db.GetAds();
            List<int> ids = new List<int>();
            if (Session["ListingIds"] != null)
            {
                ids = (List<int>) Session["ListingIds"];
            }

            return View(new HomePageViewModel
            {
                Ads = ads.Select(ad =>
                {
                    return new AdViewModel
                    {
                        Ad = ad,
                        CanDelete = ids.Contains(ad.Id)
                    };
                })
            });
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
            if (Session["ListingIds"] == null)
            {
                Session["ListingIds"] = new List<int>();
            }

            List<int> ids = (List<int>) Session["ListingIds"];
            ids.Add(ad.Id);
            return Redirect("/homesession/index");
        }

        [HttpPost]
        public ActionResult DeleteAd(int id)
        {
            SimpleAdDb db = new SimpleAdDb(Properties.Settings.Default.ConStr);
            db.Delete(id);
            return Redirect("/homesession/index");
        }
    }
}