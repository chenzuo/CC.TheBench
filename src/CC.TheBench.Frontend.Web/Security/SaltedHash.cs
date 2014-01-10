namespace CC.TheBench.Frontend.Web.Security
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
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
        private readonly int _salthLength;
        private readonly int _hashLength;
        private readonly int _numberOfIterations;

        /// <summary>
        /// Default constructor which initialises the SaltedHash with a Salt of 32 bytes (or 32*8 = 256 bits),
        /// a Hash of 64 bytes (or 64*8 = 512 bits) and 1000 PBKDF2 iterations
        /// </summary>
        public SaltedHash()
            : this(32, 64, 1000)
        {
        }

        /// <summary>
        /// The constructor takes a salt length, hash length and number of PBKDF2 iterations as parameters
        /// </summary>
        /// <param name="saltLength">Length of the salt to generate in bytes</param>
        /// <param name="hashLength">Length of the hash to generate in bytes</param>
        /// <param name="numberOfIterations">Number of PBKDF2 iterations to use on the hashing function</param>
        public SaltedHash(int saltLength, int hashLength, int numberOfIterations)
        {
            _salthLength = saltLength;
            _hashLength = hashLength;
            _numberOfIterations = numberOfIterations;
        }

        /// <summary>
        /// The actual hash calculation is shared by both GetHashAndSalt and the VerifyHash functions
        /// </summary>
        /// <param name="salt">A byte array of the Salt to Hash</param>
        /// <param name="data">A byte array of the Data to Hash</param>
        /// <returns>A byte array with the calculated hash</returns>
        private byte[] ComputeHash(byte[] salt, byte[] data)
        {
            // Calculate the hash
            // Compute hash value of our plain text with appended salt.
            using (var pbkdf2 = new Rfc2898DeriveBytes(data, salt, _numberOfIterations))
            {
                return pbkdf2.GetBytes(_hashLength);
            }
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
        /// Compares two byte arrays, always taking the same amount of time to prevent timing attacks
        /// </summary>
        /// <param name="first">First byte array to compare</param>
        /// <param name="second">Second byte array to compare</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private bool CompareArrays(IList<byte> first, IList<byte> second)
        {
            var diff = first.Count ^ second.Count;

            for (var i = 0; i < first.Count && i < second.Count; i++)
                diff |= first[i] ^ second[i];

            return diff == 0;
        }

        /// <summary>
        /// Generate strong random bytes
        /// </summary>
        /// <param name="count">Number of random bytes needed</param>
        /// <returns></returns>
        private byte[] GenerateRandomBytes(int count)
        {
            // Allocate memory for the salt
            var bytes = new byte[count];

            // Strong runtime pseudo-random number generator, on Windows uses CryptAPI
            // on Unix /dev/urandom
            var random = new RNGCryptoServiceProvider();

            // Create a random salt
            random.GetNonZeroBytes(bytes);

            return bytes;
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
            // Get a random salt
            var salt = GenerateRandomBytes(_salthLength);
            
            // Compute hash value of our data with the salt.
            var hash = ComputeHash(salt, data);

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

            var newHash = CombineArrays(salt, ComputeHash(salt, data));

            return CompareArrays(newHash, hashAndSalt);
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