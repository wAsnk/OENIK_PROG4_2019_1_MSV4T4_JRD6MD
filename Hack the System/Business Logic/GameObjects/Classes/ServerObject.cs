// <copyright file="ServerObject.cs" company="PlaceholderCompany">
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
    /// This object can creat charges
    /// </summary>
    [Serializable]
    public class ServerObject : Router, IServerNetworkController
    {
        static ServerObject()
        {
            Levels = new Dictionary<int, IServerAttributes>
            {
                { 1, new ServerAttribute(20, 50, 1, 15, 0) },
                { 2, new ServerAttribute(80, 50, 2, 40, 10) },
                { 3, new ServerAttribute(70, 50, 2, 80, 35) },
                { 4, new ServerAttribute(80, 50, 2, 120, 70) },
                { 5, new ServerAttribute(100, 30, 3, 160, 110) },
                { 6, new ServerAttribute(200, 30, 3, 200, 150) }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerObject"/> class.
        /// This constructor make an active Server
        /// </summary>
        /// <param name="owner">This player control this Server</param>
        /// <param name="life">Life points of the Server</param>
        /// <param name="x">X coordinate of Server</param>
        /// <param name="y">Y coordinate of Server</param>
        /// /// <param name="lifelimit">Life limit in this level</param>
        public ServerObject(IPlayer owner, int life, int x, int y, int lifelimit)
        : base(owner, life, x, y, lifelimit)
        {
            this.Level = this.LevelFromLife(life);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerObject"/> class.
        /// This constructor make an inactive Server
        /// </summary>
        /// <param name="life">Life points of the Server</param>
        /// <param name="x">X coordinate of Server</param>
        /// <param name="y">Y coordinate of Server</param>
        /// /// <param name="lifelimit">Life limit in this level</param>
        public ServerObject(int life, int x, int y, int lifelimit)
            : base(life, x, y, lifelimit)
        {
            this.Level = this.LevelFromLife(life);
        }

        /// <inheritdoc/>
        public int Level { get; set; }

        /// <inheritdoc/>
        public int NewLifePoint { get; set; }

        /// <inheritdoc/>
        public int NewChargePoint { get; set; }

        /// <inheritdoc/>
        public int NewLife
        {
            get { return Levels[this.Level].NewLife; }
        }

        /// <inheritdoc/>
        public int NewCharge
        {
            get { return Levels[this.Level].NewCharge; }
        }

        /// <inheritdoc/>
        public int LevelUp
        {
            get { return Levels[this.Level].LevelUp; }
        }

        /// <inheritdoc/>
        public int LevelDown
        {
            get { return Levels[this.Level].LevelDown; }
        }

        /// <inheritdoc/>
        public override int CableCapacity
        {
            get { return Levels[this.Level].CableCapacity; }
        }

        private static Dictionary<int, IServerAttributes> Levels { get; set; }

        /// <inheritdoc/>
        public override void Tick()
        {
            if (this.IsEnable)
            {
                this.NewLifePointRise();
                this.NewChargePointRise();
            }

            base.Tick();
        }

        /// <inheritdoc/>
        public override void OnCharge(IPlayer owner)
        {
            switch (this.IsEnable)
            {
                case true:
                    this.EnableOnCharge(owner);
                    break;
                case false:
                    this.DisableOnCharge(owner);
                    break;
            }
        }

        /// <inheritdoc/>
        public override bool LostLifeFromUtp()
        {
            if (this.Life > 0)
            {
                this.Life--;
                this.CheckLevelDown();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public override void GetLifeFromUtp(IPlayer owner)
        {
            this.OnCharge(owner);
        }

        /// <summary>
        /// This method call when server is active and get a charge
        /// </summary>
        /// <param name="owner">Owner player of charge</param>
        protected override void EnableOnCharge(IPlayer owner)
        {
            if (this.Owner == owner)
            {
                if (this.Life < this.LifeLimit)
                {
                    ++this.Life;
                    this.CheckLevelUp();
                }

                if (this.Life == this.LifeLimit)
                {
                    foreach (var item in this.Utps)
                    {
                        item.OnCharge(owner);
                    }
                }
            }
            else
            {
                --this.Life;
                this.CheckLevelDown();
                this.LifeCheck(owner);
            }
        }

        private void CheckLevelDown()
        {
            if (this.Life == this.LevelDown && this.Level > 1)
            {
                this.Level--;
                if (this.NewChargePoint >= this.NewCharge)
                {
                    this.NewChargePoint = 0;
                }

                if (this.NewLifePoint >= this.NewLife)
                {
                    this.NewLifePoint = 0;
                }
            }
        }

        private int LevelFromLife(int life)
        {
            foreach (var item in Levels)
            {
                if (life < item.Value.LevelUp)
                {
                    return item.Key;
                }
            }

            throw new ArgumentException("ArgumentException");
        }

        private void NewChargePointRise()
        {
            this.NewChargePoint++;
            if (this.NewChargePoint == this.NewCharge)
            {
                this.NewChargePoint = 0;
                foreach (var item in this.Utps)
                {
                    item.OnCharge(this.Owner);
                }
            }
        }

        private void NewLifePointRise()
        {
            this.NewLifePoint++;
            if (this.NewLifePoint == this.NewLife && this.Life < this.LifeLimit)
            {
                this.NewLifePoint = 0;
                ++this.Life;
                this.CheckLevelUp();
            }
        }

        private void CheckLevelUp()
        {
            if (this.Life == this.LevelUp && this.Level < Levels.Count)
            {
                this.Level++;

                if (this.NewChargePoint >= this.NewCharge)
                {
                    this.NewChargePoint = 0;
                }

                if (this.NewLifePoint >= this.NewLife)
                {
                    this.NewLifePoint = 0;
                }
            }
        }
    }
}
