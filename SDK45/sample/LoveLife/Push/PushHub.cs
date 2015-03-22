using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace LoveLife.Push
{
    [HubName("PostPushHub")]
    public class PushHub : Hub
    {
        public void Push(string connectionId)
        {
            //this.Clients.Caller.on
        }

        public void Exist(string userId)
        {
            string connectionId = this.Context.ConnectionId;

            this.Clients.Client(connectionId).onExist(connectionId, userId);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

    }
}