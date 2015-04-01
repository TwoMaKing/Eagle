using Eagle.Core.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Eagle.Web.Core.Routes
{
    public class RouteContainer : IRouteContainer
    {
        private IRouteProvider routeProvider;

        public static IRouteContainer Configure()
        {
            return Configure(typeof(RouteContainer), new ConfigRouteProvider());
        }

        public static IRouteContainer Configure(string containerName)
        {
            return AppRuntime.Instance.CurrentApplication.ObjectContainer.Resolve<IRouteContainer>(containerName);
        }

        public static IRouteContainer Configure(Type routeContainerType, params object[] args)
        {
            IRouteContainer routeContainer = (IRouteContainer)Activator.CreateInstance(routeContainerType, args);

            return routeContainer;
        }

        public RouteContainer(IRouteProvider routeProvider)
        {
            if (routeProvider == null)
            {
                throw new ArgumentNullException("The route provider cannot be null. Please specify a route provider.");
            }

            this.routeProvider = routeProvider;

            IEnumerable<RouteItem> routes = this.routeProvider.GetRoutes();

            if (routes == null)
            {
                return;
            }

            foreach (RouteItem route in routes)
            {
                this.MapRoute(route);
            }
        }

        public void MapRoute(RouteItem route)
        {
            if (route == null)
            {
                return;
            }

            RouteTable.Routes.MapPageRoute(route.Name, route.Url, route.PhysicalFile, true, route.Defaults, route.Constraints, route.DataTokens);
        }

        public void DemapRoute(RouteItem route)
        {
            RouteTable.Routes.Remove(route);
        }

        public IRouteProvider RouteProvider
        {
            get 
            {
                return this.routeProvider;
            }
        }
    }
}
