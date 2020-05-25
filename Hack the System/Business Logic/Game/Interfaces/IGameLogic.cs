// <copyright file="IGameLogic.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Classes;
    using Business_Logic.GameObjects.Interfaces;
    using Business_Logic.Profile.Interfaces;

    /// <summary>
    /// This interface describe game methodes
    /// </summary>
    public interface IGameLogic
    {
        /// <summary>
        /// This event call when game is over
        /// </summary>
        event EventHandler<GameOverEventArgs> GameOver;

        /// <summary>
        /// Send Utp from a networkcontroller to another
        /// </summary>
        /// <param name="source">Parent networkcontroller</param>
        /// <param name="target">Target networcontroller</param>
        void ConnectTwoServer(IActiveNetworkController source, INetworkController target);

        /// <summary>
        /// Call back utp
        /// </summary>
        /// <param name="utp">The utp whitc will call back</param>
        void DisconectUtp(IUtp utp);

        /// <summary>
        /// Cut an utp
        /// </summary>
        /// <param name="utp">The utp whitc will cut</param>
        /// <param name="cable">Point of cut</param>
        void CutUtp(IUtp utp, ICable cable);

        /// <summary>
        /// This method will call by timer
        /// </summary>
        void Tick();

        /// <summary>
        /// Player interaction on map
        /// </summary>
        /// <param name="location">Point of interaction</param>
        void InteractionStart(Point location);

        /// <summary>
        /// Player interaction on map
        /// </summary>
        /// <param name="networkController">Interacted networkcontroller</param>
        void InteractionStart(INetworkController networkController);

        /// <summary>
        /// Player interaction on map
        /// </summary>
        /// <param name="location">Point of interaction</param>
        void InteractionEnd(Point location);

        /// <summary>
        /// Player interaction on map
        /// </summary>
        /// <param name="networkController">Interacted networkcontroller</param>
        void InteractionEnd(INetworkController networkController);

        /// <summary>
        /// Genereate new random map
        /// </summary>
        void GenerateMap();

        /// <summary>
        /// Load an campaing map
        /// </summary>
        /// <param name="map">Map datas</param>
        void LoadMap(IMap map);

        /// <summary>
        /// Save game state
        /// </summary>
        /// <returns>Game state</returns>
        IGameModel SaveGame();

        /// <summary>
        /// Load a game state
        /// </summary>
        /// <param name="gameModel">Game state</param>
        void LoadGame(IGameModel gameModel);

        /// <summary>
        /// Sorts the game object in case direction changed
        /// </summary>
        /// <param name="direction">Waits for a direction enum.</param>
        void SortGameObjects(Direction direction);
    }
}
