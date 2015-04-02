using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Eagle.Domain.Repositories;

namespace Eagle.Repositories.EntityFramework
{
    public interface IEntityFrameworkRepositoryContext : IRepositoryContext
    {
        DbContext DbContext { get; }
    }
}
