using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eagle.Core.IoC
{
    public class ResolverParameter
    {
        public ResolverParameter(string parameterName, object parameterValue)
        {
            this.ParameterName = parameterName;
            this.ParameterValue = parameterValue;
        }

        public string ParameterName 
        { 
            get; 
            private set; 
        }

        public object ParameterValue
        {
            get;
            private set;
        }
    }
}
