using Eagle.Domain.Repositories;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Eagle.Repositories.Lite
{
    public interface ILiteRepositoryContext : IRepositoryContext
    {
        OrmLiteConnectionFactory LiteConnectionFactory { get; }
    }
}
