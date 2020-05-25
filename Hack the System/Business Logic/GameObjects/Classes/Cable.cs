// <copyright file="Cable.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// This class represent a pice of cable which can carry charge
    /// </summary>
    [Serializable]
    public class Cable : ICable
    {
        /// <summary>
        /// How many ticks should the cable wait until it transfers charges
        /// </summary>
        public static readonly int Delaystatic = 2;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cable"/> class.
        /// </summary>
        /// <param name="next">Cable or networkcontroller</param>
        public Cable(IChargeble next)
        {
            this.NextGameObject = next;
            this.Previous = null;
            this.DelayPoint = 0;
            this.ChargeOwner = null;
        }

        /// <inheritdoc/>
        public IChargeble NextGameObject { get; set; }

        /// <inheritdoc/>
        public IChargeble Previous { get; set; }

        /// <inheritdoc/>
        public int DelayPoint { get; set; }

        /// <inheritdoc/>
        public int Delay
        {
            get { return Delaystatic; }
        }

        /// <inheritdoc/>
        public double X { get; set; }

        /// <inheritdoc/>
        public double Y { get; set; }

        /// <inheritdoc/>
        public IPlayer ChargeOwner { get; set; }

        /// <inheritdoc/>
        public void OnCharge(IPlayer owner)
        {
            this.ChargeOwner = owner;
        }

        /// <inheritdoc/>
        public void Tick()
        {
            this.DelayePointRise();
        }

        private void DelayePointRise()
        {
            if (this.ChargeOwner != null)
            {
                ++this.DelayPoint;
                if (this.DelayPoint == this.Delay)
                {
                    this.NextGameObject.OnCharge(this.ChargeOwner);
                    this.ChargeOwner = null;
                    this.DelayPoint = 0;
                }
            }
        }
    }
}
