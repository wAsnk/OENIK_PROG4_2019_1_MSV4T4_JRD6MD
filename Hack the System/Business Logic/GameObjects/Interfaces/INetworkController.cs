// <copyright file="INetworkController.cs" company="PlaceholderCompany">
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
    /// Parameters of networcontroller
    /// </summary>
    public interface INetworkController : IGameObject, IChargeble
    {
        /// <summary>
        /// Gets life limit on level
        /// </summary>
        int LifeLimit { get; }

        /// <summary>
        /// Gets a value indicating whether networcontroller is captured or not
        /// </summary>
        bool IsEnable { get; }

        /// <summary>
        /// Gets life point of this networcontroller
        /// </summary>
        int Life { get; }

        /// <summary>
        /// Gets owner player
        /// </summary>
        IPlayer Owner { get; }

        /// <summary>
        /// Gets pluged Utps
        /// </summary>
        List<IUtp> Utps { get; }

        /// <summary>
        /// Gets limit of handled utp cabels
        /// </summary>
        int CableCapacity { get; }

        /// <summary>
        /// Gets handled utp count
        /// </summary>
        int UsedCable { get; }

        /// <summary>
        /// This metode will call when an other networkcontroller cut utp cable
        /// </summary>
        /// <param name="cutedUtp">Cuted utp pice</param>
        void AddCutedUtp(IUtp cutedUtp);
    }
}
