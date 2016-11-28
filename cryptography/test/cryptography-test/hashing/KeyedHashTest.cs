using cryptography;
using Xunit;

namespace cryptography_test
{
    public class KeyedHashTest
    {
        [Fact]
        public void ComputeHash_SameKeyWithoutSalt_ComputesSameHash()
        {
            // Arrange
            var secret = "my super secret password";
            var key = KeyedHash.GenerateKey(128);

            // Act
            var result1 = KeyedHash.ComputeHash(secret,key);
            var result2 = KeyedHash.ComputeHash(secret,key);

            // Assert
            Assert.Equal(result1.HashedData,result2.HashedData);
        }

        [Fact]
        public void ComputeHash_SameKeyWithSalt_ComputesSameHash()
        {
            // Arrange
            var secret = "my super secret password";
            var key = KeyedHash.GenerateKey(128);
            var salt = KeyedHash.GenerateSalt(128);
            // Act
            var result1 = KeyedHash.ComputeHash(secret,key,salt);
            var result2 = KeyedHash.ComputeHash(secret,key,salt);

            // Assert
            Assert.Equal(result1.HashedData,result2.HashedData);
            Assert.Equal(salt,result1.Salt);
            Assert.Equal(salt,result2.Salt);
        }

        [Fact]
        public void ComputeHash_SameKeyWithDifferentSalt_ComputesDifferentHash()
        {
            // Arrange
            var secret = "my super secret password";
            var key = KeyedHash.GenerateKey(128);
            var salt1 = KeyedHash.GenerateSalt(128);
            var salt2 = KeyedHash.GenerateSalt(128);

            // Act
            var result1 = KeyedHash.ComputeHash(secret,key,salt1);
            var result2 = KeyedHash.ComputeHash(secret,key,salt2);

            // Assert
            Assert.NotEqual(result1.HashedData,result2.HashedData);
            Assert.Equal(salt1,result1.Salt);
            Assert.Equal(salt2,result2.Salt);
        }

         [Fact]
        public void ComputeHash_DifferentKeyWithoutSalt_ComputesDifferentHash()
        {
            // Arrange
            var secret = "my super secret password";
            var key1 = KeyedHash.GenerateKey(128);
            var key2 = KeyedHash.GenerateKey(128);

            // Act
            var result1 = KeyedHash.ComputeHash(secret,key1);
            var result2 = KeyedHash.ComputeHash(secret,key2);

            // Assert
            Assert.NotEqual(result1.HashedData,result2.HashedData);
        }
    }
}