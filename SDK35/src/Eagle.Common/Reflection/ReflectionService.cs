using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Eagle.Common.Reflection
{
    public static class ReflectionService
    {
        public static TAttribute[] GetCustomAttributes<TAttribute>(this Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(TAttribute), false);

            return attributes as TAttribute[];
        }

        public static Type GetMemberType(this MemberInfo member)
        {
            if (member == null)
            {
                return null;
            }

            switch (member.MemberType)
            {
                case MemberTypes.Field: return (member as FieldInfo).FieldType;
                case MemberTypes.Property: return (member as PropertyInfo).PropertyType;
                case MemberTypes.Method: return (member as MethodInfo).ReturnType;
            }

            return null;
        }

        public static Type[] GetParameterTypes(this ParameterInfo[] parameters)
        {
            Guard.NotNull(parameters, "parameters");

            return (from p in parameters
                    select p.ParameterType).ToArray();
        }

        public static Type[] GetParameterTypes(this MethodBase method)
        {
            Guard.NotNull(method, "method");

            return method.GetParameters().GetParameterTypes();
        }

        public static Getter ToMemberGetter(this MemberInfo member)
        {
            Guard.NotNull(member, "member");

            switch (member.MemberType)
            {
                case MemberTypes.Field: return DynamicMethodFactory.GetGetter(member as FieldInfo);
                case MemberTypes.Property: return DynamicMethodFactory.GetGetter(member as PropertyInfo);
                case MemberTypes.Method: return DynamicMethodFactory.GetGetter(member as MethodInfo);
            }

            return null;
        }

        public static Setter ToMemberSetter(this MemberInfo member)
        {
            Guard.NotNull(member, "member");

            switch (member.MemberType)
            {
                case MemberTypes.Field: return DynamicMethodFactory.GetSetter(member as FieldInfo);
                case MemberTypes.Property: return DynamicMethodFactory.GetSetter(member as PropertyInfo);
                case MemberTypes.Method: return DynamicMethodFactory.GetSetter(member as MethodInfo);
            }

            return null;
        }

#if CSHARP30

        public static TAttribute[] GetCustomAttributes<TAttribute>(this MemberInfo member)
        {
            if (member == null)
            {
                return default(TAttribute[]);
            }

            return member.GetCustomAttributes(typeof(TAttribute), false) as TAttribute[];
        }

#endif

        internal static bool TryGetGenericInterfaceType(Type instanceType, Type targetOpenInterfaceType, out Type targetClosedInterfaceType)
        {
            // The interface must be open
            Guard.IsTrue(targetOpenInterfaceType.IsInterface);
            Guard.IsTrue(targetOpenInterfaceType.IsGenericTypeDefinition);
            Guard.IsTrue(!instanceType.IsGenericTypeDefinition);

            // if instanceType is an interface, we must first check it directly
            if (instanceType.IsInterface &&
                instanceType.IsGenericType &&
                instanceType.GetGenericTypeDefinition() == targetOpenInterfaceType)
            {
                targetClosedInterfaceType = instanceType;
                return true;
            }

            try
            {
                //  Purposefully not using FullName here because it results in a significantly
                //  more expensive component of GetInterface, this does mean that we're
                //  takign the chance that there aren't too many types which implement multiple
                //  interfaces by the same name...
                Type targetInterface = instanceType.GetInterface(targetOpenInterfaceType.Name, false);

                if (targetInterface != null &&
                    targetInterface.GetGenericTypeDefinition() == targetOpenInterfaceType)
                {
                    targetClosedInterfaceType = targetInterface;
                    return true;
                }
            }
            catch (AmbiguousMatchException)
            {
                // If there are multiple with the same name we should not pick any
            }

            targetClosedInterfaceType = null;
            return false;
        }

    }
}
