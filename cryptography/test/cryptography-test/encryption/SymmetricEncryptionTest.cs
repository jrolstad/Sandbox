using Xunit;
using cryptography.encryption;

namespace cryptography_test
{
    public class SymmetricalEncryptionTest
    {
        [Fact]
        public void EncryptedData_WithIv_IsSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var key = SymmetricEncryption.GenerateKey(32);
            var iv = SymmetricEncryption.GenerateRandomNumber(16);

            // Act
            var encryptedData = SymmetricEncryption.Encrypt(message,key,iv);
            var decryptedData = SymmetricEncryption.DecryptValue(encryptedData.EncryptedData,key,iv);

            // Assert
            Assert.Equal(message,decryptedData);
            Assert.Equal("AES",encryptedData.Method);
            Assert.NotEqual(encryptedData.EncryptedData,System.Text.Encoding.UTF8.GetBytes(message));
        }

       
    }
}