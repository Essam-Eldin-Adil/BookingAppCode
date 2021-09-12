using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingApp.Infrastructure
{
    public class ConnectionManager : IConnectionManager
    {
        private static Dictionary<string, HashSet<string>> userMap = new Dictionary<string, HashSet<string>>();
 
        public void Add(string username, string connectionId)
        {
            lock (userMap)
            {
                if (!userMap.ContainsKey(username))
                {
                    userMap[username] = new HashSet<string>();
                }
                userMap[username].Add(connectionId);
            }
        }

        public HashSet<string> GetConnection(string username)
        {
            var connection = new HashSet<string>();
            try
            {
                lock (userMap)
                {
                    connection = userMap[username];
                }
            }
            catch
            {
                connection = null;
            }
            return connection;
        }

        IEnumerable<string> IConnectionManager.OnlineUsers { get { return userMap.Keys; } }

        public void Remove(string connectionId)
        {
            lock (userMap)
            {
                foreach(var username in userMap.Keys)
                {
                    if (!userMap.ContainsKey(username))
                    {
                        if (!userMap[username].Contains(connectionId))
                        {
                            userMap[username].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }
    }
}
