// <copyright file="IServerNetworkController.cs" company="PlaceholderCompany">
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
    /// Server can produce charges
    /// </summary>
    public interface IServerNetworkController : IActiveNetworkController, IServerAttributes
    {
        /// <summary>
        /// Gets level of server
        /// </summary>
        int Level { get; }

        /// <summary>
        /// Gets passed time to get new life
        /// </summary>
        int NewLifePoint { get; }

        /// <summary>
        /// Gets passed time to make new charge
        /// </summary>
        int NewChargePoint { get; }
    }
}
