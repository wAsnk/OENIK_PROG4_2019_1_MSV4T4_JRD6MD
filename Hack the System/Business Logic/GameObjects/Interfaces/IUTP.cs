// <copyright file="IUTP.cs" company="PlaceholderCompany">
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

    /// <summary>
    /// State of an Utp
    /// </summary>
    public enum UtpModes
    {
        /// <summary>
        /// Move to target
        /// </summary>
        Attack,

        /// <summary>
        /// Move to parent
        /// </summary>
        Back,

        /// <summary>
        /// Move to half way
        /// </summary>
        MoveToBattle,

        /// <summary>
        /// Utp arrived to target
        /// </summary>
        ConnectedToServer,

        /// <summary>
        /// Utp arrived to half way
        /// </summary>
        Battle
    }

    /// <summary>
    /// Activities of Utp
    /// </summary>
    public interface IUtp : IChargeble
    {
        /// <summary>
        /// Gets networkcontroller whitc create this Utp
        /// </summary>
        IActiveNetworkController Creator { get; }

        /// <summary>
        /// Gets handled cables
        /// </summary>
        List<ICable> Cables { get; }

        /// <summary>
        /// Gets a value indicating whether can utp treasport chagres
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Gets first of cables
        /// </summary>
        ICable First { get; }

        /// <summary>
        /// Gets last of cables
        /// </summary>
        ICable Last { get; }

        /// <summary>
        /// Gets parent networkcontroller
        /// </summary>
        IActiveNetworkController Parent { get; }

        /// <summary>
        /// Gets target networkcontroller
        /// </summary>
        INetworkController Target { get; }

        /// <summary>
        /// Gets or sets mode state of Utp
        /// </summary>
        UtpModes Mode { get; set; }

        /// <summary>
        /// Gets a value indicating whether if move back to parent networkcontroller
        /// </summary>
        bool Disconnect { get; }

        /// <summary>
        /// Call back the utp
        /// </summary>
        void Aback();

        /// <summary>
        /// This method run on every timer tick event
        /// </summary>
        void Tick();

        /// <summary>
        /// Cute utp to 2 pice
        /// </summary>
        /// <param name="cable">Point of cut</param>
        void Cut(ICable cable);
    }
}
