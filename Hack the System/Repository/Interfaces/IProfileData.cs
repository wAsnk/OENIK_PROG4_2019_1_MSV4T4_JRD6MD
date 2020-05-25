// <copyright file="IProfileData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    ///  This interface describe parameters of a profile
    /// </summary>
    public interface IProfileData
    {
        /// <summary>
        /// Gets or sets name of profile
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets list of scores
        /// </summary>
        List<int> CampaignScore { get; set; }

        /// <summary>
        /// Gets or sets count of completted random levels
        /// </summary>
        int CompletedRandomLevelCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether player have saved game
        /// </summary>
        bool SavedGame { get; set; }
    }
}
