using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNetWebApp
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TwitterFeedControl.TwitterUserID = ConfigurationManager.AppSettings["TwitterUserID"];
            TwitterFeedControl.TwitterFeedUrl = ConfigurationManager.AppSettings["TwitterFeedURL"];
            TwitterFeedControl.TwitterFeedCacheTime = Convert.ToDouble(ConfigurationManager.AppSettings["TwitterFeedCacheTime"]);
        }
    }
}