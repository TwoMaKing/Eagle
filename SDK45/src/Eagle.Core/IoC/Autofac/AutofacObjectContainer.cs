using Autofac;
using Autofac.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.IoC
{
    public class AutofacObjectContainer : IObjectContainer
    {
        public void InitializeFromConfigFile(string configSectionName, string configFilePath = "")
        {
            throw new NotImplementedException();
        }

        public T GetWrapperContainer<T>()
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type t)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type t, ObjectLifetimeMode lifeitme)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type t, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type t, string name, ObjectLifetimeMode lifeitme)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type from, Type to, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterType(Type from, Type to, string name, ObjectLifetimeMode lifeitme)
        {
            throw new NotImplementedException();
        }

        public void RegisterType<TFrom>(Type to, string name)
        {
            throw new NotImplementedException();
        }

        public void RegisterType<TFrom>(Type to, string name, ObjectLifetimeMode lifeitme)
        {
            throw new NotImplementedException();
        }

        public void RegisterType<TFrom, TTo>(string name) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public void RegisterType<TFrom, TTo>(string name, ObjectLifetimeMode lifeitme) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(params ResolverParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name, params ResolverParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type t, params ResolverParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type t, string name, params ResolverParameter[] parameters)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        public bool Registered(Type t)
        {
            throw new NotImplementedException();
        }

        public bool Registered(Type t, string name)
        {
            throw new NotImplementedException();
        }

        public bool Registered<T>()
        {
            throw new NotImplementedException();
        }

        public bool Registered<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Type> TypesFrom
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<Type> TypesMapTo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
