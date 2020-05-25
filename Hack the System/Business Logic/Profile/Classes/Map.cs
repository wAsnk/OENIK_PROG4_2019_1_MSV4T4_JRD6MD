// <copyright file="Map.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.Profile.Interfaces;
    using Repository.Interfaces;

    /// <summary>
    /// This class represent a map
    /// </summary>
    public class Map : IMap
    {
        /// <inheritdoc/>
        public int Id { get; set; }

        /// <inheritdoc/>
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public IMapDatas MapData { get; set; }

        /// <inheritdoc/>
        public int Score { get; set; }
    }
}
