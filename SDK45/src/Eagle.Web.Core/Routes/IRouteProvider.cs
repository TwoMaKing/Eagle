using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Eagle.Web.Core.Routes
{
    public interface IRouteProvider
    {
        IEnumerable<RouteItem> GetRoutes();
    }
}
