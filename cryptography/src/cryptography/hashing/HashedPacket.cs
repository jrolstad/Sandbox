namespace cryptography
{
    public class HashedPacket
    {
        public byte[] HashedData {get;set;}
        public byte[] Salt {get;set;}
        public string Method {get;set;}

    }
}