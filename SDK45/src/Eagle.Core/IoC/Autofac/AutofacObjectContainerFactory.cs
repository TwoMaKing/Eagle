using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eagle.Core.IoC
{
    public class AutofacObjectContainerFactory : IObjectContainerFactory
    {
        public IObjectContainer ObjectContainer
        {
            get 
            {
                return new AutofacObjectContainer();
            }
        }
    }
}
