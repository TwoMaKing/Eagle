using Eagle.Tests.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveLife.Model
{
    public class PostModels
    {
        private static List<PostDataObject> allPosts = new List<PostDataObject>();

        private static Dictionary<DateTime, List<PostDataObject>> postsByDate = new Dictionary<DateTime, List<PostDataObject>>();

        static PostModels() 
        { 
            List<PostDataObject> posts1 = new List<PostDataObject>();
            DateTime datetimeOne = DateTime.Parse("2015-03-11");
            DateTime datetimeTwo = DateTime.Parse("2015-03-12");

            var p1 = new PostDataObject() { Id = 1000, Content = "LALALALALALA" };
            p1.Author.Name = "SB";
            p1.CreationDateTime = datetimeOne;

            var p2 = new PostDataObject() { Id = 1001, Content = "XXXXXXXXXXXX" };
            p2.Author.Name = "ZYL";
            p2.CreationDateTime = datetimeOne;
            posts1.AddRange(new PostDataObject[] { p1, p2 });

            List<PostDataObject> posts2 = new List<PostDataObject>();
            var p3= new PostDataObject() { Id = 1002, Content = "LOLOLOLOLOLO" };
            p3.Author.Name = "YSY";
            p3.CreationDateTime = datetimeTwo;

            var p4 = new PostDataObject() { Id = 1003, Content = "AIAIAIAIAIAIAI" };
            p4.Author.Name = "GQ";
            p3.CreationDateTime = datetimeTwo;

            posts2.AddRange(new PostDataObject[] { p3, p4 });

            postsByDate.Add(datetimeOne, posts1);
            postsByDate.Add(datetimeTwo, posts2);

            allPosts.AddRange(new PostDataObject[] { p1, p2, p3, p4 });
        }

        public static Dictionary<DateTime, List<PostDataObject>> PostsByDate 
        { 
            get 
            {
                return postsByDate;
            } 
        }

        public static IEnumerable<PostDataObject> AllPosts 
        { 
            get
            {
                return allPosts;
            }
        }

    }
}