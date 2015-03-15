using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core.List;

namespace Eagle.Core.Plugin
{
    public class PluginItemCollection<TPluginItem> : EntityArrayList<TPluginItem> where TPluginItem : PluginItem<TPluginItem>, new()
    {

    }
}
