﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Eagle.Core;
using log4net;

namespace Eagle.Core.Log
{
    public sealed class LoggerContext
    {
        private static ILoggerFactory currentLoggerFactory;

        static LoggerContext() 
        {
            currentLoggerFactory = CreateLoggerFactory("");
        }

        public static ILogger CurrentLogger
        {
            get 
            {
                return currentLoggerFactory.CreateLogger();
            }
        }

        public static ILoggerFactory CreateLoggerFactory(string loggerFactoryName)
        {
            return null;
        }

    }
}
