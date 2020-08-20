# BassClefStudio.NET.Cryptography
This is a basic library where I'm experimenting with various forms of cryptographic functions, secret sharing systems, and the like. It's designed to be simple and easy to try out new things.

***THIS IS A PROJECT FOR EDUCATION/EXPERIMENTAL PURPOSES ONLY!*** I am no security expert, and do not reccommend using any of these libraries in production as-is.

# Implemented Features:
## Shamir's Secret Sharing System
Shamir's Secret Sharing System ([Wikipedia](https://en.wikipedia.org/wiki/Shamir%27s_Secret_Sharing)) is a system devised (evidently) by Adi Shamir. The idea is that any value that one wanted to keep hidden from any individual could be separated, and shared, between multiple people. No one person has enough information to reconstruct the secret - in fact, with any threshold of _n_ people, _n-1_ shares provides no information about what the original value was!

This algorithm is based on the concepts of polynomials. For a polynomial of degree 1 (a line), 1 point gives no information about any other point on the line. However, with 2 points, one can easily calculate any other point on the line. Shamir's Secret Sharing takes a polynomial of degree _n-1_, with a _y_-intercept of the secret data, and gives points to some number of people. _n_ points must be found to be able to calculate the _y_-intercept. [Lagrange interpolation](https://en.wikipedia.org/wiki/Lagrange_polynomial) is used on these higher-degree polynomials to calculate any point (_x_,_y_) from the known points. Polynomials, however, are smooth curves, and to eliminate the guessing advantages of such smooth slopes, finite field artithmetic is used, which (in very basic terms) defines arithmetic operations that use a set of values instead of the whole number line (in this case, whole numbers from 0 to a given prime).

For more information, [this video](https://youtu.be/K54ildEW9-Q) from Stand-Up-Maths provides an easy-to-understand overview of the kinds of maths used in the algorithm. This repository should also serve as an easy-to-understand example of how to implement such an algorithm in C#.
