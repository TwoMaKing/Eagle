using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Eagle.Core.List
{
    public interface IEntityArrayList
    {
        event NotifyCollectionChangedEventHandler ArrayListChanged;
    }
}
