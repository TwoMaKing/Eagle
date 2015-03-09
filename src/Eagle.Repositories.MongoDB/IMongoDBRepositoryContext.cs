using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Domain.Repositories;
using MongoDB;

namespace Eagle.Repositories.MongoDB
{
    public interface IMongoDBRepositoryContext : IRepositoryContext
    {
        IMongoDatabase MongoDatabase { get; }
    }
}
