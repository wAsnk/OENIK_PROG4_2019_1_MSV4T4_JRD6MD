// <copyright file="ICable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Classes;

    /// <summary>
    /// This interface represent a pice of cable which can carry charge
    /// </summary>
    public interface ICable : IGameObject, IChargeble
    {
        /// <summary>
        /// Gets or sets next game object which can be Cable or networkcontroller
        /// </summary>
        IChargeble NextGameObject { get; set; }

        /// <summary>
        /// Gets or sets previous game object which can be Cable or networkcontroller
        /// </summary>
        IChargeble Previous { get; set; }

        /// <summary>
        /// Gets charge owner player
        /// </summary>
        IPlayer ChargeOwner { get; }

        /// <summary>
        /// Gets so far delay
        /// </summary>
        int DelayPoint { get; }

        /// <summary>
        /// Gets delay of cahrge forward
        /// </summary>
        int Delay { get; }
    }
}
