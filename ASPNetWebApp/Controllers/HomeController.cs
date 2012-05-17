using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPNetWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            TwitterFeed feed = new TwitterFeed();

            // IS IT APPROPRIATE TO ACCESS AppSettings HERE????
            feed.TwitterUserID = ConfigurationManager.AppSettings["TwitterUserID"];
            feed.TwitterFeedUrl = ConfigurationManager.AppSettings["TwitterFeedURL"];
            feed.TwitterFeedCacheTime = Convert.ToDouble(ConfigurationManager.AppSettings["TwitterFeedCacheTime"]);
            ViewBag.Message = MvcHtmlString.Create(feed.Render());
            return View();
        }
    }
}
