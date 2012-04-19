using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ASPNetWebApp.Controls
{
    public partial class TwitterFeedControl : System.Web.UI.UserControl
    {
        private string twitterUserID;
        public string TwitterUserID
        {
            get { return twitterUserID; }
            set { twitterUserID = value; }
        }

        private string twitterFeedUrl;
        public string TwitterFeedUrl
        {
            get { return twitterFeedUrl; }
            set { twitterFeedUrl = value; }
        }

        private double twitterFeedCacheTime = 10;
        public double TwitterFeedCacheTime
        {
            get { return twitterFeedCacheTime; }
            set { twitterFeedCacheTime = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            string cacheKey = "CacheKey";
            StringBuilder twitterFeedContents = new StringBuilder();

            if (Cache[cacheKey] != null)
            {
                // Used the cached version of the feed contents
                twitterFeedContents.Append(Cache[cacheKey].ToString());
            }
            else
            {
                // Refresh the cached twitter feed
                try
                {
                    // Retreive the latest from Twitter
                    WebClient webClient = new WebClient();
                    string jsonString = webClient.DownloadString(string.Format(twitterFeedUrl, twitterUserID));
                    JObject json = JObject.Parse(jsonString);

                    // Parse the feed contents
                    JArray results = (JArray)json["results"];

                    // Format the feed contents for web display
                    foreach (object result in results)
                    {
                        // Get the data for this tweet
                        JObject jsonResult = JObject.Parse(result.ToString());

                        string createdAt = (string)jsonResult["created_at"];
                        string age = string.Empty;
                        string fromUser = (string)jsonResult["from_user"];
                        string fromUserLink = fromUser;
                        string id = (string)jsonResult["id_str"];
                        string text = (string)jsonResult["text"];

                        // Determine how long ago this tweet was posted
                        DateTime createdAtDate;
                        if (DateTime.TryParse(createdAt, out createdAtDate))
                        {
                            DateTime now = DateTime.Now;
                            TimeSpan ts = now - createdAtDate;
                            if (ts.Days > 0) age = ts.Days.ToString() + " days ago";
                            else if (ts.Hours > 0) age = ts.Hours.ToString() + " hour" + (ts.Hours == 1 ? "" : "s") + " ago";
                            else if (ts.Minutes > 0) age = ts.Minutes.ToString() + " minutes ago";
                            else age = ts.Seconds.ToString() + " seconds ago";
                        }

                        // Wrap all hashtags in the text in <a> tags
                        var hashTags = from h in jsonResult["entities"]["hashtags"]
                                       select h;
                        foreach (var h in hashTags)
                        {
                            string hashText = "#" + (string)h["text"];
                            string replaceHashText = string.Format(
                                "<a class='tweet-url hashtag' href='http://twitter.com/search?q={0}' target='_blank'>{1}</a>",
                                Server.UrlEncode(hashText), hashText);
                            text = text.Replace(hashText, replaceHashText);
                        }

                        // Wrap all urls in the text in <a> tags
                        var urls = from u in jsonResult["entities"]["urls"]
                                   select u;
                        foreach (var u in urls)
                        {
                            string displayUrl = (string)u["display_url"];
                            string urlText = (string)u["url"];
                            string replaceUrlText = string.Format(
                                "<a rel='nofollow' href='{0}' target='_blank'>{1}</a>",
                                urlText, displayUrl);
                            text = text.Replace(urlText, replaceUrlText);
                        }

                        // Wrap all user mentions in the text in <a> tags
                        var userMentions = from um in jsonResult["entities"]["user_mentions"]
                                           select um;
                        foreach (var um in userMentions)
                        {
                            string screenName = (string)um["screen_name"];
                            string replaceScreenName = string.Format(
                                "<a class='tweet-url username' href='http://twitter.com/{0}' target='_blank'>{1}</a>",
                                screenName, "@" + screenName);
                            text = text.Replace("@" + screenName, replaceScreenName);
                        }

                        // Format the link to the Twitter page for the user posting the tweet
                        if (fromUser.Length > 0)
                        {
                            fromUserLink = string.Format(
                                "<a class='twtr-user' href='http://twitter.com/intent/user?screen_name={0}' target='_blank'>{1}</a>",
                                fromUser, fromUser);
                        }

                        // Format the age and favorite links
                        string ageLink = string.Format(
                            "<a class='twtr-timestamp' href='http://twitter.com/{0}/status/{1}' time='{2}' target='_blank'>{3}</a>",
                            fromUser, id, createdAt, age);

                        string favLink = string.Format(
                            "<a class='twtr-fav' href='http://twitter.com/intent/favorite?tweet_id={0}' target='_blank'>favorite</a>",
                            id);

                        // Build the complete entry for this tweet
                        twitterFeedContents.Append("<div class='twtr-tweet'>").Append("<div class='twtr-tweet-wrap'>");
                        twitterFeedContents.Append("<p>");
                        twitterFeedContents.Append(fromUserLink).Append(" ").Append(text);
                        twitterFeedContents.Append("<em>").Append(ageLink).Append(" · ").Append(favLink).Append("</em>");
                        twitterFeedContents.Append("</p>");
                        twitterFeedContents.Append("</div>").Append("</div>");
                    }
                }
                catch
                {
                    twitterFeedContents.Append(
                        string.Format("<p>Unable to access <a href='http://twitter.com/{0}' target='_blank'>{1}</a> twitter feed.</p>",
                            twitterUserID, twitterUserID));
                }

                // Cache the feed contents
                Cache.Add(cacheKey, twitterFeedContents, null, DateTime.Now.AddMinutes(twitterFeedCacheTime),
                    System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
            }

            // Output the twitter feed contents
            litFeed.Text = twitterFeedContents.ToString();
        }
    }
}


