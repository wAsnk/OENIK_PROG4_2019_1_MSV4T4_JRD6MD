// <copyright file="IChargeble.cs" company="PlaceholderCompany">
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
    /// This interface order the object to recive charges
    /// </summary>
    public interface IChargeble
    {
        /// <summary>
        /// Whit this methd can object receive charge from other object
        /// </summary>
        /// <param name="owner">Sender of this charge</param>
        void OnCharge(IPlayer owner);
    }
}
