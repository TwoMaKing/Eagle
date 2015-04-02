using Eagle.Common.Serialization;
using Eagle.Tests.DataObjects;
using LoveLife.Model;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoveLife
{
   
    public class PostPushConnection : PersistentConnection
    {
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            return base.OnConnected(request, connectionId);
        }
  
        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            string posts = SerializationManager.SerializeToJson(PostModels.AllPosts);

            return this.Connection.Send(connectionId, posts);
        }
    }
}