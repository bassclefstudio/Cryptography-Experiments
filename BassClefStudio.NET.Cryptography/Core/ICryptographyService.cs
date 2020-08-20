namespace BassClefStudio.NET.Cryptography.Core
{
    /// <summary>
    /// Represents a service that can encrypt one type of data <see cref="TIn"/> into another type of data <see cref="TOut"/>.
    /// </summary>
    public interface IEncryptionService<in TIn, out TOut>
    {
        /// <summary>
        /// Encodes a piece of <paramref name="data"/> type <typeparamref name="TIn"/> as data of type <typeparamref name="TOut"/>.
        /// </summary>
        TOut Encrypt(TIn data);
    }

    /// <summary>
    /// Represents a service that can decrypt one type of data <see cref="TIn"/> from another type of data <see cref="TOut"/>.
    /// </summary>
    public interface IDecryptionService<out TIn, in TOut>
    {
        /// <summary>
        /// Reconstructs or decodes a piece of data type <typeparamref name="TIn"/> from input <paramref name="data"/> of type <typeparamref name="TOut"/>.
        /// </summary>
        TIn Decrypt(TOut data);
    }

    /// <summary>
    /// Represents a service that can encrypt and decrypt one type of data <see cref="TIn"/> to and from another type of data <see cref="TOut"/>.
    /// </summary>
    public interface ICryptographyService<TIn, TOut> : IEncryptionService<TIn, TOut>, IDecryptionService<TIn, TOut>
    { }
}
