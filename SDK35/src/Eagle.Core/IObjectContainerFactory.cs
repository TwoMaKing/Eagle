﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core
{
    public interface IObjectContainerFactory
    {
        IObjectContainer ObjectContainer { get; }
    }
}
