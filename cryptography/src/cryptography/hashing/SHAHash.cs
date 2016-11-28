using System;
using System.Security.Cryptography;
using System.Text;

namespace cryptography
{
    public class SHAHash
    {
        public static byte[] GenerateSalt(int saltSize)
        {
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[saltSize];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public static HashedPacket ComputeHash(string toBeHashed, byte[] salt=null)
        {
            var stringAsBytes = Encoding.UTF8.GetBytes(toBeHashed);
            return ComputeHash(stringAsBytes,salt);
        }
        
        public static HashedPacket ComputeHash(byte[] toBeHashed, byte[] salt=null)
        {
            using(var sha = SHA256.Create())
            {
                
                var valueToHash = Combine(toBeHashed,salt);
                var hashedValue = sha.ComputeHash(valueToHash);

                return new HashedPacket {
                    HashedData = hashedValue,
                    Salt = salt,
                    Method = "SHA256"
                };
            }
        }

        private static byte[] Combine(byte[] first, byte[] second)
        {
            if(first == null)
                return second;
            
            if(second == null)
                return first;

            var combinedArray = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, combinedArray, 0, first.Length);
            Buffer.BlockCopy(second, 0, combinedArray, first.Length, second.Length);

            return combinedArray;
        }

        
    }
}