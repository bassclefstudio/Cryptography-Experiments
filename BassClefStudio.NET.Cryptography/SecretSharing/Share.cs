using BassClefStudio.NET.Cryptography.Maths;
using System.Collections.Generic;
using System.Numerics;

namespace BassClefStudio.NET.Cryptography.SecretSharing
{
    /// <summary>
    /// Represents a piece of encoded data from the <see cref="SecretSharingService"/> in <see cref="byte"/> arrays.
    /// </summary>
    public class Share
    {
        /// <summary>
        /// A collection of <see cref="Point{T}"/>s which contain one input/output pair for each chunk of data which was encypted. Combining the nth <see cref="Point{T}"/> with that of a threshold number of other <see cref="Share"/>s will allow the original data to be computed.
        /// </summary>
        public IEnumerable<Point<BigInteger>> Points { get; set; }

        /// <summary>
        /// Creates a new <see cref="Share"/> containing an <see cref="IEnumerable{T}"/> of <see cref="Point{T}"/>s.
        /// </summary>
        public Share(IEnumerable<Point<BigInteger>> points)
        {
            Points = points;
        }
    }

    /// <summary>
    /// An object containing all the required public information to decrypt <see cref="Share"/>s from a <see cref="SecretSharingService"/> into their original contents.
    /// </summary>
    public class ShareInfo
    {
        /// <summary>
        /// The size of the values to encrypt and decrypt together, as well as the values to provide as coefficients to random polynomials, in <see cref="byte"/>s.
        /// </summary>
        public int ChunkSize { get; }

        /// <summary>
        /// The number of <see cref="Share"/>s to create for distribution. This is different to the number of shares needed to recreate the data - for that, see <see cref="PolynomialInfo{T}.Degree"/>.
        /// </summary>
        public int NumberOfShares { get; }

        /// <summary>
        /// Information about the <see cref="IPolynomial{T}"/>s created by the <see cref="SecretSharingService"/>, including their degree and the <see cref="IField{T}"/> used for computation.
        /// </summary>
        public PolynomialInfo<BigInteger> PolynomialInfo { get; }

        /// <summary>
        /// Creates a new <see cref="ShareInfo"/> from the given information.
        /// </summary>
        /// <param name="chunkSize">The size of the values to encrypt and decrypt together, as well as the values to provide as coefficients to random polynomials, in <see cref="byte"/>s.</param>
        /// <param name="numberOfShares">The number of <see cref="Share"/>s to create for distribution.</param>
        /// <param name="polynomialInfo">Information about the <see cref="IPolynomial{T}"/>s created by the <see cref="SecretSharingService"/>.</param>
        public ShareInfo(int chunkSize, int numberOfShares, PolynomialInfo<BigInteger> polynomialInfo)
        {
            ChunkSize = chunkSize;
            NumberOfShares = numberOfShares;
            PolynomialInfo = polynomialInfo;
        }
    }
}
