namespace BassClefStudio.NET.Cryptography.Maths
{
    /// <summary>
    /// Represents a polynomial function with values of type <typeparamref name="T"/>, which contains <see cref="PolynomialInfo{T}"/> information about the type of polynomial and a method to evaluate the <see cref="IPolynomial{T}"/> at a given input.
    /// </summary>
    /// <typeparam name="T">The type of input and output values.</typeparam>
    public interface IPolynomial<T>
    {
        /// <summary>
        /// A <see cref="PolynomialInfo{T}"/> containing information about the composition and properties of the <see cref="IPolynomial{T}"/>.
        /// </summary>
        PolynomialInfo<T> Info { get; }

        /// <summary>
        /// Evaluates the <see cref="IPolynomial{T}"/> at the given input and returns a <see cref="Point{T}"/> containing the input and output.
        /// </summary>
        /// <param name="input">The input parameter, or x-value.</param>
        Point<T> EvaluateAt(T input);
    }

    /// <summary>
    /// Represents all of the important infomation needed to reconstruct or create a <see cref="Polynomial"/>.
    /// </summary>
    public class PolynomialInfo<T>
    {
        public int Degree { get; set; }
        public IField<T> Field { get; }

        public PolynomialInfo(int degree, IField<T> field)
        {
            Degree = degree;
            Field = field;
        }
    }

    /// <summary>
    /// Represents a point with input and output values of type <typeparamref name="T"/>, obtained through a <see cref="IPolynomial{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the input and output.</typeparam>
    public class Point<T>
    {
        public T Input { get; }
        public T Output { get; }

        public Point(T input, T output)
        {
            Input = input;
            Output = output;
        }
    }
}
