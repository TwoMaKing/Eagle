using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.WindowsMvc
{
    public interface IControllerFactory
    {
        IController CreateController(string controllerName);

        IController CreateController(Type controllerType);

        Type GetControllerType(string controllerName);
    }
}
