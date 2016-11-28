using System.Security.Cryptography;

namespace cryptography.encryption
{
    public class RSAKeyPair
    {
        public RSAParameters PublicKey {get;set;}
        public RSAParameters PrivateKey {get;set;}
    }
}