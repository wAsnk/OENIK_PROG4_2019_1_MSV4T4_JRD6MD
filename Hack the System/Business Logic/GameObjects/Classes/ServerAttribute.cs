// <copyright file="ServerAttribute.cs" company="PlaceholderCompany">
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
    /// This class describe server parameters
    /// </summary>
   public class ServerAttribute : IServerAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerAttribute"/> class.
        /// </summary>
        /// <param name="newlife">Time to get new life</param>
        /// <param name="newcharge">Time to make nye charge</param>
        /// <param name="cablecapacity">Limit of used Utps</param>
        /// <param name="levelup">Level up limit</param>
        /// <param name="leveldown">Level down limit</param>
        public ServerAttribute(int newlife, int newcharge, int cablecapacity, int levelup, int leveldown)
        {
            this.NewLife = newlife;
            this.NewCharge = newcharge;
            this.CableCapacity = cablecapacity;
            this.LevelUp = levelup;
            this.LevelDown = leveldown;
        }

        /// <inheritdoc/>
        public int NewLife { get; set; }

        /// <inheritdoc/>
        public int NewCharge { get; set; }

        /// <inheritdoc/>
        public int LevelUp { get; set; }

        /// <inheritdoc/>
        public int LevelDown { get; set; }

        /// <inheritdoc/>
        public int CableCapacity { get; set; }
    }
}
