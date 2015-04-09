using Eagle.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace Eagle.Web.Security
{
    internal static class SecurityHelper
    {
        private const int TokenSizeInBytes = 16;

        public static string GenerateToken()
        {
            using (var cryptographic = new RNGCryptoServiceProvider())
            {
                return GenerateToken(cryptographic);
            }
        }

        public static string GenerateToken(RandomNumberGenerator generator)
        {
            byte[] tokenBytes = new byte[TokenSizeInBytes];
            generator.GetBytes(tokenBytes);
            return HttpServerUtility.UrlTokenEncode(tokenBytes);
        }

        public static string EncodePassword(string password)
        {
            return Crypto.HashPassword(password);
        }

        public static bool CheckPassword(string inputPassword, string hashedPassword)
        {
            return hashedPassword.HasValue() && 
                   Crypto.VerifyHashedPassword(hashedPassword, inputPassword);
        }
    }
}
