using System;
using System.ComponentModel;

namespace Eagle.Core
{
    public class EventArgs<T> : EventArgs
    {
        private T data;

        public EventArgs(T data) 
        { 
            this.data = data;
        }

        public T Data
        {
            get
            {
                return this.data;
            }
        }
    }

}
