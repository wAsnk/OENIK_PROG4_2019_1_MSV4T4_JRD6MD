// <copyright file="IActiveNetworkController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;

    /// <summary>
    /// Activities of activeetworkcontroller
    /// </summary>
    public interface IActiveNetworkController : INetworkController
    {
        /// <summary>
        /// Method to send Utp to networkcontroller
        /// </summary>
        /// <param name="target">Target networkcontroller</param>
        void Attack(INetworkController target);

        /// <summary>
        /// This method will call when networkcontroller send cable to target
        /// </summary>
        /// <returns>If life will 0 after send one more cabel than return false</returns>
        bool LostLifeFromUtp();

        /// <summary>
        /// When UTP rolls back it gives it's life to it's owner
        /// </summary>
        /// <param name="owner">Owner of the UTP</param>
        void GetLifeFromUtp(IPlayer owner);
    }
}
