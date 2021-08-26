using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public static class Security
    {
        public static void CreatePasswordHashAndSalt(string input, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
            }

        }

        public static bool VerifyPasswordHash(string input, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
                for(int i=0;i < computeHash.Length; i++)
                {
                    if(computeHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }


}
