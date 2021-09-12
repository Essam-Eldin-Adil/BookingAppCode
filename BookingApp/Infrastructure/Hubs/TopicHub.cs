using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure.Hubs
{
    public class TopicHub : Hub
    {

        public void StartTimer(Guid topicId)
        {
            Clients.All.SendCoreAsync("StartTimer", new object[] { topicId });
        }
        public void StopTimer(Guid topicId)
        {
            Clients.All.SendCoreAsync("StopTimer", new object[] { topicId });
        }

        public void LoadComments(Guid topicId)
        {
            Clients.All.SendCoreAsync("LoadComments", new object[] {topicId });
        }
        public void LoadFiles(Guid topicId)
        {
            Clients.All.SendCoreAsync("LoadFiles", new object[] { topicId });
        }
        public void LoadPolls(Guid topicId)
        {
            Clients.All.SendCoreAsync("LoadPolls", new object[] { topicId });
        }
        public void LoadDecisions(Guid topicId)
        {
            Clients.All.SendCoreAsync("LoadDecisions", new object[] { topicId });
        }
        public void LoadRemarks(Guid topicId)
        {
            Clients.All.SendCoreAsync("LoadRemarks", new object[] { topicId });
        }

        //public void PollAdded(Guid pollId, Guid topicId)
        //{
        //    Clients.All.SendCoreAsync("PollAdded", new object[] { pollId, topicId });
        //}
        //public void PollDeleted(Guid pollId, Guid topicId)
        //{
        //    Clients.All.SendCoreAsync("PollDeleted", new object[] { pollId, topicId });
        //}
        //public void PollUpdated(Guid pollId, Guid topicId)
        //{
        //    Clients.All.SendCoreAsync("PollUpdated", new object[] { pollId, topicId });
        //}
    }
}
