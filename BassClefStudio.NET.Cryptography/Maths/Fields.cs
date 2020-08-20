using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace BassClefStudio.NET.Cryptography.Maths
{
    /// <summary>
    /// Represents any number system which supports the four basic mathematical operators on objects of type <see cref="T"/>.
    /// </summary>
    public interface IField<T>
    {
        T Add(T a, T b);
        T Subtract(T a, T b);
        T Multiply(T a, T b);
        T Divide(T a, T b);
    }

    /// <summary>
    /// An implementation of <see cref="IField{T}"/> which can perform basic operations on <see cref="BigInteger"/>s over all integers.
    /// </summary>
    public class InfiniteField : IField<BigInteger>
    {
        public BigInteger Add(BigInteger a, BigInteger b)
        {
            return a + b;
        }

        public BigInteger Subtract(BigInteger a, BigInteger b)
        {
            return a - b;
        }

        public BigInteger Multiply(BigInteger a, BigInteger b)
        {
            return a * b;
        }

        public BigInteger Divide(BigInteger a, BigInteger b)
        {
            return a / b;
        }        
    }

    /// <summary>
    /// An implementation of <see cref="IField{T}"/> which can perform basic operations on <see cref="BigInteger"/>s over a prime finite field.
    /// </summary>
    public class FiniteField : IField<BigInteger>
    {
        public BigInteger FieldSize { get; }

        public FiniteField(BigInteger fieldSize)
        {
            FieldSize = fieldSize;
        }

        #region Operations

        public BigInteger Add(BigInteger a, BigInteger b)
        {
            return Mod(a + b);
        }

        public BigInteger Subtract(BigInteger a, BigInteger b)
        {
            return Mod(a - b);
        }

        public BigInteger Multiply(BigInteger a, BigInteger b)
        {
            return Mod(Mod(a) * Mod(b));
        }

        public BigInteger Divide(BigInteger a, BigInteger b)
        {
            return Multiply(a, ModInverse(b));
        }

        private BigInteger Mod(BigInteger a)
        {
            BigInteger r = a % FieldSize;
            return r < 0 ? r + FieldSize : r;
        }

        private BigInteger ModInverse(BigInteger a)
        {
            BigInteger m0 = FieldSize;
            BigInteger y = 0, x = 1;

            if (FieldSize == 1)
                return 0;

            while (a > 1)
            {
                BigInteger q = a / m0;
                BigInteger t = m0;

                m0 = a % FieldSize;
                a = t;
                t = y;

                y = x - q * y;
                x = t;
            }

            // Make x positive 
            if (x < 0)
                x += FieldSize;

            return x;
        }

        #endregion
    }

    public static class FieldExtensions
    {
        public static T Pow<T>(this IField<T> field, T a, int b)
        {
            if (b < 1)
            {
                throw new ArgumentException("Power must be a positive integer.", "b");
            }
            else
            {
                T value = a;
                for (int i = 1; i < b - 1; i++)
                {
                    value = field.Multiply(value, a);
                }
                return value;
            }
        }

        public static T Product<T>(this IField<T> field, IEnumerable<T> values)
        {
            if (values.Any())
            {
                T product = values.First();
                foreach (var v in values.Skip(1))
                {
                    product = field.Multiply(product, v);
                }

                return product;
            }
            else
            {
                throw new ArgumentException("The values collection cannot be empty.", "values");
            }
        }
    }
}
