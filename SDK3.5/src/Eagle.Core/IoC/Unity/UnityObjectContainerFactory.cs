using System;

namespace Eagle.Core.IoC
{
    public class UnityObjectContainerFactory : IObjectContainerFactory
    {
        public IObjectContainer ObjectContainer
        {
            get 
            {
                return new UnityObjectContainer();
            }
        }
    }
}
