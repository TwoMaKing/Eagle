using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Plugin
{
    public interface IPluginServiceProviderFactory
    {
        IPluginServiceProvider ServiceProvider { get; }
    }
}
