// <copyright file="EasyCPU.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Game.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;

    /// <summary>
    /// This class is an AI implementation
    /// </summary>
    [Serializable]
    public class EasyCpu : ICpuPlayer
    {
        private static Random random = new Random();

        private readonly int attackLimit = 5;
        private readonly int abackLimit = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="EasyCpu"/> class.
        /// </summary>
        public EasyCpu()
        {
            this.Type = PlayerType.EasyCpu;
        }

        /// <inheritdoc/>
        public PlayerType Type { get; set; }

        /// <inheritdoc/>
        public void MakeDecision(IGameModel model, IGameLogic logic)
        {
            List<IActiveNetworkController> owned = this.GetOwnedNetworkController(model);
            List<INetworkController> notowned = this.GetNotOwnedNetworkController(model);
            foreach (var item in owned)
            {
                foreach (var utp in item.Utps)
                {
                    if (utp.IsActive && utp.Target.Owner == this && utp.Target.IsEnable && utp.Target is IServerNetworkController && this.RandomDecision(this.abackLimit))
                    {
                        utp.Aback();
                    }
                }

                if (this.RandomDecision(this.attackLimit) && item.Utps.Count < item.CableCapacity && notowned.Count != 0)
                {
                    logic.ConnectTwoServer(item, notowned[random.Next(0, notowned.Count)]);
                }
            }
        }

        private bool RandomDecision(double limit)
        {
            return random.Next(0, 1000) < limit;
        }

        private List<INetworkController> GetNotOwnedNetworkController(IGameModel model)
        {
            return model.GameObjects
                  .Where(x => x is INetworkController)
                  .Select(x => x as INetworkController)
                  .Where(x => x.Owner != this).ToList();
        }

        private List<IActiveNetworkController> GetOwnedNetworkController(IGameModel model)
        {
            return model.GameObjects
                 .Where(x => x is IActiveNetworkController)
                 .Select(x => x as IActiveNetworkController)
                 .Where(x => x.Owner == this).ToList();
        }
    }
}
