// <copyright file="ICPUPlayer.cs" company="PlaceholderCompany">
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
    /// Represetation of CPU player
    /// </summary>
    public interface ICpuPlayer : IPlayer
    {
        /// <summary>
        /// Ai make decision depends on this method
        /// </summary>
        /// <param name="model">Model of game</param>
        /// <param name="logic">Logic of game</param>
        void MakeDecision(IGameModel model, IGameLogic logic);
    }
}
