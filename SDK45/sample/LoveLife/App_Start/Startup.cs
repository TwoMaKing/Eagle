using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(LoveLife.Startup))]
namespace LoveLife
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR("/PostPush", typeof(PostPushConnection), new ConnectionConfiguration());
        }
    }
    
}