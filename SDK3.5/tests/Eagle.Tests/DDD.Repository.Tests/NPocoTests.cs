using Eagle.Common.Query;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Query;
using Eagle.Domain.Repositories;
using Eagle.Tests.Commands;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Events;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;
using Eagle.Tests.ServiceContracts;
using NPoco;
using NPoco.Expressions;
using NPoco.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Eagle.Tests
{
    [TestFixture]
    public class NPocoTests
    {
        [Test]
        public void Test_Insert()
        {

        }

        [Test]
        public void Test_Update()
        {

        }

        [Test]
        public void Test_Delete()
        {

        }

        [Test]
        public void Test_Query()
        {
            using (IDatabase db = new Database(@"server=localhost\OSPTTEST;database=EXPRESSLIFE;User ID=sa;Password=3Edcvfr4567ujmnb", DatabaseType.SqlServer2008))
            {
                var p1 = db.Single<Post>("select p.post_id as Id, p.*, ps.* from post p inner join post_status ps on p.post_id = ps.post_id where p.post_id = @0", 1016);
                Assert.AreEqual(p1.Id, 1016);
                
                var ps2 = db.Query<Post, PostStatus>("select p.post_id as Id, p.*, ps.* from post p inner join post_status ps on p.post_id = ps.post_id where p.post_id = @0", 1016);
                var p2 = ps2.SingleOrDefault();
                Assert.AreEqual(p2.Id, 1016);

                var ps3 = db.Fetch<Post, PostStatus>("select p.post_id as Id, p.*, ps.* from post p inner join post_status ps on p.post_id = ps.post_id where p.post_id = @0", 1016);
                var p3 = ps3.SingleOrDefault();
                Assert.AreEqual(p3.Id, 1016);

                Func<Post, PostStatus, Post> map = (p, ps) =>
                {
                    p.Status = ps;
                    return p;
                };

                var ps4 = db.Fetch<Post, PostStatus, Post>
                    (map,
                     "select p.post_id as Id, p.*, ps.* from post p inner join post_status ps on p.post_id = ps.post_id where p.post_id = @0", 1016);
                
                var p4 = ps3.SingleOrDefault();

                Assert.AreEqual(p4.Id, 1016);


                Func<Post, PostStatus, Post> pageMap = (p, ps) =>
                {
                    p.Status = ps;
                    return p;
                };

                var ps5 = db.Page<Post, PostStatus, Post>
                    (pageMap, 3, 3,
                     "select p.post_id as Id, p.*, ps.* from post p inner join post_status ps on p.post_id = ps.post_id where p.post_id > @0", 1016);

                var p5 = ps5.Items;

                Assert.AreEqual(p5.Count, 3);

            }
        }

    }
}
