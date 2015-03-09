using System;

namespace Eagle.Core.IoC
{
    public class NInjectObjectContainerFactory : IObjectContainerFactory
    {
        public IObjectContainer ObjectContainer
        {
            get { return new NInjectObjectContainer(); }
        }
    }
}
