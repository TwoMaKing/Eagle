using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Plugin
{
    public interface IPluginProvider 
    {
        IDictionary<NavigationNodeItem, IEnumerable<TPluginItem>> GetPlugins<TPluginItem>() where TPluginItem : PluginItem<TPluginItem>, new();
    }

}
