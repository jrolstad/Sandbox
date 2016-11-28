namespace cryptography.encryption
{
    public class EncryptedPacket
    {
        public byte[] EncryptedSessionKey;
        public byte[] EncryptedData;
        public byte[] InitializationVector;
        public string Method {get;set;}
    }

    public class DecryptedPacket
    {
        public byte[] DecryptedData;
        public byte[] InitializationVector;
        public string Method {get;set;}
    }
}