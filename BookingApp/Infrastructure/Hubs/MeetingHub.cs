using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure.Hubs
{
    public class MeetingHub: Hub
    {
        public void LoadMembers(Guid meetingId)
        {
            Clients.All.SendCoreAsync("LoadMembers", new object[] { meetingId });        
        }
        public void PrepareAll(Guid meetingId)
        {
            Clients.All.SendCoreAsync("PrepareAll", new object[] { meetingId });
        }
        public void AbsentAll(Guid meetingId)
        {
            Clients.All.SendCoreAsync("AbsentAll", new object[] { meetingId });
        }
        public void LoadAddMember(Guid meetingId)
        {
            Clients.All.SendCoreAsync("LoadAddMember", new object[] { meetingId });
        }
    }
}
