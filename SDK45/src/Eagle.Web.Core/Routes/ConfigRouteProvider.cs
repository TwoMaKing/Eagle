using Eagle.Core.Exceptions;
using Eagle.Core.Extensions;
using Eagle.Web.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Eagle.Web.Core.Routes
{
    public class ConfigRouteProvider : IRouteProvider
    {
        private List<RouteItem> routes = new List<RouteItem>();

        public ConfigRouteProvider()
        {
            RouteConfigurationSection routeConfigSection = ConfigurationManager.GetSection("routeConfig") as RouteConfigurationSection;

            if (routeConfigSection == null)
            {
                throw new ConfigException("The specified RouteConfigurationSection object does not exist or this is not an instance of RouteConfigurationSection.");
            }

            if (routeConfigSection.Routes != null &&
                routeConfigSection.Routes.Count > 0)
            {
                string routeName;
                string routeUrl;
                string physicalFile;

                string[] urlParams;

                for(int routeIndex = 0; routeIndex < routeConfigSection.Routes.Count; routeIndex++)
                {
                    RouteMappingElement routeMapping = routeConfigSection.Routes[routeIndex];

                    if (routeMapping == null)
                    {
                        continue;
                    }

                    routeName = routeMapping.Name;

                    if (!routeName.HasValue())
                    {
                        throw new ConfigException("The route name is null or empty. Please provide a route name.");
                    }

                    routeUrl = routeMapping.Url;

                    Regex regex = new Regex("\\{" + @"([\w\d]+)" + "\\}");
                    MatchCollection matches = regex.Matches(routeUrl);
                    if (matches != null &&
                        matches.Count > 0)
                    {
                        urlParams = new string[matches.Count];
                        for(int matchIndex =0; matchIndex < matches.Count; matchIndex++)
                        {
                            urlParams[matchIndex] = matches[matchIndex].Value.Trim(new char[] { '{', '}' });
                        }
                    }

                    if (!routeUrl.HasValue())
                    {
                        throw new ConfigException("The route url is null or empty. Please provide a route url.");
                    }

                    physicalFile = routeMapping.PhysicalFile;

                    if (!physicalFile.HasValue())
                    {
                        throw new ConfigException("The physical path is null or empty. Please provide a physical path.");
                    }

                    var defaults = routeMapping.Defaults;
                    var constraints = routeMapping.Constraints;
                    RouteValueDictionary defaultDictionary = null;
                    RouteValueDictionary constraintDictionary = null;

                    if (defaults != null &&
                        defaults.Count > 0)
                    {
                        //{areacode}/{days}
                        //{areacode, 010},{days, 2}
                        defaultDictionary = new RouteValueDictionary();
                        for (int defaultIndex = 0; defaultIndex < defaults.Count; defaultIndex++)
                        {
                            string param = defaults[defaultIndex].Name;
                            string value = defaults[defaultIndex].Value;

                            defaultDictionary.Add(param, value);
                        }
                    }

                    if (constraints != null &&
                        constraints.Count > 0)
                    {
                        //{areacode}/{days}
                        //{ areacode, 0\d{2,3} }, { days, [1-3]{1} } 
                        constraintDictionary = new RouteValueDictionary();
                        for (int constraintIndex = 0; constraintIndex < constraints.Count; constraintIndex++)
                        {
                            string param = constraints[constraintIndex].Name;
                            string value = constraints[constraintIndex].Value;

                            constraintDictionary.Add(param, value);
                        }
                    }

                    RouteItem route = new RouteItem(routeName, routeUrl, physicalFile)
                    {
                        Defaults = defaultDictionary,
                        Constraints = constraintDictionary
                    };

                    if (!this.routes.Contains(route))
                    {
                        this.routes.Add(route);
                    }
                }
            }
        }

        public IEnumerable<RouteItem> GetRoutes()
        {
            return this.routes;
        }
    }
}
