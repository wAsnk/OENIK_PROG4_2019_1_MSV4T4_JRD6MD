// <copyright file="IGameModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Classes;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// This interface describe game parameters
    /// </summary>
    public interface IGameModel
    {
        /// <summary>
        /// Gets gameobjects of actualy game
        /// </summary>
        List<IGameObject> GameObjects { get; }

        /// <summary>
        /// Gets player of actualy game
        /// </summary>
        List<IPlayer> Players { get; }

        /// <summary>
        /// Gets passed game time
        /// </summary>
        int Time { get; }

        /// <summary>
        /// Gets passed game time
        /// </summary>
        int TickCount { get; }

        /// <summary>
        /// Gets life limit in actualy game
        /// </summary>
        int LifeLimit { get; }

        /// <summary>
        /// Gets score of local player
        /// </summary>
        int Score { get; }

        /// <summary>
        /// Gets representation of human player
        /// </summary>
        IHumanPlayer LocalPlayer { get; }

        /// <summary>
        /// Gets messages of actualy game
        /// </summary>
        List<IGameMessage> Messages { get; }

        /// <summary>
        /// Gets height of map
        /// </summary>
        int MapHeight { get; }

        /// <summary>
        /// Gets width of map
        /// </summary>
        int MapWidth { get; }

        /// <summary>
        /// Gets size of tile
        /// </summary>
        int TileSize { get; }

        /// <summary>
        /// Gets the gametype, random or campaign
        /// </summary>
        GameType GameType { get; }

        /// <summary>
        /// Gets the map ID
        /// </summary>
        int MapNumber { get; }
    }
}
