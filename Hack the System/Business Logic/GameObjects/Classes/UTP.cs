// <copyright file="UTP.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.GameObjects.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// This class handle cabels
    /// </summary>
    [Serializable]
    public class Utp : IUtp
    {
        private const double CABLEDISTANCE = 30;

        private const int CABLESIZE = 20;

        private const double CABLENORMALSPEED = 3;

        private const double CABLEBACKSPEED = 6;

        private UtpModes mode;

        /// <summary>
        /// Initializes a new instance of the <see cref="Utp"/> class.
        /// </summary>
        /// <param name="parent">Parent newtwork controller</param>
        /// <param name="target">Target networkcontroller</param>
        public Utp(IActiveNetworkController parent, INetworkController target)
        {
            this.IsActive = false;
            this.Parent = parent;
            this.Target = target;
            this.Cables = new List<ICable>
            {
                new Cable(target) { X = this.Parent.X, Y = this.Parent.Y }
            };
            this.Creator = this.Parent;

            if (this.EnemyUTP() != null)
            {
                this.Mode = UtpModes.MoveToBattle;
                this.EnemyUTP().Mode = UtpModes.MoveToBattle;
            }
            else
            {
                this.Mode = UtpModes.Attack;
            }

            this.Disconnect = false;

            this.HeadArrive += this.UTP_HeadArrive;
        }

        private event EventHandler HeadArrive;

        /// <inheritdoc/>
        public List<ICable> Cables { get; set; }

        /// <inheritdoc/>
        public bool IsActive { get; set; }

        /// <inheritdoc/>
        public ICable First
        {
            get { return this.Cables?.First(); }
        }

        /// <inheritdoc/>
        public ICable Last
        {
            get { return this.Cables?.Last(); }
        }

        /// <inheritdoc/>
        public IActiveNetworkController Parent { get; set; }

        /// <inheritdoc/>
        public INetworkController Target { get; set; }

        /// <inheritdoc/>
        public UtpModes Mode
        {
            get
            {
                return this.mode;
            }

            set
            {
                this.mode = value;
                this.CheckMode();
            }
        }

        /// <inheritdoc/>
        public bool Disconnect { get; set; }

        /// <inheritdoc/>
        public IActiveNetworkController Creator { get; set; }

        private double PrevousDistance { get; set; }

        private double HeadSpeed { get; set; }

        private Point GoalPoint { get; set; }

        private Point CenterPoint
        {
            get
            {
                Vector vector = new Vector(this.Target.X - this.Parent.X, this.Target.Y - this.Parent.Y) / 2;
                Point point = new Point(vector.X + this.Parent.X, vector.Y + this.Parent.Y);
                return point;
            }
        }

        /// <inheritdoc/>
        public void Tick()
        {
            if (this.Mode == UtpModes.MoveToBattle)
            {
                this.CheckBattlePosition();
            }

            if (this.IsMove())
            {
                this.HeadMove();
            }

            foreach (var item in this.Cables)
            {
                if (item != this.First && this.IsMove())
                {
                    this.SetCablePosition(item);
                }

                item.Tick();
            }

            if (this.IsMove())
            {
                this.LastCheck();
            }
        }

        /// <inheritdoc />
        public void OnCharge(IPlayer owner)
        {
            if (this.IsActive)
            {
                this.Last.OnCharge(owner);
            }
        }

        /// <inheritdoc/>
        public void Aback()
        {
            this.Mode = UtpModes.Back;
            if (this.EnemyUTP() != null)
            {
                this.EnemyUTP().Mode = UtpModes.Attack;
            }
        }

        /// <inheritdoc/>
        public void Cut(ICable cable)
        {
            if (this.Mode == UtpModes.ConnectedToServer)
            {
                List<ICable> cables = new List<ICable>();
                if (cable.NextGameObject is ICable)
                {
                    cables = this.RemoveCable(cable.NextGameObject as ICable);
                }

                if (cables.Count > 0)
                {
                    cables[0].NextGameObject = this.Parent;
                    this.Target.AddCutedUtp(this.CutedUTP(cables));
                    this.First.NextGameObject = this.Target;
                }
            }

            this.Aback();
        }

        private bool IsMove()
        {
            return this.Mode != UtpModes.ConnectedToServer && this.Mode != UtpModes.Battle;
        }

        private void SetCablePosition(ICable item)
        {
            Vector vector = new Vector(this.Parent.X - this.Target.X, this.Parent.Y - this.Target.Y);
            vector.Normalize();
            vector *= 20;
            item.X = (item.NextGameObject as ICable).X + vector.X;
            item.Y = (item.NextGameObject as ICable).Y + vector.Y;
        }

        private void CheckMode()
        {
            switch (this.Mode)
            {
                case UtpModes.Attack:
                    this.IsActive = false;
                    this.HeadSpeed = CABLENORMALSPEED;
                    this.GoalPoint = new Point(this.Target.X, this.Target.Y);
                    break;
                case UtpModes.Back:
                    this.IsActive = false;
                    this.HeadSpeed = CABLEBACKSPEED;
                    this.GoalPoint = new Point(this.Parent.X, this.Parent.Y);
                    break;
                case UtpModes.MoveToBattle:
                    this.IsActive = false;
                    this.HeadSpeed = CABLEBACKSPEED;
                    break;
                case UtpModes.Battle:
                    this.IsActive = true;
                    break;
                case UtpModes.ConnectedToServer:
                    this.IsActive = true;
                    break;
                default:
                    break;
            }
        }

        private void CheckBattlePosition()
        {
            Point enemy = new Point(this.EnemyUTP().First.X, this.EnemyUTP().First.Y);
            Vector v = new Vector(enemy.X - this.First.X, enemy.Y - this.First.Y);
            if (v.Length > 20)
            {
                this.GoalPoint = enemy;
            }
            else
            {
                this.GoalPoint = this.CenterPoint;
            }
        }

        private void HeadMove()
        {
            Vector vector = new Vector(this.GoalPoint.X - this.First.X, this.GoalPoint.Y - this.First.Y);
            if (vector.Length > 5)
            {
                vector.Normalize();
                vector = vector * this.HeadSpeed;
                this.First.X += vector.X;
                this.First.Y += vector.Y;
            }
            else
            {
                this.First.X = this.GoalPoint.X;
                this.First.Y = this.GoalPoint.Y;
                this.HeadArrive?.Invoke(this, null);
            }
        }

        private void LastCheck()
        {
            double newdistance = new Vector(this.Last.X - this.Parent.X, this.Last.Y - this.Parent.Y).Length;
            if (newdistance > this.PrevousDistance)
            {
                this.ForwardLastCheck(newdistance);
            }
            else if (newdistance < this.PrevousDistance)
            {
                this.BackLastCheck(newdistance);
            }
        }

        private void ForwardLastCheck(double newdistance)
        {
            if (newdistance > CABLEDISTANCE)
            {
                if (this.Parent.LostLifeFromUtp())
                {
                    Cable newCable = new Cable(this.Last) { X = this.Parent.X, Y = this.Parent.Y };
                    this.Last.Previous = newCable;
                    this.Cables.Add(newCable);
                    this.PrevousDistance = 0;
                }
                else
                {
                    this.Aback();
                    this.PrevousDistance = newdistance;
                }
            }
            else
            {
                this.PrevousDistance = newdistance;
            }
        }

        private void BackLastCheck(double newdistance)
        {
            if (newdistance < CABLEDISTANCE)
            {
                if (this.Last == this.First)
                {
                    this.Disconnect = true;
                    this.Parent.GetLifeFromUtp(this.Creator.Owner);
                }
                else
                {
                    this.Cables.Remove(this.Last);
                    this.Parent.GetLifeFromUtp(this.Creator.Owner);
                    this.PrevousDistance = new Vector(this.Last.X - this.Parent.X, this.Last.Y - this.Parent.Y).Length;
                }
            }
            else
            {
                this.PrevousDistance = newdistance;
            }
        }

        private IUtp CutedUTP(List<ICable> cables)
        {
            IUtp uTP = new Utp(this.Target as IActiveNetworkController, this.Parent)
            {
                Cables = cables,
                Creator = this.Parent,
                PrevousDistance = double.MaxValue
            };
            uTP.Aback();
            return uTP;
        }

        private List<ICable> RemoveCable(ICable next)
        {
            List<ICable> ret = new List<ICable>();
            if (next is ICable)
            {
                ret = this.RemoveCable(next.NextGameObject as ICable);
            }

            this.Cables.Remove(next as ICable);
            if (next != null)
            {
                var a = next.NextGameObject;
                next.NextGameObject = next.Previous;
                next.Previous = a;
                ret.Insert(0, next as ICable);
            }

            return ret;
        }

        private IUtp EnemyUTP()
        {
            return this.Target.Utps.Where(x => x.Target == this.Parent && x.Mode != UtpModes.Back).FirstOrDefault();
        }

        private void UTP_HeadArrive(object sender, EventArgs e)
        {
            switch (this.Mode)
            {
                case UtpModes.Attack:
                    this.Mode = UtpModes.ConnectedToServer;
                    break;
                case UtpModes.Back:
                    this.Disconnect = true;
                    break;
                case UtpModes.MoveToBattle:
                    this.Mode = UtpModes.Battle;
                    break;
                default:
                    break;
            }
        }
    }
}
