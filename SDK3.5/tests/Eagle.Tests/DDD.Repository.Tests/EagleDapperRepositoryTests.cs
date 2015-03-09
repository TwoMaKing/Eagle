using Eagle.Common.Query;
using Eagle.Common.Util;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Query;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Dapper;
using Eagle.Repositories.Sql;
using Eagle.Tests.Commands;
using Eagle.Tests.DataObjects;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Events;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;
using Eagle.Tests.Domain.Services;
using Eagle.Tests.ServiceContracts;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace Eagle.Tests
{
    [TestFixture()]
    public class EagleDapperRepositoryTests : TestBase
    {
        public EagleDapperRepositoryTests() : base() { }

        [Test()]
        public void Test_Add_Item()
        {
            using (IRepositoryContext ctx = ServiceLocator.Instance.GetService<IRepositoryContext>("DapperRepositoryContext"))
            {
                IPostRepository postRep = (IPostRepository)ctx.GetRepository<Post>();

                Post p = new Post();
                p.AuthorId = 1000;
                p.TopicId = 1000;
                p.Content = "Test Add_Post" + Utils.GetUniqueIdentifier(5);
                p.Status = new PostStatus();
                p.Status.InternalId = Guid.NewGuid();
                p.Status.InternalStatus = "NEW";

                postRep.Add(p);
               
                ctx.Commit();
            }
        }

        [Test()]
        public void Test_Add_Items()
        {

        }

        [Test()]
        public void Test_Update_Item()
        {
            using (IRepositoryContext ctx = ServiceLocator.Instance.GetService<IRepositoryContext>("DapperRepositoryContext"))
            {
                IPostRepository postRep = (IPostRepository)ctx.GetRepository<Post>();

                Post p = postRep.FindByKey(1016);
                p.Content = "Test_Update_Item" + Utils.GetUniqueIdentifier(5);
                p.Status.InternalStatus = "MODIFIED";

                postRep.Update(p);

                ctx.Commit();
            }
        }

        [Test()]
        public void Test_Delete_Item()
        {
            using (IRepositoryContext ctx = ServiceLocator.Instance.GetService<IRepositoryContext>("DapperRepositoryContext"))
            {
                IPostRepository postRep = (IPostRepository)ctx.GetRepository<Post>();

                Post p = postRep.FindByKey(1021);
                postRep.Delete(p);

                ctx.Commit();
            }
        }

        [Test()]
        public void Test_CRUD_Item()
        {
            using (IRepositoryContext ctx = ServiceLocator.Instance.GetService<IRepositoryContext>("DapperRepositoryContext"))
            {
                IPostRepository postRep = (IPostRepository)ctx.GetRepository<Post>();

                Post p1 = new Post();
                p1.AuthorId = 1000;
                p1.TopicId = 1000;
                p1.Content = "Test CRUD Add Item" + Utils.GetUniqueIdentifier(5);
                p1.Status = new PostStatus();
                p1.Status.InternalId = Guid.NewGuid();
                p1.Status.InternalStatus = "NEW";
                postRep.Add(p1);
                
                Post p2 = postRep.FindByKey(1017);
                p2.Content = "Test CRUD Update Item" + Utils.GetUniqueIdentifier(5);
                p2.Status.InternalStatus = "MODIFIED";
                postRep.Update(p2);

                Post p3 = postRep.FindByKey(1038);
                postRep.Delete(p3);

                ctx.Commit();
            }
        }

        [Test()]
        public void Test_Query_FindItemsByPaging()
        {

        }
    }
}
