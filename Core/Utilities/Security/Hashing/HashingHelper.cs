using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public static class HashingHelper
    {
        public static bool VerifyPasswordHash(byte[] passwordHash,byte[] passwordSalt,string password)
        {
            using (var hasher = new HMACSHA512(passwordSalt))
            {
                var computedHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
                for(int i = 0;i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public static void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt)
        {
            using (var hasher = new HMACSHA512())
            {
                passwordSalt = hasher.Key;
                passwordHash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
