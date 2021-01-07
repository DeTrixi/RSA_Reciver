using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RSAReciver
{
    class RsaDecrypt
    {
        public RSAParameters _publicKey;
        public RSAParameters _privateKey;

        /// <summary>
        /// creates a private and a public key
        /// </summary>
        public void AssignNewKey()
        {
            using var rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;
            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);
        }

        /// <summary>
        /// Decrypts the data if key is the right one  
        /// </summary>
        /// <param name="dataToEncrypt"></param>
        /// <returns></returns>
        public byte[] DecryptData(byte[] dataToEncrypt)
        {
            byte[] plain;

            using var rsa = new RSACryptoServiceProvider(2048);
            rsa.PersistKeyInCsp = false;

            rsa.ImportParameters(_privateKey);
            plain = rsa.Decrypt(dataToEncrypt, true);

            return plain;
        }
    }
}