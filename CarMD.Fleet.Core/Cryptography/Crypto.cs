namespace CarMD.Fleet.Core.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Summary description for Crypto.
    /// </summary>
    public class Crypto
    {
        private static string IV;
        private static string Key;

        /// <summary>
        /// Encryption providers
        /// </summary>
        public enum Provider
        {
            DES,
            RC2,
            Rijndael,
            TripleDES
        }

        public void TripleDESSetup(string iv, string key)
        {
            IV = iv;
            Key = key;
        }

        /// <summary>
        /// Create object and specify cryptographic provider
        /// </summary>
        /// <param name="provider">Provider to user while performing encryption/decryption</param>
        public Crypto(Provider provider)
        {
            IV = string.Empty;
            Key = string.Empty;
            switch (provider)
            {
                case Provider.DES:
                    this.provider = new DESCryptoServiceProvider();
                    break;
                case Provider.RC2:
                    this.provider = new RC2CryptoServiceProvider();
                    break;
                case Provider.Rijndael:
                    this.provider = new RijndaelManaged();
                    break;
                case Provider.TripleDES:
                    IV = "SuFjcEmp/TE=";
                    Key = "KIPSToILGp6fl+3gXJvMsN4IajizYBBT";
                    this.provider = new TripleDESCryptoServiceProvider();
                    break;
            }
        }

        /// <summary>
        /// Create object and specify a custom provider
        /// </summary>
        /// <param name="provider">Provider to use for encryption/decryption</param>
        public Crypto(SymmetricAlgorithm provider)
        {
            this.provider = provider;
        }

        /// <summary>
        /// Convert string to legal encryption key
        /// </summary>
        /// <param name="key">String to convert</param>
        /// <returns>Legal encryption key</returns>
        private byte[] GetLegalKey(string key) // TODO: Support max key size
        {
            string result = key;

            if (this.provider.LegalBlockSizes.Length > 0)
            {
                KeySizes sizes = this.provider.LegalBlockSizes[0];

                if (sizes.MinSize % 8 != 0)
                    throw new Exception("Non byte-aligned key sizes are not supported.");

                int bytes = sizes.MinSize / 8;

                if (result.Length > bytes)
                    result = result.Substring(0, bytes);
                else if (result.Length < bytes)
                    result = result.PadRight(bytes, ' ');
            }

            // Convert key to byte array and return
            return ASCIIEncoding.ASCII.GetBytes(result);
        }

        public byte[] Encrypt(byte[] source, string key)
        {
            // Create a MemoryStream so that the process can be done without I/O files
            var ms = new System.IO.MemoryStream();

            // Create encryption key from key string

            byte[] bytKey = null;
            if (!string.IsNullOrEmpty(key))
                bytKey = GetLegalKey(key);

            // Set the key and initialization vector
            if (string.IsNullOrEmpty(Key))
                this.provider.Key = bytKey;
            else
                this.provider.Key = System.Convert.FromBase64String(Key);

            if (string.IsNullOrEmpty(IV))
                this.provider.IV = bytKey;
            else
                this.provider.IV = System.Convert.FromBase64String(IV);

            // Create an Encryptor from the Provider Service instance
            ICryptoTransform encrypto = this.provider.CreateEncryptor();

            // Create CryptoStream that transforms a stream using the encryption
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            // Write out encrypted content into MemoryStream
            cs.Write(source, 0, source.Length);

            // Complete last write
            cs.FlushFinalBlock();

            var dest = new byte[ms.Length];

            Array.Copy(ms.GetBuffer(), 0, dest, 0, dest.Length);

            return dest;
        }

        public byte[] Decrypt(byte[] source, string key)
        {
            // Create a MemoryStream with the input
            var ms = new System.IO.MemoryStream(source, 0, source.Length);

            // Create decryption key from key string
            byte[] bytKey = GetLegalKey(key);

            // Set the key and initialization vector
            this.provider.Key = bytKey;
            this.provider.IV = bytKey;

            // Create a Decryptor from the Provider Service instance
            ICryptoTransform encrypto = this.provider.CreateDecryptor();

            // Create Crypto Stream that transforms a stream using the decryption
            var cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            var buffer = new byte[source.Length];

            int read = cs.Read(buffer, 0, buffer.Length);

            var dest = new byte[read];

            Array.Copy(buffer, 0, dest, 0, dest.Length);

            return dest;
        }

        /// <summary>
        /// Encrypt source string using key and return result string
        /// </summary>
        /// <param name="source">String to encrypt</param>
        /// <param name="key">Encryption key</param>
        /// <returns>Encrypted string</returns>
        public string Encrypt(string source, string key)
        {
            // Create input byte buffer from source sring
            byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(source);

            // Create a MemoryStream so that the process can be done without I/O files
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            // Create encryption key from key string
            byte[] bytKey = GetLegalKey(key);

            if (!(this.provider is TripleDESCryptoServiceProvider))
            {
                // Set the key and initialization vector
                this.provider.Key = bytKey;
                this.provider.IV = bytKey;
            }
            else
            {
                // HOA: ADDED THE NEXT 2 LINES
                this.provider.Key = Convert.FromBase64String(Crypto.Key);
                this.provider.IV = Convert.FromBase64String(Crypto.IV);
            }

            // Create an Encryptor from the Provider Service instance
            ICryptoTransform encrypto = this.provider.CreateEncryptor();

            // Create CryptoStream that transforms a stream using the encryption
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

            // Write out encrypted content into MemoryStream
            cs.Write(bytIn, 0, bytIn.Length);

            // Complete last write
            cs.FlushFinalBlock();

            // Get output length
            int len = (int)ms.Length;

            // Get the output buffer
            byte[] bytOut = ms.GetBuffer();

            // Convert into Base64 so that the result can be used in Xml
            return System.Convert.ToBase64String(bytOut, 0, len);
        }

        /// <summary>
        /// Decrypt source string using key and return result string
        /// </summary>
        /// <param name="source">String to decrypt</param>
        /// <param name="key">Decryption key</param>
        /// <returns>Decrypted string</returns>
        public string Decrypt(string source, string key)
        {
            // Convert string from Base64 to binary data
            byte[] bytIn = System.Convert.FromBase64String(source);

            // Create a MemoryStream with the input
            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            // Create decryption key from key string
            byte[] bytKey = GetLegalKey(key);

            // HOA: ADDED AN IF CHECK FOR TRIPLEDES
            if (!(this.provider is TripleDESCryptoServiceProvider))
            {
                // Set the key and initialization vector
                this.provider.Key = bytKey;
                this.provider.IV = bytKey;
            }
            else
            {
                // HOA ADDED 2 LINES BELOW
                this.provider.Key = Convert.FromBase64String(Crypto.Key);
                this.provider.IV = Convert.FromBase64String(Crypto.IV);
            }

            // Create a Decryptor from the Provider Service instance
            ICryptoTransform encrypto = this.provider.CreateDecryptor();

            // Create Crypto Stream that transforms a stream using the decryption
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            // Read out the result from the Crypto Stream
            System.IO.StreamReader sr = new System.IO.StreamReader(cs);

            // Return decrypted string
            return sr.ReadToEnd();
        }

        /// <summary>
        /// Analyze source string to detect if it has already been encrypted
        /// </summary>
        /// <param name="source">String to analyze</param>
        /// <returns>Returns true if it is likely that the string has already been encrypted</returns>
        public static bool ProbablyEncrypted(string source)
        {
            try
            {
                // Encrypted data will be base64 encoded so try to decode it
                Convert.FromBase64String(source);

                // Decoding succeeded so data probably is encrypted
                return true;
            }

            // Base64 decodng failed
            catch
            {
                // Probably not encrypted
                return false;
            }
        }

        public static ushort GenerateSeed()
        {
            return 13003;
        }

        public static string GenerateKey(ushort seed)
        {
            byte[] key = new byte[16];

            for (int i = 0; i < key.Length; i++)
            {
                key[i] = (byte)seed;
                seed += 59999;
            }

            StringBuilder str = new StringBuilder();

            foreach (byte b in key)
                str.Append((char)b);

            return str.ToString();
        }

        public static string HashPassword(string password)
        {
            try
            {
                var enc = Encoding.Unicode.GetEncoder();
                var unicodeText = new byte[password.Length * 2];
                enc.GetBytes(password.ToCharArray(), 0, password.Length, unicodeText, 0, true);
                // Now that we have a byte array we can ask the CSP to hash it
                var md5 = new MD5CryptoServiceProvider();
                var result = md5.ComputeHash(unicodeText);
                // Build the final string by converting each byte
                // into hex and appending it to a StringBuilder
                var sb = new StringBuilder();
                for (var i = 0; i < result.Length; i++)
                    sb.Append(result[i].ToString("X2"));
                return sb.ToString();
            }
            catch
            {
                return String.Empty;
            }
        }

        private SymmetricAlgorithm provider; // Encryption provider to encrypt/decrypt the data
    }
}