using System.Text;
using System.Security.Cryptography;

namespace cryptography.encryption
{
    
    public class HybridEncryption
    {
        // This class encrypts the data with symmetric encryption (AES) and encrypts the key to be sent with it via Asymmetric Encryption (RSA).
        // Useful for transerring data without requiring a shared symmetric key.

        public static  EncryptedPacket Encrypt(string toBeEncrypted, RSAParameters publicKey, byte[] key=null)
        {
            var stringAsBytes = Encoding.UTF8.GetBytes(toBeEncrypted);
            return Encrypt(stringAsBytes,publicKey,key);
        }

        public static EncryptedPacket Encrypt(byte[] toBeEncrypted, RSAParameters publicKey, byte[] key=null)
        {
            // Ensure we have a key
            if(key == null)
                key = SymmetricEncryption.GenerateKey(32);
            
            // Encrypt the payload
            var initializationVector = SymmetricEncryption.GenerateRandomNumber(16);
            var encryptedPayload = SymmetricEncryption.Encrypt(toBeEncrypted,key,initializationVector);
            encryptedPayload.InitializationVector = initializationVector;

            // Encrypt the key with RSA
            var encryptedKey = AsymmetricEncryption.Encrypt(key,publicKey);
            encryptedPayload.EncryptedKey = encryptedKey.EncryptedData;
            encryptedPayload.Method = "Hybrid";

            // Add a hash for integrity
            using (var hmac = new HMACSHA256(key))
            {
                encryptedPayload.Hmac = hmac.ComputeHash(encryptedPayload.EncryptedData);
            }

            return encryptedPayload;
        }

        public static  string DecryptValue(byte[] toBeDecrypted, RSAParameters privateKey, byte[] encryptedKey, byte[] initializationVector,byte[] hmac=null)
        {
            var result = Decrypt(toBeDecrypted,privateKey,encryptedKey,initializationVector,hmac);
            var resultAsString = Encoding.UTF8.GetString(result.DecryptedData);

            return resultAsString;
        }
        public static  DecryptedPacket Decrypt(byte[] toBeDecrypted, RSAParameters privateKey, byte[] encryptedKey, byte[] initializationVector,byte[] hmac=null)
        {
            var key = AsymmetricEncryption.Decrypt(encryptedKey,privateKey);

            ValidateMessageIntegrity(toBeDecrypted,key.DecryptedData,hmac);

            var decryptionResult = SymmetricEncryption.Decrypt(toBeDecrypted,key.DecryptedData,initializationVector);
            decryptionResult.InitializationVector = initializationVector;

            return decryptionResult;

        }

        private static void ValidateMessageIntegrity(byte[] toBeDecrypted, byte[] key, byte[] hashOnPacket)
        {
            if(hashOnPacket == null)
                return;

            using (var hmac = new HMACSHA256(key))
            {
                var hmacToCheck = hmac.ComputeHash(toBeDecrypted);

                if (!Compare(hashOnPacket, hmacToCheck))
                {
                    throw new CryptographicException("HMAC for decryption does not match encrypted packet.");
                }
            }
        }

        private static bool Compare(byte[] array1, byte[] array2)
        {
            // This is not the most efficient, but is safer since it will always take the longest
            var result = array1.Length == array2.Length;

            for (var i = 0; i < array1.Length && i < array2.Length; ++i)
            {
                result &= array1[i] == array2[i];
            }

            return result;
        }

    }
}