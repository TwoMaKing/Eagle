using Eagle.Common.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Domain
{
    internal class TypePropertiesCache
    {
        private readonly static Dictionary<Type, PropertyInfo[]> typePropertiesCache = new Dictionary<Type, PropertyInfo[]>();

        public PropertyInfo[] CreateAndCacheTypeProperties(Type type)
        {
            if (!typePropertiesCache.ContainsKey(type))
            {
                PropertyInfo[] properties = ReflectionHelper.DeepGetProperties(type);

                typePropertiesCache.Add(type, properties);

                return properties;
            }
            else
            {
                return typePropertiesCache[type];
            }
        }
    }
}
