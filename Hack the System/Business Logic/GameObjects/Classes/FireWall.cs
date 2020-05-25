// <copyright file="FireWall.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// This object block Utp Cabels
    /// </summary>
    [Serializable]
    public class FireWall : IFireWall
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FireWall"/> class.
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public FireWall(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <inheritdoc/>
        public double X { get; set; }

        /// <inheritdoc/>
        public double Y { get; set; }

        /// <inheritdoc/>
        public void Tick()
        {
        }
    }
}
