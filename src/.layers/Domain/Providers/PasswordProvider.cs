using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Pharmacies.Domain.Providers
{
    public sealed class PasswordProvider
    {
        private const int IterCount = 10000;

        public static string Encrypt(string password)
        {
            using (var random = RandomNumberGenerator.Create())
            {
                return Convert.ToBase64String(HashPasswordV3(password, random));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name = "provided" > The hash value for a user's stored password. </param>
        /// <param name = "correct" > The password supplied for comparison. </param>
        /// <remarks> Implementations of this method should be time consistent. </remarks>
        public static bool Verify(string provided, string correct)
        {
            var decoded = Convert.FromBase64String(correct);

            // read the format marker from the hashed password
            if (decoded.Length == 0)
            {
                return false;
            }

            try
            {
                // Read header information
                var PRF = (KeyDerivationPrf) ReadNetworkByteOrder(decoded, 1);
                var saltLength = (int) ReadNetworkByteOrder(decoded, 9);

                // Read the salt: must be >= 128 bits
                if (saltLength < 128 / 8)
                {
                    return false;
                }

                var salt = new byte[saltLength];
                Buffer.BlockCopy(decoded, 13, salt, 0, salt.Length);

                // Read the subkey (the rest of the payload): must be >= 128 bits
                var subkeyLength = decoded.Length - 13 - salt.Length;

                if (subkeyLength < 128 / 8)
                {
                    return false;
                }

                var expected = new byte[subkeyLength];
                Buffer.BlockCopy(decoded, 13 + salt.Length, expected, 0, expected.Length);

                // Hash the incoming password and verify it
                var actual = KeyDerivation.Pbkdf2(provided, salt, PRF, IterCount, subkeyLength);

                return ByteArraysEqual(actual, expected);
            }
            catch
            {
                // This should never occur except in the case of a malformed payload, where
                // we might go off the end of the array. Regardless, a malformed payload
                // implies verification failed.
                return false;
            }
        }

        // Compares two byte arrays for equality. The method is specifically written so that the loop is not optimized.
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;

            for (var i = 0; i < a.Length; i++)
            {
                areSame &= a[i] == b[i];
            }

            return areSame;
        }

        private static byte[] HashPasswordV3
            (string password,
             RandomNumberGenerator random,
             KeyDerivationPrf prf = KeyDerivationPrf.HMACSHA256,
             int iterCount = IterCount,
             int saltSize = 128 / 8,
             int numBytesRequested = 256 / 8)
        {
            // Produce a version 3 (see comment above) text hash.
            var salt = new byte[saltSize];
            random.GetBytes(salt);
            var subkey = KeyDerivation.Pbkdf2(password, salt, prf, iterCount, numBytesRequested);
            var outputBytes = new byte[13 + salt.Length + subkey.Length];
            outputBytes[0] = 0x01; // format marker
            WriteNetworkByteOrder(outputBytes, 1, (uint) prf);
            WriteNetworkByteOrder(outputBytes, 5, (uint) iterCount);
            WriteNetworkByteOrder(outputBytes, 9, (uint) saltSize);
            Buffer.BlockCopy(salt, 0, outputBytes, 13, salt.Length);
            Buffer.BlockCopy(subkey, 0, outputBytes, 13 + saltSize, subkey.Length);

            return outputBytes;
        }

        private static void WriteNetworkByteOrder(byte[] buffer, int offset, uint value)
        {
            buffer[offset + 0] = (byte) (value >> 24);
            buffer[offset + 1] = (byte) (value >> 16);
            buffer[offset + 2] = (byte) (value >> 8);
            buffer[offset + 3] = (byte) (value >> 0);
        }

        private static uint ReadNetworkByteOrder(byte[] buffer, int offset)
        {
            return ((uint) buffer[offset + 0] << 24)
                | ((uint) buffer[offset + 1] << 16)
                | ((uint) buffer[offset + 2] << 8)
                | buffer[offset + 3];
        }
    }
}
