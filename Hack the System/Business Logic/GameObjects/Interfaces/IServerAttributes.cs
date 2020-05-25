// <copyright file="IServerAttributes.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface describe server parameters
    /// </summary>
    public interface IServerAttributes
    {
        /// <summary>
        /// Gets time to get new life
        /// </summary>
        int NewLife { get; }

        /// <summary>
        /// Gets time to make nye charge
        /// </summary>
        int NewCharge { get; }

        /// <summary>
        /// Gets limit of used Utps
        /// </summary>
        int LevelUp { get; }

        /// <summary>
        /// Gets level up limit
        /// </summary>
        int LevelDown { get; }

        /// <summary>
        /// Gets level down limit
        /// </summary>
        int CableCapacity { get; }
    }
}
