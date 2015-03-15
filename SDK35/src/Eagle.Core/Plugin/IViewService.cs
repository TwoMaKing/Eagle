﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;

namespace Eagle.Core.Plugin
{
    public interface IViewService<TPluginItem> where TPluginItem : PluginItem<TPluginItem>, new()
    {
        IPluginController<TPluginItem> CurrentPluginController { get; }

        TPluginItem CurrentPluginItem { get; }

        IPluginManager<TPluginItem> PluginManager { get; }
    }

}
