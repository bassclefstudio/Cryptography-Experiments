using System;

namespace BassClefStudio.NET.Cryptography.Maths
{
    /// <summary>
    /// Represents an <see cref="IPolynomial{T}"/> defined by its <see cref="PolynomialInfo{T}"/> and the list of coefficients of that polynomial (which can be used to simply find points with <see cref="EvaluateAt(T)"/>).
    /// </summary>
    public class CoefficientPolynomial<T> : IPolynomial<T>
    {
        /// <inheritdoc/>
        public PolynomialInfo<T> Info { get; }

        public T[] Coefficients { get; }

        public CoefficientPolynomial(PolynomialInfo<T> info, T[] coeffs)
        {
            Info = info;

            if(coeffs.Length != Info.Degree + 1)
            {
                throw new ArgumentException("Number of coefficients does not correspond to the degree of the polynomial.", "coeffs");
            }

            Coefficients = coeffs;
        }

        /// <inheritdoc/>
        public Point<T> EvaluateAt(T input)
        {
            T value = Coefficients[0];
            for (int i = 1; i < Coefficients.Length; i++)
            {
                value = Info.Field.Add(value, Info.Field.Multiply(Coefficients[i], Info.Field.Pow(input, i)));
            }
            return new Point<T>(input, value);
        }
    }
}
