﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core.List;
using Eagle.Core.Plugin;

namespace Eagle.Core.Plugin
{
    public class PluginControllerCollection<TPluginItem> : EntityArrayList<IPluginController<TPluginItem>> where TPluginItem : PluginItem<TPluginItem>, new()
    {
        public PluginControllerCollection() { }

        public IPluginController<TPluginItem> this[string pluginName]
        {
            get 
            {
                if (string.IsNullOrEmpty(pluginName))
                {
                    return null;
                }

                return this.SingleOrDefault(p => p.PluginItem != null &&
                                                 pluginName.Equals(p.PluginItem.Name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public IPluginController<TPluginItem> this[string navigationName, string pluginName]
        {
            get 
            {
                if (string.IsNullOrEmpty(navigationName) ||
                    string.IsNullOrEmpty(pluginName))
                {
                    return null;
                }

                return this.SingleOrDefault(p => p.PluginItem != null &&
                                                 p.PluginItem.Navigation != null &&
                                                 navigationName.Equals(p.PluginItem.Navigation.Name, StringComparison.InvariantCultureIgnoreCase) &&
                                                 pluginName.Equals(p.PluginItem.Name, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        public IPluginController<TPluginItem> RunningPluginController
        {
            get 
            {
                return this.SingleOrDefault(p => p.Running == true);
            }
        }
    }
}
