namespace CC.TheBench.Frontend.Web.Security
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using Utilities.Extensions.ByteExtensions;
    using Utilities.Extensions.StringExtensions;

    public interface ISaltedHash
    {
        /// <summary>
        /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
        /// from the required byte arrays to strings. Both the Hash and Salt are returned as hex encoded strings.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to hash and salt</param>
        /// <returns>A hex encoded <see cref="System.String"/> containing the generated hash and salt</returns>
        string GetHashAndSaltString(string data);

        /// <summary>
        /// This routine provides a wrapper around VerifyHash converting the strings containing the
        /// data, hash and salt into byte arrays before calling VerifyHash.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to verify</param>
        /// <param name="hashAndSalt">A hex encoded string containing the previously stored hash and salt</param>
        /// <returns></returns>
        bool VerifyHashString(string data, string hashAndSalt);
    }

    public class SaltedHash : ISaltedHash
    {
        private readonly HashAlgorithm _hashProvider;
        private readonly int _salthLength;

        /// <summary>
        /// Default constructor which initialises the SaltedHash with the SHA256Managed algorithm
        /// and a Salt of 4 bytes (or 4*8 = 32 bits)
        /// </summary>
        public SaltedHash()
            : this(new SHA256Managed(), 4)
        {
        }

        /// <summary>
        /// The constructor takes a HashAlgorithm as a parameter.
        /// </summary>
        /// <param name="hashAlgorithm">
        /// A <see cref="HashAlgorithm"/> HashAlgorihm which is derived from HashAlgorithm. C# provides
        /// the following classes: SHA1Managed,SHA256Managed, SHA384Managed, SHA512Managed and MD5CryptoServiceProvider
        /// </param>
        /// <param name="theSaltLength"></param>
        public SaltedHash(HashAlgorithm hashAlgorithm, int theSaltLength)
        {
            _hashProvider = hashAlgorithm;
            _salthLength = theSaltLength;
        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="dataAndSalt">A byte array of the Data and Salt to Hash</param>
        /// <returns>A byte array with the calculated hash</returns>
        private byte[] ComputeHash(byte[] dataAndSalt)
        {
            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            return _hashProvider.ComputeHash(dataAndSalt);
        }

        /// <summary>
        /// Combine two byte arrays into one byte array
        /// </summary>
        /// <param name="first">First byte array to combine</param>
        /// <param name="second">Second byte array to combine</param>
        /// <returns>A byte array with the combined data and salt</returns>
        private byte[] CombineArrays(byte[] first, byte[] second)
        {
            // Allocate memory to store both the Data and Salt together
            var combinedArray = new byte[first.Length + second.Length];

            // Copy both the data and salt into the new array
            Buffer.BlockCopy(first, 0, combinedArray, 0, first.Length);
            Buffer.BlockCopy(second, 0, combinedArray, first.Length, second.Length);
            return combinedArray;
        }

        /// <summary>
        /// Given a data block this routine returns both a Hash and a Salt
        /// </summary>
        /// <param name="data">
        /// A <see cref="System.Byte"/>byte array containing the data from which to derive the salt
        /// </param>
        /// <returns>A <see cref="System.Byte"/>byte array which will contain the hash and salt generated</returns>
        public byte[] GetHashAndSalt(byte[] data)
        {
            // Allocate memory for the salt
            var salt = new byte[_salthLength];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            var random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(salt);

            // Compute hash value of our data with the salt.
            var hash = ComputeHash(CombineArrays(salt, data));

            // Prepend the salt with our hash
            return CombineArrays(salt, hash);
        }

        /// <summary>
        /// The routine provides a wrapper around the GetHashAndSalt function providing conversion
        /// from the required byte arrays to strings. Both the Hash and Salt are returned as hex encoded strings.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to hash and salt</param>
        /// <returns>A hex encoded <see cref="System.String"/> containing the generated hash and salt</returns>
        public string GetHashAndSaltString(string data)
        {
            // Obtain the Hash and Salt for the given string
            // Transform the byte[] to Hex encoded strings
            return GetHashAndSalt(Encoding.UTF8.GetBytes(data)).ToHex();
        }

        /// <summary>
        /// This routine verifies whether the data generates the same hash as we had stored previously
        /// </summary>
        /// <param name="data">The data to verify </param>
        /// <param name="hashAndSalt">The hash and salt we had stored previously</param>
        /// <returns>True on a succesfull match</returns>
        public bool VerifyHash(byte[] data, byte[] hashAndSalt)
        {
            // Pull salt out of hashAndSalt
            var salt = new byte[_salthLength];
            Buffer.BlockCopy(hashAndSalt, 0, salt, 0, _salthLength);

            var newHash = CombineArrays(salt, ComputeHash(CombineArrays(salt, data)));

            //  No easy array comparison in C# -- we do the legwork
            if (newHash.Length != hashAndSalt.Length) return false;

            for (var lp = 0; lp < hashAndSalt.Length; lp++)
                if (!hashAndSalt[lp].Equals(newHash[lp]))
                    return false;

            return true;
        }

        /// <summary>
        /// This routine provides a wrapper around VerifyHash converting the strings containing the
        /// data, hash and salt into byte arrays before calling VerifyHash.
        /// </summary>
        /// <param name="data">A UTF-8 encoded string containing the data to verify</param>
        /// <param name="hashAndSalt">A hex encoded string containing the previously stored hash and salt</param>
        /// <returns></returns>
        public bool VerifyHashString(string data, string hashAndSalt)
        {
            var hashAndSaltToVerify = hashAndSalt.ToByteArray();
            var dataToVerify = Encoding.UTF8.GetBytes(data);
            return VerifyHash(dataToVerify, hashAndSaltToVerify);
        }
    }
}