using Xunit;
using cryptography.encryption;

namespace cryptography_test.encryption
{
    public class AsymmetricEncryptionTest
    {
        [Fact]
        public void EncryptedData_WithKey_IsSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var key = AsymmetricEncryption.GenerateKey(32);

            // Act
            var encryptedData = AsymmetricEncryption.Encrypt(message,key.PublicKey);
            var decryptedData = AsymmetricEncryption.DecryptValue(encryptedData.EncryptedData,key.PrivateKey);

            // Assert
            Assert.Equal(message,decryptedData);
            Assert.Equal("RSA",encryptedData.Method);
            Assert.NotEqual(encryptedData.EncryptedData,System.Text.Encoding.UTF8.GetBytes(message));
        }

        [Fact]
        public void EncryptedData_DecryptWithPublicKey_IsNotSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var key = AsymmetricEncryption.GenerateKey(32);

            var encryptedData = AsymmetricEncryption.Encrypt(message,key.PublicKey);

            // Act / Assert
            Assert.ThrowsAny<System.Security.Cryptography.CryptographicException>(()=>{AsymmetricEncryption.DecryptValue(encryptedData.EncryptedData,key.PublicKey);});

        }
    }
}