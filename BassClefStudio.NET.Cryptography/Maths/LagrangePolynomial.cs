using System;
using System.Linq;

namespace BassClefStudio.NET.Cryptography.Maths
{
    /// <summary>
    /// Represents an <see cref="IPolynomial{T}"/> defined by its <see cref="PolynomialInfo{T}"/> and a collection of points, which can then be interpolated in <see cref="EvaluateAt(T)"/> to get points on the <see cref="LagrangePolynomial{T}"/> for any input.
    /// </summary>
    public class LagrangePolynomial<T> : IPolynomial<T>
    {
        /// <inheritdoc/>
        public PolynomialInfo<T> Info { get; }
        public Point<T>[] Points { get; }

        public LagrangePolynomial(PolynomialInfo<T> info, Point<T>[] points)
        {
            Info = info;

            if (points.Length != Info.Degree + 1)
            {
                throw new ArgumentException("Number of points given does not correspond to the degree of the polynomial.", "points");
            }

            Points = points;
        }

        /// <inheritdoc/>
        public Point<T> EvaluateAt(T input)
        {
            T[] lNum = new T[Points.Length];
            T[] lDen = new T[Points.Length];
            for (int i = 0; i < Points.Length; i++)
            {
                var other = Points.Except(new Point<T>[] { Points[i] });
                lNum[i] = Info.Field.Product(other.Select(p => Info.Field.Subtract(input, p.Input)));
                lDen[i] = Info.Field.Product(other.Select(p => Info.Field.Subtract(p.Input, Points[i].Input)));
            }

            T Product(int i)
            {
                return Info.Field.Multiply(
                    Points[i].Output,
                    Info.Field.Divide(
                        lNum[i], lDen[i]));
            }

            T val = Product(0);
            for (int i = 1; i < Points.Length; i++)
            {
                val = Info.Field.Add(
                    val,
                    Product(i));
            }

            return new Point<T>(input, val);
        }
    }
}
