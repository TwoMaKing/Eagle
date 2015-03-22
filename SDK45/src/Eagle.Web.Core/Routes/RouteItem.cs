using Eagle.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Eagle.Web.Core.Routes
{
    public class RouteItem :RouteBase
    {
        public RouteItem(string name, string url, string physicalFile)
            : this(name, url, physicalFile, null, null, null)
        {
        }

        public RouteItem(string name, string url, string physicalFile, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens) 
        {
            this.Name = name;
            this.Url = url;
            this.PhysicalFile = physicalFile;
            this.Defaults = defaults;
            this.Constraints = constraints;
            this.DataTokens = dataTokens;
        }

        /// <summary>
        /// Gets or sets a dictionary of expressions that specify valid values for a
        //  URL parameter.
        /// </summary>
        public RouteValueDictionary Constraints { get; set; }

        /// <summary>
        /// Gets or sets custom values that are passed to the route handler, but which
        /// are not used to determine whether the route matches a URL pattern.
        /// </summary>
        public RouteValueDictionary DataTokens { get; set; }

        /// <summary>
        /// Gets or sets the values to use if the URL does not contain all the parameters.
        /// </summary>
        public RouteValueDictionary Defaults { get; set; }

        /// <summary>
        /// Gets or sets the object that processes requests for the route.
        /// </summary>
        public string PhysicalFile { get; set; }

        /// <summary>
        ///  Gets or sets the URL pattern for the route.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            RouteItem other = obj as RouteItem;
            if (other == ((RouteItem)null))
            {
                return false;
            }

            return this.Name.Equals(other.Name);
        }

        public override int GetHashCode()
        {
            return Utils.GetHashCode(this.Name.GetHashCode(),
                                     this.Url.GetHashCode(),
                                     this.PhysicalFile.GetHashCode(),
                                     this.Defaults == null ? 0 : this.Defaults.GetHashCode(),
                                     this.Constraints == null ? 0 : this.Constraints.GetHashCode());
        }

        public override RouteData GetRouteData(System.Web.HttpContextBase httpContext)
        {
            return new RouteData(this, new PageRouteHandler(this.Url, true));
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return new VirtualPathData(this, requestContext.HttpContext.Request.Path);
        }
    }
}
