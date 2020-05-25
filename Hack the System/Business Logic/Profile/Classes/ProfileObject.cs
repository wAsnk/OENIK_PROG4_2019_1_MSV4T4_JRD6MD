// <copyright file="ProfileObject.cs" company="PlaceholderCompany">
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
    /// This class represent a profle with their datas
    /// </summary>
    public class ProfileObject : IProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileObject"/> class.
        /// </summary>
        /// <param name="data">Datas of profile</param>
        public ProfileObject(IProfileData data)
        {
            this.ProfileDatas = data;
        }

        /// <inheritdoc/>
        public string Name
        {
            get { return this.ProfileDatas.Name; }
            set { this.ProfileDatas.Name = value; }
        }

        /// <inheritdoc/>
        public int CompletedLevelCount
        {
            get
            {
                return this.CampaignScore.Count;
            }
        }

        /// <inheritdoc/>
        public List<int> CampaignScore
        {
            get { return this.ProfileDatas.CampaignScore; }
            set { this.ProfileDatas.CampaignScore = value; }
        }

        /// <inheritdoc/>
        public int CompletedRandomLevelCount
        {
            get { return this.ProfileDatas.CompletedRandomLevelCount; }
            set { this.ProfileDatas.CompletedRandomLevelCount = value; }
        }

        /// <inheritdoc/>
        public int TotalScore
        {
            get
            {
                return this.CampaignScore.Sum();
            }
        }

        /// <inheritdoc/>
        public int BestScore
        {
            get
            {
                return this.CampaignScore.Count > 0 ? this.CampaignScore.Max() : 0;
            }
        }

        /// <inheritdoc/>
        public bool EnableSavedGame
        {
            get { return this.ProfileDatas.SavedGame; }
            set { this.ProfileDatas.SavedGame = value; }
        }

        /// <inheritdoc/>
        public IProfileData ProfileDatas { get; set; }
    }
}
