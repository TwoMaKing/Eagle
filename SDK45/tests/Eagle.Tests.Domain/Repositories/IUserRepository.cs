using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Eagle.Core.QuerySepcifications;
using Eagle.Domain.Repositories;
using Eagle.Tests.Domain;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {

    }
}
