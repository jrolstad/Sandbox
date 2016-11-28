using Xunit;
using cryptography.encryption;

namespace cryptography_test.encryption
{
    public class HybridEncryptionTest
    {
        [Fact]
        public void EncryptedData_WithKey_IsSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var keyPair = AsymmetricEncryption.GenerateKey(32);

            // Act
            var encryptedData = HybridEncryption.Encrypt(message,keyPair.PublicKey);
            var decryptedData = HybridEncryption.DecryptValue(encryptedData.EncryptedData,keyPair.PrivateKey,encryptedData.EncryptedKey,encryptedData.InitializationVector);

            // Assert
            Assert.Equal(message,decryptedData);
            Assert.NotEqual(encryptedData.EncryptedData,System.Text.Encoding.UTF8.GetBytes(message));
        }

        [Fact]
        public void EncryptedData_WithModifiedHMAC_IsNotSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var keyPair = AsymmetricEncryption.GenerateKey(32);

            var encryptedData = HybridEncryption.Encrypt(message,keyPair.PublicKey);
            encryptedData.Hmac[1] = byte.MaxValue;

             // Act 
            var exception = Assert.ThrowsAny<System.Security.Cryptography.CryptographicException>(()=>{HybridEncryption.DecryptValue(encryptedData.EncryptedData,keyPair.PrivateKey,encryptedData.EncryptedKey,encryptedData.InitializationVector,encryptedData.Hmac);});

            // Assert
            Assert.Contains("HMAC for decryption does not match encrypted packet",exception.Message);
        }
    }
}