using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleAds.Data;

namespace SimpleAds.Web.Models
{
    public class HomePageViewModel
    {
        public IEnumerable<AdViewModel> Ads { get; set; }
        public string Message { get; set; }
    }
}