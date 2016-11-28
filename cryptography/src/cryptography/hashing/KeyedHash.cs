using System;
using System.Security.Cryptography;
using System.Text;

namespace cryptography
{
    public class KeyedHash
    {
        public static byte[] GenerateKey(int keySize)
        {
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[keySize];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }    

         public static byte[] GenerateSalt(int saltSize)
        {
            using(var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                var randomNumber = new byte[saltSize];
                randomNumberGenerator.GetBytes(randomNumber);

                return randomNumber;
            }
        }    

        public static HashedPacket ComputeHash(string toBeHashed, byte[] key, byte[] salt=null)
        {
            var stringAsBytes = Encoding.UTF8.GetBytes(toBeHashed);
            return ComputeHash(stringAsBytes,key, salt);
        }

        public static HashedPacket ComputeHash(byte[] toBeHashed, byte[] key, byte[] salt=null)
        {
            using(var hmac = new HMACSHA256(key))
            {
                var valueToHash = Combine(toBeHashed,salt);
                var hashedValue = hmac.ComputeHash(valueToHash);

                return new HashedPacket{
                    HashedData = hashedValue,
                    Method = hmac.HashName,
                    Salt = salt
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