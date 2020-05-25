// <copyright file="IProfileRepository.cs" company="PlaceholderCompany">
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
    /// This interface describe activities of profileloader
    /// </summary>
    public interface IProfileRepository
    {
        /// <summary>
        /// Insert a new profile to file
        /// </summary>
        /// <param name="newItem">Datas of new profile</param>
        void Insert(IProfileData newItem);

        /// <summary>
        /// Return all profike with their datas
        /// </summary>
        /// <returns>List of player in file</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
        IQueryable<IProfileData> GetAll();

        /// <summary>
        /// Delete a profile by name
        /// </summary>
        /// <param name="name">Name of profile</param>
        void Delete(string name);

        /// <summary>
        /// Update profile datas
        /// </summary>
        /// <param name="item">Datas of profile</param>
        void Update(IProfileData item);

        /// <summary>
        /// Return default player
        /// </summary>
        /// <returns>Default player</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Reviewed")]
        IProfileData GetDefault();

        /// <summary>
        /// Set a player to default
        /// </summary>
        /// <param name="name">Name of default profile</param>
        void SetDefault(string name);

        /// <summary>
        /// Save a gamestate to file
        /// </summary>
        /// <param name="serializedgamestate">Game state</param>
        /// <param name="name">Name of player</param>
        void SaveGame(MemoryStream serializedgamestate, string name);

        /// <summary>
        /// Load a gamestate from file
        /// </summary>
        /// <param name="name">Name of player</param>
        /// <returns>Saved game state</returns>
        MemoryStream LoadGame(string name);
    }
}
