// <copyright file="IGameMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This interface describe a game message whitc can display on screen
    /// </summary>
    public interface IGameMessage
    {
        /// <summary>
        /// Gets text of message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets life time of message
        /// </summary>
        int LeftTime { get; }

        /// <summary>
        /// Gets or sets a value indicating whether its time has expired
        /// </summary>
        bool Over { get; set; }

        /// <summary>
        /// This method run on every timer tick event
        /// </summary>
        void Tick();
    }
}
