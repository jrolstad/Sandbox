using Xunit;
using cryptography.encryption;

namespace cryptography_test.encryption
{
    public class HybridEncryptionTest
    {
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
    }
}