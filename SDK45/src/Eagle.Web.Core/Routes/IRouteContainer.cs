using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Eagle.Web.Core.Routes
{
    public interface IRouteContainer
    {
        IRouteProvider RouteProvider { get; }

        void MapRoute(RouteItem route);

        void DemapRoute(RouteItem route);
    }
}
