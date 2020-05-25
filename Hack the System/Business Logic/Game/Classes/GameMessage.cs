// <copyright file="GameMessage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;

    /// <summary>
    /// Game can send message to display
    /// </summary>
    [Serializable]
    public class GameMessage : IGameMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameMessage"/> class.
        /// </summary>
        /// <param name="message">Text of message</param>
        public GameMessage(string message)
        {
            this.Message = message;
            this.LeftTime = 200;

            this.Over = false;
        }

        /// <inheritdoc/>
        public string Message { get; set; }

        /// <inheritdoc/>
        public int LeftTime { get; set; }

        /// <inheritdoc/>
        public bool Over { get; set; }

        /// <inheritdoc/>
        public void Tick()
        {
            if (this.LeftTime > 0)
            {
                --this.LeftTime;
            }
            else
            {
                this.Over = true;
            }
        }
    }
}
