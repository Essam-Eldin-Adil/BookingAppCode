using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure
{
    public interface IConnectionManager
    {
        void Add(string username, string connectionId);
        void Remove(string connectionId);
        HashSet<string> GetConnection(string username);
        IEnumerable<string> OnlineUsers { get; }

    }
}
