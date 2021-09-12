using Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Auth
{
    public class AuthRepository : IAuthRepository
    {
      
        public Task<User> Login(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> Register(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UserExists(string username)
        {
            throw new NotImplementedException();
        }


    }
}
