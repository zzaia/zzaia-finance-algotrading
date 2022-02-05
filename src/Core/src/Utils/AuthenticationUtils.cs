using System;
using System.Security.Cryptography;
using System.Text;

namespace MarketIntelligency.Core.Utils
{
    public static class AuthenticationUtils
    {
        /// <summary>
        /// Computes the HMACSHA512 keyed hash for a given message;
        /// </summary>
        /// <param name="key">The string key</param>
        /// <param name="sourceMessage">The source string message</param>
        /// <returns>The HMACSHA512 keyed hash as a Hexdecimal string</returns>
        public static string SignMessage(string key, string sourceMessage)
        {
            // Initialize the keyed hash object:
            using HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(key));
            // Compute the hash of the source message:
            byte[] hashValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(sourceMessage));
            return BitConverter.ToString(hashValue).ToLowerInvariant().Replace("-", String.Empty);
        }

        /// <summary>
        /// Computes the HMACSHA512 keyed hash for a given message;
        /// </summary>
        /// <param name="key">The byte array key</param>
        /// <param name="sourceMessage">The source byte array message</param>
        /// <returns>The HMACSHA512 keyed hash as a Hexdecimal string</returns>
        public static string SignMessage(byte[] key, byte[] sourceMessage)
        {
            // Initialize the keyed hash object:
            using HMACSHA512 hmac = new HMACSHA512(key);
            // Compute the hash of the source message:
            byte[] hashValue = hmac.ComputeHash(sourceMessage);
            return BitConverter.ToString(hashValue).ToLowerInvariant().Replace("-", String.Empty);
        }
    }
}
