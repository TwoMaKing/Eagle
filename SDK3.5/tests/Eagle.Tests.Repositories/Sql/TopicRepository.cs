using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Eagle.Common.Util;
using Eagle.Core.Query;
using Eagle.Core.QuerySepcifications;
using Eagle.Data;
using Eagle.Domain.Repositories;
using Eagle.Repositories.Sql;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;
using Eagle.Tests.Domain.Repositories;

namespace Eagle.Tests.Repositories
{
    public class TopicRepository : SqlRepository<Topic>, ITopicRepository
    {
        private const string whereById = "topic_id=@id";

        public TopicRepository(IRepositoryContext repositoryContext) : base(repositoryContext) { }

        protected override string GetAggregateRootQuerySqlById()
        {
            return "select * from topic where topic_id = @id";
        }

        protected override Topic BuildAggregateRootFromDataReader(IDataReader dataReader)
        {
            Topic topic = new Topic();

            topic.Id = Convertor.ConvertToInteger(dataReader["topic_id"]).Value;
            topic.Name = dataReader["topic_name"].ToString();
            topic.Summary = dataReader["topic_desc"].ToString();

            return topic;
        }

        protected override Dictionary<string, AppendChildToAggregateRoot> BuildChildCallbacks()
        {
            return null;
        }

        protected override string GetFromTableSqlByFindAll()
        {
            throw new NotImplementedException();
        }

        protected override string[] GetSelectColumnsByFindAll()
        {
            throw new NotImplementedException();
        }

        protected override void DoAdd(Topic topic)
        {
            throw new NotImplementedException();
        }

        protected override void DoUpdate(Topic topic)
        {
            throw new NotImplementedException();
        }

        protected override void DoDelete(Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}
