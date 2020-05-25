// <copyright file="IGameObject.cs" company="PlaceholderCompany">
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
    /// Object of ths game
    /// </summary>
    public interface IGameObject
    {
        /// <summary>
        /// Gets or sets x coordinate of gameobject
        /// </summary>
        double X { get; set; }

        /// <summary>
        /// Gets or sets y coordinate of gameobject
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// This method run on every timer tick event
        /// </summary>
        void Tick();
    }
}
