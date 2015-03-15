using Eagle.Common;
using Eagle.Common.Serialization;
using Eagle.Core;
using Eagle.Core.Application;
using Eagle.Core.Configuration;
using Eagle.Data;
using Eagle.Data.Mapping;
using Eagle.Data.Queries;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Eagle.Tests.Domain.Models;

namespace Eagle.Tests
{
    public class TestBase
    {
        public TestBase() 
        {
            AppRuntime.Instance.Create(new EAppConfigSource(AppDomain.CurrentDomain.BaseDirectory + "Eagle.Tests\\bin\\Debug\\Eagle.Tests.dll.config"));
        }
    }
}
