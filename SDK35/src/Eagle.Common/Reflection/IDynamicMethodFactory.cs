﻿using Eagle.Core.Attributes;
using Eagle.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Eagle.Common.Reflection
{
    /// <summary>
    /// 函数委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="arguments">函数参数</param>
    /// <returns>函数返回值</returns>
    public delegate object Func(object target, params object[] arguments);
    /// <summary>
    /// 过程委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="arguments">过程参数</param>
    public delegate void Proc(object target, params object[] arguments);


    /// <summary>
    /// 访问器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <returns></returns>
    public delegate object Getter(object target);

    /// <summary>
    /// 设置器委托
    /// </summary>
    /// <param name="target">目标对象</param>
    /// <param name="value">设置器参数</param>
    public delegate void Setter(object target, object value);

    /// <summary>
    /// 构造函数委托
    /// </summary>
    /// <param name="arguments">构造函数参数列表</param>
    /// <returns>返回创建的对象</returns>
    public delegate object ConstructorHandler(params object[] arguments);

    /// <summary>
    /// 缺省构造函数委托
    /// </summary>
    /// <returns></returns>
    public delegate object DefaultConstructorHandler();

    /// <summary>
    /// 动态方法工厂
    /// </summary>
    public static class DynamicMethodFactory
    {
        /// <summary>
        /// 得到过程委托
        /// </summary>
        /// <param name="method">方法对象</param>
        /// <returns>返回过程委托</returns>
        public static Proc GetProc(this System.Reflection.MethodInfo method)
        {
            Guard.NotNull(method, "method");

            var proc = method.DeclaringType.IsValueType
                 ? (target, args) => method.Invoke(target, args)
                 : DefaultDynamicMethodFactory.CreateProcMethod(method);


            return (target, args) =>
                {
                    if (args == null)
                        args = new object[method.GetParameters().Length];

                    try
                    {
                        proc(target, args);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
        }

        /// <summary>
        /// 得到函数委托
        /// </summary>
        /// <param name="method">方法对象</param>
        /// <returns>返回函数委托</returns>
        public static Func GetFunc(this System.Reflection.MethodInfo method)
        {
            Guard.NotNull(method, "method");

            var func = method.DeclaringType.IsValueType 
                ? (target,args)=>method.Invoke(target,args)
                : DefaultDynamicMethodFactory.CreateFuncMethod(method);

            return (target, args) =>
                {
                    if (args == null)
                        args = new object[method.GetParameters().Length];
                        
                    try
                    {
                        return func(target, args);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };
        }

        /// <summary>
        /// 得到构造函数委托
        /// </summary>
        /// <param name="constructor">构造函数</param>
        /// <returns>返回构造函数委托</returns>
        public static ConstructorHandler GetCreator(this System.Reflection.ConstructorInfo constructor)
        {
            Guard.NotNull(constructor, "constructor");

            ConstructorHandler ctor = constructor.DeclaringType.IsValueType ? 
                (args)=>constructor.Invoke(args)
                : DefaultDynamicMethodFactory.CreateConstructorMethod(constructor);

            ConstructorHandler handler = args =>
                {
                    if (args == null)
                        args = new object[constructor.GetParameters().Length];
                    try
                    {
                        return ctor(args);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw ex.InnerException;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                };

            return handler;
        }

        /// <summary>
        /// 得到缺省构造函数委托
        /// </summary>
        /// <param name="binderType"></param>
        /// <returns></returns>
        public static DefaultConstructorHandler GetDefaultCreator(this Type type)
        {
            Guard.NotNull(type, "type");
            var ctor = DefaultDynamicMethodFactory.CreateDefaultConstructorMethod(type);

            DefaultConstructorHandler handler = () =>
            {
                try
                {
                    return ctor();
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };

            return handler;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public static Getter GetGetter(this System.Reflection.MemberInfo member)
        {
            Guard.NotNull(member, "member");

            Getter getter = null;
            if (member.DeclaringType.IsValueType)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        getter = (target) => (member as FieldInfo).GetValue(target);
                        break;
                    case MemberTypes.Property:
                        getter = (target) => (member as PropertyInfo).GetValue(target, null);
                        break;
                    case MemberTypes.Method:
                        getter = (target) => (member as MethodInfo).Invoke(target, new object[] { });
                        break;
                }

            }
            else
                getter = DefaultDynamicMethodFactory.CreateGetter(member);

            return target =>
            {
                try
                {
                    return getter(target);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }

        /// <summary>
        /// 得到设置器委托
        /// </summary>
        /// <param name="member">成员</param>
        /// <returns>返回设置器委托</returns>
        public static Setter GetSetter(this System.Reflection.MemberInfo member)
        {
            Guard.NotNull(member, "member");

            Setter setter = null;
            if (member.DeclaringType.IsValueType)
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        setter = (target, value) => (member as FieldInfo).SetValue(target, value);
                        break;
                    case MemberTypes.Property:
                        setter = (target, value) => (member as PropertyInfo).SetValue(target, value,null);
                        break;
                    case MemberTypes.Method:
                        setter = (target, value) => (member as MethodInfo).Invoke(target, new object[] { value });
                        break;
                }
               
            }
            else
                setter = DefaultDynamicMethodFactory.CreateSetter(member);

            return (target,value) =>
            {
                try
                {
                    if(setter != null)
                        setter(target,value);
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            };
        }
    }

    /// <summary>
    /// 成员元数据模型
    /// </summary>
    [DebuggerDisplay("{Name}")]
    public class MemberModel
    {
        /// <summary>
        /// 成员类型
        /// </summary>
        public Type Type{ get;internal set;}
        /// <summary>
        /// 成员
        /// </summary>
        public MemberInfo Member{ get;internal set;}

        /// <summary>
        /// 成员的设置器
        /// </summary>
        public Setter SetMember{ get;internal set;}
        /// <summary>
        /// 成员的读取器
        /// </summary>
        public Getter GetMember{ get;internal set;}
        internal string Id
        {
            get
            {
                return string.Format("[{0} {1}]", Type.FullName, Member.Name);
            }
        }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string Name
        {
            get { return Member.Name; }
        }

        /// <summary>
        /// 得到成员的值
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            if (GetMember == null)
                throw new ArgumentNullException("GetMember");

            return GetMember(instance);
        }
        
        /// <summary>
        /// 设置成员的值
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        public void SetValue(object instance, object value)
        {
            if (SetMember == null)
                throw new ArgumentNullException("SetMember");

            SetMember(instance,value);
        }

        public override string ToString()
        {
            return Id;
        }
    }

    /// <summary>
    /// 成员的绑定标识。
    /// </summary>
    public static class MemberFlags
    {
        #region Fields

        /// <summary>
        /// 所有实例成员和静态成员（区分大小写）。
        /// </summary>
        public const BindingFlags AnyFlags = InstanceFlags | BindingFlags.Static;

        /// <summary>
        /// 所有实例成员（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceFlags = DefaultFlags | BindingFlags.Instance;

        /// <summary>
        /// 所有可获取的实例属性（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceGetPropertyFlags = InstanceFlags | BindingFlags.GetProperty;

        /// <summary>
        /// 所有可设置的实例属性（区分大小写）。
        /// </summary>
        public const BindingFlags InstanceSetPropertyFlags = InstanceFlags | BindingFlags.SetProperty;

        /// <summary>
        /// 所有静态成员（区分大小写）。
        /// </summary>
        public const BindingFlags StaticFlags = DefaultFlags | BindingFlags.Static;

        /// <summary>
        /// 所有可获取的静态属性（区分大小写）。
        /// </summary>
        public const BindingFlags StaticGetPropertyFlags = StaticFlags | BindingFlags.GetProperty;

        /// <summary>
        /// 所有可设置的静态属性（区分大小写）。
        /// </summary>
        public const BindingFlags StaticSetPropertyFlags = StaticFlags | BindingFlags.SetProperty;

        private const BindingFlags DefaultFlags = BindingFlags.Public | BindingFlags.NonPublic;

        #endregion Fields
    }

    /// <summary>
    /// 类型成员工厂
    /// </summary>
    public static class TypeMemberFactory
    {
        private static Dictionary<string, MemberModel[]> getMemberCache = new Dictionary<string, MemberModel[]>();
        private static Dictionary<string, MemberModel[]> setMemberCache = new Dictionary<string, MemberModel[]>();

        /// <summary>
        /// 得到指定类型具有GetValue的成员列表
        /// </summary>
        /// <param name="binderType"></param>
        /// <returns></returns>
        public static MemberModel[] GetGetMembers(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return type.GetGetMembers(BindingFlags.Public | BindingFlags.Instance);
        }
        /// <summary>
        /// 得到指定类型具有GetValue的成员列表
        /// </summary>
        /// <param name="binderType"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static MemberModel[] GetGetMembers(this Type type, BindingFlags bindingFlags)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            MemberModel[] members;
            var key = type.FullName + "||" + (long)bindingFlags;
            lock (type)
            {
                if (!getMemberCache.TryGetValue(key, out members))
                {
                    var sourceMembers = type
                               .GetFields(bindingFlags | BindingFlags.GetField)
                               .Where(p => !p.HasAttribute<IgnoreAttribute>(true))
                               .Where(p=>!p.Name.Contains("k__BackingField"))
                               .Cast<MemberInfo>()
                               .Union(type
                                   .GetProperties(bindingFlags)
                                   .Where(p => p.CanRead)
                                   .Where(p => !p.HasAttribute<IgnoreAttribute>(true))
                                   .Cast<MemberInfo>()
                                   ).Distinct();

                    members = sourceMembers.Select(p=> new MemberModel{ Type = p.GetMemberType(), Member = p, GetMember = p.GetGetter()}).ToArray();

                    getMemberCache.Add(key, members);
                }
            }
            return members;
        }

        /// <summary>
        /// 得到指定类型具有SetValue的成员列表
        /// </summary>
        /// <param name="binderType"></param>
        /// <returns></returns>
        public static MemberModel[] GetSetMembers(this Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            return type.GetSetMembers(BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// 得到指定类型具有SetValue的成员列表
        /// </summary>
        /// <param name="binderType"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static MemberModel[] GetSetMembers(this Type type,BindingFlags bindingFlags)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            MemberModel[] members;
            var key = type.FullName + "||" + (long)bindingFlags;
            lock (type)
            {
                if (!setMemberCache.TryGetValue(key, out members))
                {
                    var sourceMembers = type
                               .GetFields(bindingFlags | BindingFlags.SetField)
                               .Where(p => !p.HasAttribute<IgnoreAttribute>(true))
                               .Where(f => !f.IsInitOnly)//ensure it is editable
                               .Where(p => !p.Name.Contains("k__BackingField"))
                               .Cast<MemberInfo>()
                               .Union(type
                                   .GetProperties(bindingFlags)
                                   .Where(p => p.CanWrite)
                                   .Where(p => !p.HasAttribute<IgnoreAttribute>(true))
                                   .Cast<MemberInfo>()
                                   );

                    members = sourceMembers.Distinct().Select(p => new MemberModel { Type = p.GetMemberType(), Member = p, SetMember = p.GetSetter() }).ToArray();

                    setMemberCache.Add(key, members);
                }
            }
            return members;
        }

        static IEnumerable<PropertyInfo> GetAllProperties(Type type, BindingFlags bindingFlags)
        {
            return type.GetInterfaces().Concat(new Type[] { type }).SelectMany(itf => itf.GetProperties(bindingFlags)).Distinct();
        }
    }
}
