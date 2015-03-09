﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.List
{
    public interface IHierarchicalEntity<TParent, TChildren> where TChildren : IEnumerable<TParent>
    {
        TParent Parent { get; }

        TChildren SubItems { get; }
    }
}
