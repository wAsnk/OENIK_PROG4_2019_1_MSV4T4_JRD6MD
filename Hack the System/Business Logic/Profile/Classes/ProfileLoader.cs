// <copyright file="ProfileLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using Business_Logic.Game.Classes;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.Profile.Exceptions;
    using Business_Logic.Profile.Interfaces;
    using Repository.Classes;
    using Repository.Interfaces;

    /// <summary>
    /// This class handle profiles and maps
    /// </summary>
    public class ProfileLoader : IProfileLoader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileLoader"/> class.
        /// </summary>
        public ProfileLoader()
        {
            this.ProfileRepository = new ProfileRepository();
            this.MapDatas = new MapRepository().GetAll().ToList();
            this.AllProfile = new ObservableCollection<IProfile>();
            this.AllMap = new ObservableCollection<IMap>();
            foreach (var item in this.AllProfiles())
            {
                this.AllProfile.Add(item);
            }
        }

        /// <inheritdoc/>
        public IProfile Profile { get; set; }

        /// <inheritdoc/>
        public ObservableCollection<IProfile> AllProfile { get; set; }

        /// <inheritdoc/>
        public ObservableCollection<IMap> AllMap { get; set; }

        private IProfileRepository ProfileRepository { get; set; }

        private List<IMapDatas> MapDatas { get; set; }

        /// <inheritdoc/>
        public void ChangeProfile(string name)
        {
            this.Profile = this.AllProfile.Where(x => x.Name == name).Single();
            this.MapScoresByProfile();
            this.ProfileRepository.SetDefault(this.Profile.Name);
        }

        /// <inheritdoc/>
        public void CreateNewProfie(string name)
        {
            IProfileData profileData = new ProfileData()
            {
                Name = name,
                CampaignScore = new List<int>(),
                CompletedRandomLevelCount = 0
            };

            try
            {
                this.ProfileRepository.Insert(profileData);
                this.AllProfile.Add(new ProfileObject(profileData) as IProfile);
            }
            catch (Exception)
            {
                throw new ProfileAlreadyExistException();
            }
        }

        /// <inheritdoc/>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        public void SaveGame(IGameModel gameModel)
        {
            this.Profile.EnableSavedGame = true;

            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, gameModel);
            this.ProfileRepository.SaveGame(stream, this.Profile.Name);
            stream.Close();
        }

        /// <inheritdoc/>
        public IGameModel LoadGame()
        {
            IFormatter formatter = new BinaryFormatter();
            GameObject objnew = (GameObject)formatter.Deserialize(this.ProfileRepository.LoadGame(this.Profile.Name));
            return objnew as IGameModel;
        }

        /// <inheritdoc/>
        public void DeleteProfile(string name)
        {
            IProfile deleteProfile = this.AllProfile.Where(x => x.Name == name).Single();
            if (deleteProfile != this.Profile)
            {
                this.AllProfile.Remove(deleteProfile);
                this.ProfileRepository.Delete(name);
            }
            else
            {
                throw new DeleteCurrentProfileException();
            }
        }

        /// <inheritdoc/>
        public void LoadDefaultProfile()
        {
            IProfileData profileData = this.ProfileRepository.GetDefault();
            if (profileData == null)
            {
                throw new DefaultPlayerNotFoundException();
            }
            else
            {
                this.Profile = new ProfileObject(profileData) as IProfile;
                this.MapScoresByProfile();
            }
        }

        /// <inheritdoc/>
        public void UpdateProfile()
        {
           /* IProfileData profileData = new ProfileData()
            {
                Name = this.Profile.Name,
                CampaignScore = this.Profile.CampaignScore,
                CompletedRandomLevelCount = this.Profile.CompletedRandomLevelCount
            };*/

            foreach (var item in this.AllProfile)
            {
                if (item.Name == this.Profile.Name)
                {
                    item.ProfileDatas = this.Profile.ProfileDatas;
                }
            }

            this.ProfileRepository.Update(this.Profile.ProfileDatas);
        }

        /// <inheritdoc/>
        public List<IMap> LoadMaps()
        {
            List<IMap> ret = new List<IMap>();
            int i = 0;
            foreach (var item in this.MapDatas)
            {
                ret.Add(new Map()
                {
                    IsEnabled = this.Profile.CampaignScore.Count >= i,
                    Id = i + 1,
                    MapData = item,
                    Score = this.Profile.CampaignScore.Count > i ? this.Profile.CampaignScore[i] : 0
                });
                ++i;
            }

            return ret;
        }

        /// <inheritdoc/>
        public void WinRandomLevel()
        {
            ++this.Profile.CompletedRandomLevelCount;
            this.UpdateProfile();

            // this.ProfileRepository.Update(this.Profile.ProfileDatas);
        }

        /// <inheritdoc/>
        public void WinCampaign(int level, int score)
        {
            if (level < this.Profile.CompletedLevelCount)
            {
                if (this.Profile.CampaignScore[level] < score)
                {
                    this.Profile.CampaignScore[level] = score;
                    this.AllMap[level].Score = score;
                }
            }
            else
            {
                this.Profile.CampaignScore.Add(score);
                this.AllMap[level].Score = score;
                if (level + 1 < this.AllMap.Count)
                {
                    this.AllMap[level + 1].IsEnabled = true;
                }
            }

            this.UpdateProfile();
        }

        private void MapScoresByProfile()
        {
            this.AllMap.Clear();
            foreach (var item in this.LoadMaps())
            {
                this.AllMap.Add(item);
            }
        }

        private List<IProfile> AllProfiles()
        {
            return this.ProfileRepository.GetAll()
                .Select(x => new ProfileObject(x) as IProfile).ToList();
        }
    }
}
