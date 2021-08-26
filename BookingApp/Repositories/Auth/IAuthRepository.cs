using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Auth
{
    public interface IAuthRepository
    {
        public Task<User> Login(string username, string password);

        public Task<User> Register(User user);

        public Task<bool> UserExists(string username);
    }
}
