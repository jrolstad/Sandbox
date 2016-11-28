using cryptography;
using Xunit;

namespace cryptography_test.hashing
{
    public class SHAHashTest
    {
        [Fact]
        public void ComputeHash_WithoutSalt_ComputesSameHash()
        {
            // Arrange
            var secret = "my super secret password";

            // Act
            var result1 = SHAHash.ComputeHash(secret);
            var result2 = SHAHash.ComputeHash(secret);

            // Assert
            Assert.Equal(result1.HashedData,result2.HashedData);
            Assert.Equal(result1.Salt,result2.Salt);
        }

        [Fact]
        public void ComputeHash_WithSalt_ComputesSameHash()
        {
            // Arrange
            var secret = "my super secret password";
            var salt = SHAHash.GenerateSalt(128);

            // Act
            var result1 = SHAHash.ComputeHash(secret,salt);
            var result2 = SHAHash.ComputeHash(secret,salt);

            // Assert
            Assert.Equal(result1.HashedData,result2.HashedData);
            Assert.Equal(salt,result2.Salt);
        }

        [Fact]
        public void ComputeHash_WithDifferentSalt_ComputesDifferentHash()
        {
            // Arrange
            var secret = "my super secret password";
            var salt1 = SHAHash.GenerateSalt(128);
            var salt2 = SHAHash.GenerateSalt(128);

            // Act
            var result1 = SHAHash.ComputeHash(secret,salt1);
            var result2 = SHAHash.ComputeHash(secret,salt2);

            // Assert
            Assert.NotEqual(result1.HashedData,result2.HashedData);
            Assert.Equal(salt1,result1.Salt);
            Assert.Equal(salt2,result2.Salt);
        }

        [Fact]
        public void ComputeHash_WithoutSalt_ComputesDifferentHashForDifferentValues()
        {
            // Arrange
            var secret1 = "my super secret password";
            var secret2 = "my super secret password, part deux";

            // Act
            var result1 = SHAHash.ComputeHash(secret1);
            var result2 = SHAHash.ComputeHash(secret2);

            // Assert
            Assert.NotEqual(result1.HashedData,result2.HashedData);
            Assert.Equal(result1.Salt,result2.Salt);
        }

         [Fact]
        public void ComputeHash_WithSalt_ComputesDifferentHashForDifferentValues()
        {
            // Arrange
            var secret1 = "my super secret password";
            var secret2 = "my super secret password, part deux";

            var salt = SHAHash.GenerateSalt(128);

            // Act
            var result1 = SHAHash.ComputeHash(secret1,salt);
            var result2 = SHAHash.ComputeHash(secret2,salt);

            // Assert
            Assert.NotEqual(result1.HashedData,result2.HashedData);
            Assert.Equal(salt,result1.Salt);
            Assert.Equal(salt,result2.Salt);
        }
    }
}