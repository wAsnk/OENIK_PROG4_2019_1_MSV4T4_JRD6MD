// <copyright file="Router.cs" company="PlaceholderCompany">
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
    /// This object can handle many Utp cables but it have low lifepoint
    /// </summary>
    [Serializable]
    public class Router : IActiveNetworkController, IInactiveNetworkController
    {
        /// <summary>
        /// Life limit of routers
        /// </summary>
        public static readonly int RouterLifeLimit = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// This constructor make an active Router
        /// </summary>
        /// <param name="owner">This player control this Router</param>
        /// <param name="life">Life points of the Router</param>
        /// <param name="x">X coordinate of Router</param>
        /// <param name="y">Y coordinate of Router</param>
        /// <param name="lifelimit">Life limit in this level</param>
        public Router(IPlayer owner, int life, int x, int y, int lifelimit)
        {
            this.IsEnable = true;
            this.Owner = owner;
            this.Life = life;
            this.X = x;
            this.Y = y;
            this.LifeLimit = lifelimit;
            this.Utps = new List<IUtp>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Router"/> class.
        /// This constructor make an inactive Router
        /// </summary>
        /// <param name="life">Life points of the Router</param>
        /// <param name="x">X coordinate of Server</param>
        /// <param name="y">Y coordinate of Server</param>
        /// <param name="lifelimit">Life limit in this level</param>
        public Router(int life, int x, int y, int lifelimit)
        {
            this.IsEnable = false;
            this.CaptureLimit = life / 10;
            this.CapturePoint = 0;
            this.Life = life;
            this.X = x;
            this.Y = y;
            this.LifeLimit = lifelimit;
            this.Utps = new List<IUtp>();
        }

        /// <inheritdoc/>
        public List<IUtp> Utps { get; set; }

        /// <inheritdoc/>
        public virtual int CableCapacity
        {
            get
            {
                return 4;
            }
        }

        /// <inheritdoc/>
        public int Life { get; set; }

        /// <inheritdoc/>
        public IPlayer Owner { get; set; }

        /// <inheritdoc/>
        public double X { get; set; }

        /// <inheritdoc/>
        public double Y { get; set; }

        /// <inheritdoc/>
        public bool IsEnable { get; set; }

        /// <inheritdoc/>
        public int CapturePoint { get; set; }

        /// <inheritdoc/>
        public int CaptureLimit { get; set; }

        /// <inheritdoc/>
        public int LifeLimit { get; set; }

        /// <inheritdoc/>
        public int UsedCable
        {
            get { return this.Utps.Where(x => x.Creator == this).Count(); }
        }

        /// <inheritdoc/>
        public void Attack(INetworkController target)
        {
            if (this.IsEnable)
            {
                if (this.UsedCable < this.CableCapacity && this.Utps.Where(x => x.Target == target).FirstOrDefault() == null)
                {
                    this.Utps.Add(new Utp(this, target));
                    if (target.Owner == this.Owner)
                    {
                        IUtp utp = (target as IActiveNetworkController).Utps.Where(x => x.Target == this).FirstOrDefault();
                        if (utp != null)
                        {
                            utp.Aback();
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void Captured()
        {
            this.IsEnable = true;
        }

        /// <inheritdoc/>
        public void AddCutedUtp(IUtp cutedUtp)
        {
            this.Utps.Add(cutedUtp);
        }

        /// <inheritdoc/>
        public virtual bool LostLifeFromUtp()
        {
            if (this.Life > 0)
            {
                this.Life--;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <inheritdoc/>
        public virtual void OnCharge(IPlayer owner)
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

        /// <inheritdoc />
        public virtual void Tick()
        {
            List<IUtp> disconect = new List<IUtp>();
            foreach (var item in this.Utps)
            {
                item.Tick();
                if (item.Disconnect)
                {
                    disconect.Add(item);
                }
            }

            foreach (var item in disconect)
            {
               this.Utps.Remove(item);
            }
        }

        /// <inheritdoc/>
        public virtual void GetLifeFromUtp(IPlayer owner)
        {
            switch (this.IsEnable)
            {
                case true:
                    this.EnableGetLifeFromUTP(owner);
                    break;
                case false:
                    this.DisableOnCharge(owner);
                    break;
            }
        }

        /// <summary>
        /// This method call when router is active and get a charge
        /// </summary>
        /// <param name="owner">Owner player of charge</param>
        protected virtual void EnableOnCharge(IPlayer owner)
        {
            if (this.Owner == owner)
            {
                if (this.Life < Math.Min(this.LifeLimit, RouterLifeLimit))
                {
                    ++this.Life;
                }

                foreach (var item in this.Utps)
                {
                    item.OnCharge(owner);
                }
            }
            else
            {
                --this.Life;
                this.LifeCheck(owner);
            }
        }

        /// <summary>
        /// This method call when router life decrease
        /// </summary>
        /// <param name="owner">Owner player of charge</param>
        protected void LifeCheck(IPlayer owner)
        {
            if (this.Life <= 0)
            {
                foreach (var item in this.Utps)
                {
                    item.Aback();
                }

                this.Owner = owner;
                this.Life = 10;
            }
        }

        /// <summary>
        /// This method call when router is inactive and get a charge
        /// </summary>
        /// <param name="owner">Owner player of charge</param>
        protected void DisableOnCharge(IPlayer owner)
        {
            if (this.Owner == owner)
            {
                ++this.CapturePoint;
                if (this.CapturePoint == this.CaptureLimit)
                {
                    this.Captured();
                }
            }
            else
            {
                if (this.CapturePoint == 0)
                {
                    this.Owner = owner;
                    ++this.CapturePoint;
                    if (this.CapturePoint == this.CaptureLimit)
                    {
                        this.Captured();
                    }
                }
                else
                {
                    --this.CapturePoint;
                    if (this.CapturePoint == 0)
                    {
                        this.Owner = null;
                    }
                }
            }
        }

        private void EnableGetLifeFromUTP(IPlayer owner)
        {
            if (this.Owner == owner)
            {
                if (this.Life < Math.Min(this.LifeLimit, RouterLifeLimit))
                {
                    ++this.Life;
                }
            }
            else
            {
                --this.Life;
                this.LifeCheck(owner);
            }
        }
    }
}
