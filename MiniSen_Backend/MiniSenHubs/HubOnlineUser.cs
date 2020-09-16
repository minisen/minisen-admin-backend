using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniSen_Backend.MiniSenHubs
{
    public class HubOnlineUser
    {
        public string ConnectionId { get; set; }
        public string Account { get; set; }
        public DateTime OnlineLastTime { get; set; }

        public void SendSysMessage(Hub hub, string messageJson)
        {
            hub.Clients.Clients(this.ConnectionId).SendAsync("ReceiveSysMessage", messageJson);
        }
    }
}
