// <copyright file="GameOverEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Classes
{
    using System;

    /// <summary>
    /// This eventargs store result of game
    /// </summary>
    public class GameOverEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverEventArgs"/> class.
        /// </summary>
        /// <param name="playerwin">Result of game</param>
        public GameOverEventArgs(bool playerwin)
        {
            this.PlayerWin = playerwin;
            this.GameType = GameType.Random;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverEventArgs"/> class.
        /// </summary>
        /// <param name="playerwin">When true the local player won.</param>
        /// <param name="mapnumber">Map id</param>
        public GameOverEventArgs(bool playerwin, int mapnumber)
        {
            this.PlayerWin = playerwin;
            this.GameType = GameType.Campaign;
            this.MapNumber = mapnumber;
        }

        /// <summary>
        /// Gets a value indicating whether local player win
        /// </summary>
        public bool PlayerWin { get; }

        /// <summary>
        /// Gets the game type which can be campaign or random
        /// </summary>
        public GameType GameType { get; }

        /// <summary>
        /// Gets map id number
        /// </summary>
        public int MapNumber { get; }
    }
}