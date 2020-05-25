// <copyright file="ProfileData.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Repository.Classes
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Repository.Interfaces;

    /// <summary>
    /// This class store profile datas
    /// </summary>
    public class ProfileData : IProfileData
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public List<int> CampaignScore { get; set; }

        /// <inheritdoc/>
        public int CompletedRandomLevelCount { get; set; }

        /// <inheritdoc/>
        public bool SavedGame { get; set; }
    }
}
