// <copyright file="IPlayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// Categories of players
    /// </summary>
    public enum PlayerType
    {
        /// <summary>
        /// Human player
        /// </summary>
        LocalHumanPlayer,

        /// <summary>
        /// A easy computer controlled
        /// </summary>
        EasyCpu,

        /// <summary>
        /// A hard computer controlled
        /// </summary>
        HardCpu,

        /// <summary>
        /// Networkcontroller with out owner
        /// </summary>
        Nobody
    }

    /// <summary>
    /// Base interface of players
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Gets category of player
        /// </summary>
        PlayerType Type { get; }
    }
}
