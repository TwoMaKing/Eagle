﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.Configuration.Fluent
{
    public interface IEAppConfigurator : IConfigSourceConfigurator 
    { 
    
    }

    public class EAppConfigurator : IEAppConfigurator
    {
        private RegularConfigSource configSource = new RegularConfigSource();

        public RegularConfigSource Configure()
        {
            return this.configSource;
        }
    }
}
