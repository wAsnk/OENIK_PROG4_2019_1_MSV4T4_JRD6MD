// <copyright file="IHumanPlayer.cs" company="PlaceholderCompany">
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
    /// This interface describe humanplayer attributes in game
    /// </summary>
    public interface IHumanPlayer : IPlayer
    {
        /// <summary>
        /// Gets sellected newtworkcontroller
        /// </summary>
        IActiveNetworkController SelectedController { get; }

        /// <summary>
        /// Gets a value indicating whether player select a point on map
        /// </summary>
        bool IsSelectPoint { get; }

        /// <summary>
        /// Gets selected point X cordinate
        /// </summary>
        double SelectedX { get; }

        /// <summary>
        /// Gets selected point Y cordinate
        /// </summary>
        double SelectedY { get; }

        /// <summary>
        /// Select an owned networkcontroller
        /// </summary>
        /// <param name="controller">Selected networkcontroller</param>
        void SelectController(IActiveNetworkController controller);

        /// <summary>
        /// Select a point on map
        /// </summary>
        /// <param name="location">Selected point</param>
        void SelectPoint(Point location);

        /// <summary>
        /// Set selected point null
        /// </summary>
        void DiselectPoint();
    }
}
