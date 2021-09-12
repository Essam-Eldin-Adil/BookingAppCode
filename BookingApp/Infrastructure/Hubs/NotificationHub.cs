using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
       


        public void PushNotifications(Guid meetingId)
        {
            Clients.All.SendCoreAsync("PushNotifications", new object[] { meetingId });
        }


    }
}
