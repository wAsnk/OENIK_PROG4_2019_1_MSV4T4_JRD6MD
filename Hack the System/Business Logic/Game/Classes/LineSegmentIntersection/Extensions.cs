// <copyright file="Extensions.cs" company="PlaceholderCompany">
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
    public static class Extensions
    {
        private const double Epsilon = 1e-10;

        /// <summary>
        /// This methode decide the parameter is small enough
        /// </summary>
        /// <param name="d">Number</param>
        /// <returns>Resoult</returns>
        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }
    }
}
