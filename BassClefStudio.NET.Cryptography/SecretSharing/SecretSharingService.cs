using BassClefStudio.NET.Cryptography.Core;
using BassClefStudio.NET.Cryptography.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;

namespace BassClefStudio.NET.Cryptography.SecretSharing
{
    /// <summary>
    /// Represents an <see cref="ICryptographyService{TIn, TOut}"/> that encrypts and decrypts an array of <see cref="byte"/>s to and from collections of <see cref="Share"/>s, which can be distributed and later re-combined to retreive the original data.
    /// </summary>
    public class SecretSharingService : ICryptographyService<byte[], IEnumerable<Share>>
    {
        /// <summary>
        /// The <see cref="ShareInfo"/> of this <see cref="SecretSharingService"/> instance describes how <see cref="Share"/>s should be encrypted and decrypted.
        /// </summary>
        public ShareInfo ShareInfo { get; }

        /// <summary>
        /// Creates a new <see cref="SecretSharingService"/>.
        /// </summary>
        /// <param name="info">Information about how this <see cref="SecretSharingService"/> should create and decrypt <see cref="Share"/>s.</param>
        public SecretSharingService(ShareInfo info)
        {
            ShareInfo = info;
        }

        /// <inheritdoc/>
        public IEnumerable<Share> Encrypt(byte[] data)
        {
            var inputs = Enumerable.Range(0, ShareInfo.NumberOfShares).ToArray();
            return data.ChunkBy(ShareInfo.ChunkSize)
                .Select(c => EncryptChunk(c, inputs))
                .Transpose()
                .Select(p => new Share(p));
        }

        private IEnumerable<Point<BigInteger>> EncryptChunk(IEnumerable<byte> data, int[] inputs)
        {
            BigInteger[] coeffs = new BigInteger[ShareInfo.PolynomialInfo.Degree + 1];
            using (var rng = RandomNumberGenerator.Create())
            {
                coeffs[0] = new BigInteger(data.ToArray());
                for (int i = 1; i < coeffs.Length; i++)
                {
                    byte[] randomBytes = new byte[ShareInfo.ChunkSize];
                    rng.GetBytes(randomBytes);
                    coeffs[i] = new BigInteger(randomBytes);
                }
            }

            var poly = new CoefficientPolynomial<BigInteger>(ShareInfo.PolynomialInfo, coeffs);
            return inputs.Select(i => poly.EvaluateAt(i));
        }

        /// <inheritdoc/>
        public byte[] Decrypt(IEnumerable<Share> data)
        {
            return data.Select(s => s.Points)
                .Transpose()
                .SelectMany(ps => DecryptChunk(ps))
                .ToArray();
        }

        private byte[] DecryptChunk(IEnumerable<Point<BigInteger>> points)
        {
            var poly = new LagrangePolynomial<BigInteger>(ShareInfo.PolynomialInfo, points.ToArray());
            return poly.EvaluateAt(0).Output.ToByteArray();
        }
    }

    static class ListExtensions
    {
        public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkSize)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value));
        }

        public static IEnumerable<IEnumerable<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            
            if(!source.Any())
            {
                return Enumerable.Empty<IEnumerable<T>>();
            }

            return TransposeCore(source);
        }

        static IEnumerable<IEnumerable<T>> TransposeCore<T>(this IEnumerable<IEnumerable<T>> source)
        {
            var enumerators = source.Select(x => x.GetEnumerator()).ToArray();
            try
            {
                while (enumerators.All(x => x.MoveNext()))
                {
                    yield return enumerators.Select(x => x.Current).ToArray();
                }
            }
            finally
            {
                foreach (var enumerator in enumerators)
                    enumerator.Dispose();
            }
        }
    }
}