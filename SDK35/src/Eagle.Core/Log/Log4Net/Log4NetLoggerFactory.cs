using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Eagle.Core;
using log4net;

namespace Eagle.Core.Log
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger()
        {
            return new Log4NetLogger();
        }
    }
}
