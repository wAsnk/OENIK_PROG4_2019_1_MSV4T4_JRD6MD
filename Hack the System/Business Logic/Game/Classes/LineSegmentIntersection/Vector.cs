// <copyright file="Vector.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Classes.LineSegmentIntersection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper to line segmentation
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        public Vector()
            : this(double.NaN, double.NaN)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Gets or sets x coordinate
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets y coordinate
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="w">Second member</param>
        /// <returns>Result</returns>
        public static Vector operator -(Vector v, Vector w)
        {
            return new Vector(v.X - w.X, v.Y - w.Y);
        }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="w">Second member</param>
        /// <returns>Result</returns>
        public static bool operator ==(Vector v, Vector w)
        {
            if (v.X == w.X && v.Y == w.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Vector subtraction
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="w">Second member</param>
        /// <returns>Result</returns>
        public static bool operator !=(Vector v, Vector w)
        {
            if (v.X != w.X && v.Y != w.Y)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Vector summation
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="w">Second member</param>
        /// <returns>Result</returns>
        public static Vector operator +(Vector v, Vector w)
        {
            return new Vector(v.X + w.X, v.Y + w.Y);
        }

        /// <summary>
        /// Multiplication with vector
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="w">Second member</param>
        /// <returns>Resoult</returns>
        public static double operator *(Vector v, Vector w)
        {
            return (v.X * w.X) + (v.Y * w.Y);
        }

        /// <summary>
        /// Multiplication with scalar
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="mult">Second member</param>
        /// <returns>Resoult</returns>
        public static Vector operator *(Vector v, double mult)
        {
            return new Vector(v.X * mult, v.Y * mult);
        }

        /// <summary>
        /// multiplication with scalar
        /// </summary>
        /// <param name="v">First member</param>
        /// <param name="mult">Second member</param>
        /// <returns>Resoult</returns>
        public static Vector operator *(double mult, Vector v)
        {
            return new Vector(v.X * mult, v.Y * mult);
        }

        /// <summary>
        /// Cross of vector
        /// </summary>
        /// <param name="v">First member</param>
        /// <returns>Resoult</returns>
        public double Cross(Vector v)
        {
            return (this.X * v.Y) - (this.Y * v.X);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            var v = (Vector)obj;
            return (this.X - v.X).IsZero() && (this.Y - v.Y).IsZero();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("({0};{1})", this.X, this.Y);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
