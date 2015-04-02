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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Posts.DataSource = PostModels.AllPosts;

            this.Posts.DataBind();
        }

        public IEnumerable<PostDataObject> AllPosts 
        {
            get 
            {
                return PostModels.AllPosts;
            }
        }
    }
}