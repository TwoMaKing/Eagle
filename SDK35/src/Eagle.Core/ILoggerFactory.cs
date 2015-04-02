using System;

namespace Eagle.Core
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger();
    }

}
