// <copyright file="IProfileLoader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Profile.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;

    /// <summary>
    /// This interface describe activities of profileloader
    /// </summary>
    public interface IProfileLoader
    {
        /// <summary>
        /// Gets actualy sellected profile
        /// </summary>
        IProfile Profile { get; }

        /// <summary>
        /// Gets list of profiles in reposytory
        /// </summary>
        ObservableCollection<IProfile> AllProfile { get; }

        /// <summary>
        /// Gets list of maps in reposytory
        /// </summary>
        ObservableCollection<IMap> AllMap { get; }

        /// <summary>
        /// Select a profile by name
        /// </summary>
        /// <param name="name">Name of player</param>
        void ChangeProfile(string name);

        /// <summary>
        /// Delete a profile by name
        /// </summary>
        /// <param name="name">Name of profile</param>
        void DeleteProfile(string name);

        /// <summary>
        /// Load default profile
        /// </summary>
        void LoadDefaultProfile();

        /// <summary>
        /// Create a new proile
        /// </summary>
        /// <param name="name">Uniq name</param>
        void CreateNewProfie(string name);

        /// <summary>
        /// Save selected profile datas
        /// </summary>
        void UpdateProfile();

        /// <summary>
        /// List of maps in repository
        /// </summary>
        /// <returns>List of maps</returns>
        List<IMap> LoadMaps();

        /// <summary>
        /// Load profile's saved game
        /// </summary>
        /// <returns>Saved game state</returns>
        IGameModel LoadGame();

        /// <summary>
        /// Save a game state
        /// </summary>
        /// <param name="gameModel">Game state</param>
        void SaveGame(IGameModel gameModel);

        /// <summary>
        /// This method will call  when player win a random level
        /// </summary>
        void WinRandomLevel();

        /// <summary>
        /// This method will call  when player win a random level
        /// </summary>
        /// <param name="level">Number of completed level</param>
        /// <param name="score">Acquired score</param>
        void WinCampaign(int level, int score);
    }
}
