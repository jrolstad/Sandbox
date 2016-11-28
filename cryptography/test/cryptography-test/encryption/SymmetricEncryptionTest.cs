using Xunit;
using cryptography.encryption;

namespace cryptography_test.encryption
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

        public void EncryptedData_WithDifferentIv_IsNotSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var key = SymmetricEncryption.GenerateKey(32);
            var iv1 = SymmetricEncryption.GenerateRandomNumber(16);
            var iv2 = SymmetricEncryption.GenerateRandomNumber(16);

            // Act
            var encryptedData = SymmetricEncryption.Encrypt(message,key,iv1);
            var decryptedData = SymmetricEncryption.DecryptValue(encryptedData.EncryptedData,key,iv2);

            // Assert
            Assert.NotEqual(message,decryptedData);
           
        }

         public void EncryptedData_WithDifferentKey_IsNotSuccessfullyDecrypted()
        {
            // Arrange
            var message = "my super secret message";
            var key1 = SymmetricEncryption.GenerateKey(32);
            var key2 = SymmetricEncryption.GenerateKey(32);
            var iv = SymmetricEncryption.GenerateRandomNumber(16);

            // Act
            var encryptedData = SymmetricEncryption.Encrypt(message,key1,iv);
            var decryptedData = SymmetricEncryption.DecryptValue(encryptedData.EncryptedData,key2,iv);

            // Assert
            Assert.NotEqual(message,decryptedData);
           
        }

       
    }
}