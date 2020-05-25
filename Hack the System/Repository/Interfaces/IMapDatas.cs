// <copyright file="IMapDatas.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    /// <summary>
    /// This interface describe parameters of a map
    /// </summary>
    public interface IMapDatas
    {
        /// <summary>
        /// Gets power limit on map
        /// </summary>
        int PowerLimit { get; }

        /// <summary>
        /// Gets size of map
        /// </summary>
        Size MapSize { get; }

        /// <summary>
        /// Gets a string whitc describe tiles
        /// </summary>
        string Fields { get; }

        /// <summary>
        /// Gets list of players on map
        /// </summary>
        List<string> Players { get; }

        /// <summary>
        /// Gets list of servers on map
        /// </summary>
        List<string> Servers { get; }

        /// <summary>
        /// Gets list of routers on map
        /// </summary>
        List<string> Routers { get; }

        /// <summary>
        /// Gets list of firewals on map
        /// </summary>
        List<string> FireWalls { get; }
    }
}
