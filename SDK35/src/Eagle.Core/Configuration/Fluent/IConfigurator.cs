﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Configuration.Fluent
{
    /// <summary>
    /// Represents that the implemented classes are configuration configurators.
    /// </summary>
    /// <typeparam name="TContainer">The type of the object container.</typeparam>
    public interface IConfigurator<TContainer>
    {
        TContainer Configure();
    }
}
