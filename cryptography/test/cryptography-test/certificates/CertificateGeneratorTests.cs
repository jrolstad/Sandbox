using cryptography.certificates;
using Xunit;
using System.Security.Cryptography.X509Certificates;
using System;

namespace cryptography_test.certificates
{
    public class CertificateGeneratorTest
    {


        [Fact]
        public void ReadCerts()
        {
            using(var store = new X509Store("MY",StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                 X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

                Console.WriteLine("Number of certificates: {0}{1}",collection.Count,Environment.NewLine);

                Console.WriteLine("Reading Certs");
                foreach(var cert in store.Certificates)
                {
                    Console.WriteLine("FriendlyName:{0}",cert.FriendlyName);
                    Console.WriteLine("KeyAlgorithm:{0}",cert.GetKeyAlgorithm());
                    Console.WriteLine("IssuerName:{0}",cert.IssuerName.Name);
                    Console.WriteLine("NotBefore:{0}",cert.NotBefore);
                    Console.WriteLine("NotAfter:{0}",cert.NotAfter);
                }
            }

           
        }
    }
}