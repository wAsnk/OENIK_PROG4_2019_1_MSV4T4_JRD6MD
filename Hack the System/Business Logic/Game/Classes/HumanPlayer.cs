// <copyright file="HumanPlayer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// Representation of human player
    /// </summary>
    [Serializable]
    public class HumanPlayer : IHumanPlayer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HumanPlayer"/> class.
        /// </summary>
        public HumanPlayer()
        {
            this.SelectedController = null;
            this.IsSelectPoint = false;
            this.Type = PlayerType.LocalHumanPlayer;
        }

        /// <inheritdoc/>
        public IActiveNetworkController SelectedController { get; set; }

        /// <inheritdoc/>
        public double SelectedX { get; set; }

        /// <inheritdoc/>
        public double SelectedY { get; set; }

        /// <inheritdoc/>
        public bool IsSelectPoint { get; set; }

        /// <inheritdoc/>
        public PlayerType Type { get; set; }

        /// <inheritdoc/>
        public void DiselectPoint()
        {
            this.IsSelectPoint = false;
        }

        /// <inheritdoc/>
        public void SelectController(IActiveNetworkController controller)
        {
            this.SelectedController = controller;
        }

        /// <inheritdoc/>
        public void SelectPoint(Point location)
        {
            this.SelectedX = location.X;
            this.SelectedY = location.Y;
            this.IsSelectPoint = true;
        }
    }
}
