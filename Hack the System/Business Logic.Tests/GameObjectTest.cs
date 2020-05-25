// <copyright file="GameObjectTest.cs" company="PlaceholderCompany">
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
    /// Game object test class
    /// </summary>
    [TestFixture]
    public class GameObjectTest
    {
        /// <summary>
        /// Test if InactiveServer CapturePoint Is Zero And Get Charge Then CapturePoint Will Rise
        /// </summary>
        [Test]
        public void InactiveServer_CapturePoint_Is_Zero_And_Get_Charge_Then_CapturePoint_Will_Rise()
        {
            IInactiveNetworkController inactiveServer = new ServerObject(10, 0, 0, 40);
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            var beforecaptuepoint = inactiveServer.CapturePoint;
            inactiveServer.OnCharge(humanmock.Object);

            Assert.That(inactiveServer.CapturePoint == beforecaptuepoint + 1);
            Assert.That(inactiveServer.Owner == humanmock.Object);
        }

        /// <summary>
        /// Test if InactiveRouter With 30 Life Get 3 Charge Then It Will Be Active
        /// </summary>
        [Test]
        public void InactiveRouter_With_30_Life_Get_3_Charge_Then_It_Will_Be_Active()
        {
            INetworkController inactiveroute = new Router(30, 0, 0, 40);
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();

            for (int i = 0; i < 3; i++)
            {
                inactiveroute.OnCharge(humanmock.Object);
            }

            Assert.That(inactiveroute.IsEnable);
            Assert.That(inactiveroute.Owner == humanmock.Object);
        }

        /// <summary>
        /// Test if Level1 Server After 20 Tick Get Life
        /// </summary>
        [Test]
        public void Level1_Server_After_20_Tick_Get_Life()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IServerNetworkController server = new ServerObject(humanmock.Object, 10, 0, 0, 40);
            for (int i = 0; i < 20; i++)
            {
                server.Tick();
            }

            Assert.That(server.Life == 11);
        }

        /// <summary>
        /// Test if Server Attack Another Networkcontroller Then Server Get New Utp
        /// </summary>
        [Test]
        public void If_Server_Attack_Another_Networkcontroller_Then_Server_Get_New_Utp()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IServerNetworkController server = new ServerObject(humanmock.Object, 10, 0, 0, 40);
            INetworkController target = new ServerObject(10, 125, 25, 100);
            server.Attack(target);
            Assert.That(server.Utps.Count > 0);
        }

        /// <summary>
        /// Test If Server Cant Attack One Networkcontroller Twice
        /// </summary>
        [Test]
        public void If_Server_Cant_Attack_One_Networkcontroller_Twice()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IServerNetworkController server = new ServerObject(humanmock.Object, 40, 0, 0, 40);
            INetworkController target = new ServerObject(10, 125, 25, 100);
            server.Attack(target);
            server.Attack(target);
            Assert.That(server.Utps.Count == 1);
        }

        /// <summary>
        /// Test if Two Server With Similar Owner Attack Eachother Then One Of Them Will Call Back Their UTP
        /// </summary>
        [Test]
        public void If_Two_Server_With_Similar_Owner_Attack_Eachother_Then_One_Of_Them_Will_Call_Back_Their_Utp()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IServerNetworkController server1 = new ServerObject(humanmock.Object, 40, 0, 0, 40);
            IServerNetworkController server2 = new ServerObject(humanmock.Object, 40, 50, 50, 40);
            server1.Attack(server2);
            server2.Attack(server1);
            Assert.That(server1.Utps.First().Mode == UtpModes.Back);
            Assert.That(server2.Utps.First().Mode == UtpModes.Attack);
        }

        /// <summary>
        /// Test if Two Enemy Servers Attack Eachothe Then They Will Fight In Midle Of Battlefild
        /// </summary>
        [Test]
        public void If_Two_Enemy_Servers_Attack_Eachothe_Then_They_Will_Fight_In_Midle_Of_Battlefild()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            Mock<ICpuPlayer> cpumock = new Mock<ICpuPlayer>();
            IServerNetworkController server1 = new ServerObject(humanmock.Object, 40, 0, 0, 40);
            IServerNetworkController server2 = new ServerObject(cpumock.Object, 40, 50, 50, 40);
            server1.Attack(server2);
            server2.Attack(server1);
            Assert.That(server1.Utps.First().Mode == UtpModes.MoveToBattle);
            Assert.That(server2.Utps.First().Mode == UtpModes.MoveToBattle);
        }

        /// <summary>
        /// Test if Level1 Server With 14 Life Get New Life Then It Will Level Up
        /// </summary>
        [Test]
        public void Level1_Server_With_14_Life_Get_New_Life_Then_It_Will_Level_Up()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            IServerNetworkController server = new ServerObject(humanmock.Object, 14, 0, 0, 40);
            for (int i = 0; i < 20; i++)
            {
                server.Tick();
            }

            Assert.That(server.Level == 2);
        }

        /// <summary>
        /// Test if Cable Have Charge And Call Tick Method Then DelayPoint Will Increased By One
        /// </summary>
        [Test]
        public void If_Cable_Have_Charge_And_Call_Tick_Method_Then_DelayPoint_Will_Increased_By_One()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            Mock<IChargeble> chargeblemock = new Mock<IChargeble>();
            ICable cable = new Cable(chargeblemock.Object) { ChargeOwner = humanmock.Object };
            int befortick = cable.DelayPoint;
            cable.Tick();
            Assert.That(cable.DelayPoint == befortick + 1);
        }

        /// <summary>
        /// Test if Charged Cable Ticked DELAY Times Then It Will Give The Charge To The Next Cable
        /// </summary>
        [Test]
        public void If_Charged_Cable_Ticked_Delay_Times_Then_It_Will_Give_The_Charge_To_The_Next_Cable()
        {
            Mock<IHumanPlayer> humanmock = new Mock<IHumanPlayer>();
            Mock<IChargeble> chargeblemock = new Mock<IChargeble>();
            ICable cable1 = new Cable(chargeblemock.Object);
            ICable cable2 = new Cable(cable1) { ChargeOwner = humanmock.Object };
            for (int i = 0; i < Cable.Delaystatic; i++)
            {
                cable2.Tick();
            }

            Assert.That(cable2.ChargeOwner == null);
            Assert.That(cable1.ChargeOwner == humanmock.Object);
        }
    }
}
