// <copyright file="GameTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Business_Logic.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Business_Logic.Game.Classes;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Classes;
    using Business_Logic.GameObjects.Interfaces;
    using Moq;
    using NUnit.Framework;

    /// <summary>
    /// Game test class
    /// </summary>
    [TestFixture]
    public class GameTest
    {
        private GameObject game = new GameObject();

        /// <summary>
        /// Test if generate map adds game object to game.
        /// </summary>
        [Test]
        public void GenerateMap_Add_GameObjects_To_Game()
        {
            this.game.GenerateMap();
            Assert.That(this.game.GameObjects.Count > 0);
        }

        /// <summary>
        /// Test if generate map add Players to game
        /// </summary>
        [Test]
        public void GenerateMap_Add_Players_To_Game()
        {
            this.game.GenerateMap();
            Assert.That(this.game.Players.Count > 0);
        }

        /// <summary>
        /// Test if Wall Is Between Two Server Then Two Server Cannot Connection To Eachother
        /// </summary>
        [Test]
        public void If_Wall_Is_Between_Two_Server_Then_Two_Server_Cannot_Connection_To_Eachother()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IActiveNetworkController active = new ServerObject(humanmock.Object, 100, 25, 25, 100);
            INetworkController target = new ServerObject(10, 125, 25, 100);
            IFireWall fireWall = new FireWall(75, 25);
            this.game.GameObjects.Add(active);
            this.game.GameObjects.Add(target);
            this.game.GameObjects.Add(fireWall);
            this.game.ConnectTwoServer(active, target);
            Assert.That(active.Utps.Count == 0);
        }
    }
}
