using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace BinkodApp.Web.Chat
{
    public class ChatHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public void Send(string name, string message, string group)
        {
            // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);

        }

        public void JoinGroup(string groupName)
        {
            Groups.Add(Context.ConnectionId, groupName);
            //Clients.All.ShowGroup(groupName);
        }

        public void Group(string user = "", string message = "", string group = "")
        {
            //Clients.All.addMessage(message);
            //Groups.Add(Context.ConnectionId, group);
            //Clients.Others.addMessage(message)
            Clients.Group(group).addMessage(message);
        }

        public void LeaveGroup(string group = "")
        {
            Groups.Remove(Context.ConnectionId, group);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}