// <copyright file="IProfile.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Repository.Interfaces;

    /// <summary>
    /// This interface describe a profle with their datas
    /// </summary>
    public interface IProfile
    {
        /// <summary>
        /// Gets  name of profile
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets count of completed levels
        /// </summary>
        int CompletedLevelCount { get; }

        /// <summary>
        /// Gets  scores on completed levels
        /// </summary>
        List<int> CampaignScore { get; }

        /// <summary>
        /// Gets or sets count of completed random levels
        /// </summary>
        int CompletedRandomLevelCount { get; set; }

        /// <summary>
        /// Gets sum of points
        /// </summary>
        int TotalScore { get; }

        /// <summary>
        /// Gets max of points
        /// </summary>
        int BestScore { get; }

        /// <summary>
        /// Gets or sets a value indicating whether player have saved game
        /// </summary>
        bool EnableSavedGame { get; set; }

        /// <summary>
        /// Gets or sets datas of profile
        /// </summary>
        IProfileData ProfileDatas { get; set; }
    }
}
