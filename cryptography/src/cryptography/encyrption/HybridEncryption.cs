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
            if(key == null)
                key = SymmetricEncryption.GenerateKey(32);
            
            var initializationVector = SymmetricEncryption.GenerateRandomNumber(16);

            var encryptedPayload = SymmetricEncryption.Encrypt(toBeEncrypted,key,initializationVector);
            encryptedPayload.InitializationVector = initializationVector;

            var encryptedKey = AsymmetricEncryption.Encrypt(key,publicKey);
            encryptedPayload.EncryptedKey = encryptedKey.EncryptedData;
            encryptedPayload.Method = "Hybrid";

            return encryptedPayload;
        }

        public static  string DecryptValue(byte[] toBeDecrypted, RSAParameters privateKey, byte[] encryptedKey, byte[] initializationVector)
        {
            var result = Decrypt(toBeDecrypted,privateKey,encryptedKey,initializationVector);
            var resultAsString = Encoding.UTF8.GetString(result.DecryptedData);

            return resultAsString;
        }
        public static  DecryptedPacket Decrypt(byte[] toBeDecrypted, RSAParameters privateKey, byte[] encryptedKey, byte[] initializationVector)
        {
            var key = AsymmetricEncryption.Decrypt(encryptedKey,privateKey);

            var decryptionResult = SymmetricEncryption.Decrypt(toBeDecrypted,encryptedKey,initializationVector);
            decryptionResult.InitializationVector = initializationVector;

            return decryptionResult;

        }

    }
}