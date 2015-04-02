using Eagle.Tests.DataObjects;
using LoveLife.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LoveLife
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime creationDatetime;
            int postId;

            string date = this.RouteData.Values["date"].ToString();
            string id = this.RouteData.Values["id"].ToString();

            bool parsedDatetime = DateTime.TryParse(date, out creationDatetime);

            bool parsedPostId = int.TryParse(id, out postId);

            if (parsedDatetime && parsedPostId)
            {
                this.CurrentPost = PostModels.PostsByDate[creationDatetime].SingleOrDefault(p => p.Id == postId);
            }
        }

        protected PostDataObject CurrentPost
        {
            get;
            set;
        }

    }
}