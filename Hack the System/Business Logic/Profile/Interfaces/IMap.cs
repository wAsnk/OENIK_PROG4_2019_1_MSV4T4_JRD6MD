// <copyright file="IMap.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Repository.Interfaces;

    /// <summary>
    /// This interface describe a  map
    /// </summary>
    public interface IMap
    {
        /// <summary>
        /// Gets id of map
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets map datas from repository
        /// </summary>
        IMapDatas MapData { get; }

        /// <summary>
        /// Gets or sets  score if player already play this map
        /// </summary>
        int Score { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets a value indicating whether
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
