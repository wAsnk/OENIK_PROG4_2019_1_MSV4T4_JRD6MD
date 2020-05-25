// <copyright file="MapData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Repository.Interfaces;

    /// <summary>
    /// This class store map datas
    /// </summary>
    public class MapData : IMapDatas
    {
        /// <inheritdoc/>
        public Size MapSize { get; set; }

        /// <inheritdoc/>
        public string Fields { get; set; }

        /// <inheritdoc/>
        public List<string> Servers { get; set; }

        /// <inheritdoc/>
        public List<string> Routers { get; set; }

        /// <inheritdoc/>
        public List<string> FireWalls { get; set; }

        /// <inheritdoc/>
        public List<string> Players { get; set; }

        /// <inheritdoc/>
        public int PowerLimit { get; set; }
    }
}
