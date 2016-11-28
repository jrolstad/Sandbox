namespace cryptography.encryption
{
    public class EncryptedPacket
    {
        public byte[] EncryptedKey {get;set;}
        public byte[] EncryptedData {get;set;}
        public byte[] InitializationVector {get;set;}
        public string Method {get;set;}
    }

    public class DecryptedPacket
    {
        public byte[] DecryptedData;
        public byte[] InitializationVector;
        public string Method {get;set;}
    }
}