// <copyright file="GameObject.cs" company="PlaceholderCompany">
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
    using Business_Logic.Game.Classes.LineSegmentIntersection;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Classes;
    using Business_Logic.GameObjects.Interfaces;
    using Business_Logic.Profile.Interfaces;
    using Repository.Interfaces;

    /// <summary>
    /// Enum to define direction
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// Defines UP
        /// </summary>
        Up = 0,

        /// <summary>
        /// Defines Right
        /// </summary>
        Right = 1,

        /// <summary>
        /// Defines Down
        /// </summary>
        Down = 2,

        /// <summary>
        /// Defines left
        /// </summary>
        Left = 3
    }

    /// <summary>
    /// Defines gametypes with enum
    /// </summary>
    public enum GameType
    {
        /// <summary>
        /// Map is random generated
        /// </summary>
        Random,

        /// <summary>
        /// Map is a campaign map
        /// </summary>
        Campaign
    }

    /// <summary>
    /// This class handle logic of game
    /// </summary>
    [Serializable]
    public class GameObject : IGameModel, IGameLogic
    {
        /// <summary>
        /// Gets or sets this dictionary which contains every useable player
        /// </summary>
        public static readonly Dictionary<PlayerType, IPlayer> AllPlayers = new Dictionary<PlayerType, IPlayer>
            {
                { PlayerType.LocalHumanPlayer, new HumanPlayer() },
                { PlayerType.EasyCpu, new EasyCpu() },
                { PlayerType.HardCpu, new HardCpu() },
                { PlayerType.Nobody, null }
            };

        private static Random random = new Random();

        private readonly int startscore = 3000;

        private readonly int minusscore = 5;

        private readonly double scoretime = 40;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameObject"/> class.
        /// </summary>
        public GameObject()
        {
            this.Messages = new List<IGameMessage>();
            this.GameObjects = new List<IGameObject>();
            this.Players = new List<IPlayer>();
        }

        /// <inheritdoc/>
        public event EventHandler<GameOverEventArgs> GameOver;

        /// <inheritdoc/>
        public List<IGameObject> GameObjects { get; set; }

        /// <inheritdoc/>
        public List<IPlayer> Players { get; set; }

        /// <inheritdoc/>
        public int Time
        {
            get { return (int)(0.025 * this.TickCount); }
        }

        /// <inheritdoc/>
        public int LifeLimit { get; set; }

        /// <inheritdoc/>
        public IHumanPlayer LocalPlayer { get; set; }

        /// <inheritdoc/>
        public List<IGameMessage> Messages { get; set; }

        /// <inheritdoc/>
        public int Score { get; set; }

        /// <inheritdoc/>
        public int MapHeight { get; set; }

        /// <inheritdoc/>
        public int MapWidth { get; set; }

        /// <inheritdoc/>
        public int TileSize
        {
            get { return 50; }
        }

        /// <inheritdoc/>
        public int TickCount { get; set; }

        /// <inheritdoc/>
        public GameType GameType { get; set; }

        /// <inheritdoc/>
        public int MapNumber { get; set; }

        /// <inheritdoc/>
        public void ConnectTwoServer(IActiveNetworkController source, INetworkController target)
        {
            if (this.IsEnableConnection(source, target))
            {
                source.Attack(target);
            }
            else
            {
                this.Messages.Add(new GameMessage("Firewall blocked connection"));
            }
        }

        /// <inheritdoc/>
        public void CutUtp(IUtp utp, ICable cable)
        {
            utp.Cut(cable);
        }

        /// <inheritdoc/>
        public void DisconectUtp(IUtp utp)
        {
            utp.Aback();
        }

        /// <inheritdoc/>
        public void GenerateMap()
        {
            this.GameType = GameType.Random;
            this.TickCount = 0;
            this.Score = this.startscore;

            this.MapHeight = random.Next(8, 15);
            this.MapWidth = random.Next(15, 20);

            this.LifeLimit = (random.Next(30, 201) / 10) * 10;

            this.LocalPlayer = AllPlayers[PlayerType.LocalHumanPlayer] as IHumanPlayer;
            this.Players.Clear();
            this.Messages.Clear();
            this.GameObjects.Clear();
            this.AddPlayer(PlayerType.LocalHumanPlayer);

            switch (random.Next(0, 3))
            {
                case 0:
                    this.AddPlayer(PlayerType.EasyCpu);
                    break;
                case 1:
                    this.AddPlayer(PlayerType.HardCpu);
                    break;
                case 2:
                    this.AddPlayer(PlayerType.EasyCpu);
                    this.AddPlayer(PlayerType.HardCpu);
                    break;
            }

            this.AddInactiveServers();
            this.AddInactiveRouters();

            this.SortGameObjects();
        }

        /// <inheritdoc/>
        public void InteractionEnd(Point location)
        {
            if (this.LocalPlayer.IsSelectPoint)
            {
                this.CableSegmentDetection(new Vector(this.LocalPlayer.SelectedX, this.LocalPlayer.SelectedY), new Vector(location.X, location.Y));
                this.LocalPlayer.DiselectPoint();
            }
        }

        /// <inheritdoc/>
        public void InteractionStart(Point location)
        {
            this.LocalPlayer.SelectPoint(location);
        }

        /// <inheritdoc/>
        public void LoadMap(IMap map)
        {
            this.GameType = GameType.Campaign;
            this.MapNumber = map.Id;
            IMapDatas datas = map.MapData;
            this.TickCount = 0;
            this.Score = this.startscore;
            this.MapHeight = (int)datas.MapSize.Height;
            this.MapWidth = (int)datas.MapSize.Width;

            this.LifeLimit = datas.PowerLimit;

            this.LocalPlayer = AllPlayers[PlayerType.LocalHumanPlayer] as IHumanPlayer;
            this.Players.Clear();

            foreach (var item in datas.Players)
            {
                this.Players.Add(AllPlayers[(PlayerType)Enum.Parse(typeof(PlayerType), item)]);
            }

            this.Messages.Clear();
            this.GameObjects.Clear();
            foreach (var item in datas.Servers)
            {
                string[] data = item.Split('#');
                if (data[0] == "Active")
                {
                    IGameObject server = new ServerObject(
                        AllPlayers[(PlayerType)Enum.Parse(typeof(PlayerType), data[1])],
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        this.LifeLimit);
                    this.GameObjects.Add(server);
                }
                else
                {
                    IGameObject server = new ServerObject(
                        int.Parse(data[1]),
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        this.LifeLimit);
                    this.GameObjects.Add(server);
                }
            }

            foreach (var item in datas.Routers)
            {
                string[] data = item.Split('#');
                if (data[0] == "Active")
                {
                    IGameObject router = new Router(
                        AllPlayers[(PlayerType)Enum.Parse(typeof(PlayerType), data[1])],
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        int.Parse(data[4]),
                        this.LifeLimit);
                    this.GameObjects.Add(router);
                }
                else
                {
                    IGameObject router = new Router(
                        int.Parse(data[1]),
                        int.Parse(data[2]),
                        int.Parse(data[3]),
                        this.LifeLimit);
                    this.GameObjects.Add(router);
                }
            }

            foreach (var item in datas.FireWalls)
            {
                string[] data = item.Split('#');
                IGameObject firewall = new FireWall(int.Parse(data[0]), int.Parse(data[1]));

                this.GameObjects.Add(firewall);
            }

            this.SortGameObjects();
        }

        /// <inheritdoc/>
        public void Tick()
        {
            foreach (var item in this.GameObjects)
            {
                item.Tick();
            }

            foreach (var item in this.Players)
            {
                ICpuPlayer cPTemp = item as ICpuPlayer;
                if (cPTemp != null)
                {
                    cPTemp.MakeDecision(this, this);
                }
            }

            foreach (var item in this.Messages)
            {
                item.Tick();
            }

            this.GameOverCheck();

            this.TickCount++;
            if ((double)this.TickCount % this.scoretime == 0 && this.Score != 0)
            {
                this.Score -= this.minusscore;
            }
        }

        /// <inheritdoc/>
        public IGameModel SaveGame()
        {
            this.GameOver = null;
            return this;
        }

        /// <inheritdoc/>
        public void LoadGame(IGameModel gameModel)
        {
            this.GameObjects = gameModel.GameObjects;
            this.LifeLimit = gameModel.LifeLimit;
            this.LocalPlayer = gameModel.LocalPlayer;
            this.Messages = gameModel.Messages;
            this.Players = gameModel.Players;
            this.Score = gameModel.Score;
            this.TickCount = gameModel.TickCount;
            this.MapWidth = gameModel.MapWidth;
            this.MapHeight = gameModel.MapHeight;
            this.GameType = gameModel.GameType;
            this.MapNumber = gameModel.MapNumber;
        }

        /// <inheritdoc/>
        public void InteractionStart(INetworkController networkController)
        {
            IActiveNetworkController aNCTemp = networkController as IActiveNetworkController;
            if (networkController.IsEnable && aNCTemp != null && networkController.Owner == this.LocalPlayer)
            {
                this.LocalPlayer.SelectController(aNCTemp);
            }
        }

        /// <inheritdoc/>
        public void InteractionEnd(INetworkController networkController)
        {
            if (this.LocalPlayer.SelectedController != null)
            {
                if (this.LocalPlayer.SelectedController != networkController)
                {
                    this.ConnectTwoServer(this.LocalPlayer.SelectedController, networkController);
                }

                this.LocalPlayer.SelectController(null);
            }
        }

        /// <inheritdoc/>
        public void SortGameObjects(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: // ok
                    this.GameObjects.Sort((x, y) =>
                    {
                        if (x.X < y.X)
                        {
                            return 1;
                        }
                        else if (y.X < x.X)
                        {
                            return -1;
                        }
                        else if (x.Y < y.Y)
                        {
                            return 1;
                        }
                        else if (y.Y < x.Y)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    break;
                case Direction.Right:
                    this.GameObjects.Sort((x, y) =>
                    {
                        if (x.Y < y.Y)
                        {
                            return 1;
                        }
                        else if (y.Y < x.Y)
                        {
                            return -1;
                        }
                        else if (x.X < y.X)
                        {
                            return -1;
                        }
                        else if (y.X < x.X)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    break;
                case Direction.Down: // def
                    this.GameObjects.Sort((x, y) =>
                    {
                        if (x.X < y.X)
                        {
                            return -1;
                        }
                        else if (y.X < x.X)
                        {
                            return 1;
                        }
                        else if (x.Y < y.Y)
                        {
                            return -1;
                        }
                        else if (y.Y < x.Y)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    break;
                case Direction.Left:
                    this.GameObjects.Sort((x, y) =>
                    {
                        if (x.Y < y.Y)
                        {
                            return -1;
                        }
                        else if (y.Y < x.Y)
                        {
                            return 1;
                        }
                        else if (x.X < y.X)
                        {
                            return 1;
                        }
                        else if (y.X < x.X)
                        {
                            return -1;
                        }
                        else
                        {
                            return 0;
                        }
                    });
                    break;
            }
        }

        private void CableSegmentDetection(Vector vector1, Vector vector2)
        {
            IEnumerable<List<IUtp>> utps = this.GameObjects
                .Where(x => x is IActiveNetworkController)
                .Select(x => x as IActiveNetworkController)
                .Where(x => x.Owner == this.LocalPlayer).Select(x => x.Utps);

            foreach (List<IUtp> uTPs in utps)
            {
                foreach (IUtp utp in uTPs)
                {
                    Vector vector3 = new Vector(utp.Parent.X, utp.Parent.Y);
                    Vector vector4 = new Vector(utp.First.X, utp.First.Y);
                    bool segment = LineSegment.LineSegementsIntersect(vector1, vector2, vector3, vector4, out Vector resoult);
                    if (segment)
                    {
                        if (!utp.IsActive)
                        {
                            this.DisconectUtp(utp);
                        }
                        else
                        {
                            double min = utp.Cables.Min(x => Math.Sqrt(Math.Pow(resoult.X - x.X, 2) + Math.Pow(resoult.Y - x.Y, 2)));
                            var cable = utp.Cables.Where(x => Math.Sqrt(Math.Pow(resoult.X - x.X, 2) + Math.Pow(resoult.Y - x.Y, 2)) == min).FirstOrDefault();

                            this.CutUtp(utp, cable);
                        }
                    }
                }
            }
        }

        private INetworkController FindNetworkController(Point location, IGameModel gm)
        {
            return this.GameObjects
           .Where(x => x is INetworkController)
           .Select(x => x as INetworkController)
           .Where(x => new Rectangle((int)(x.X - (gm.TileSize / 2)), (int)(x.Y - (gm.TileSize / 2)), gm.TileSize, gm.TileSize).Contains(location))
           .FirstOrDefault();
        }

        private bool IsEnableConnection(IActiveNetworkController source, INetworkController target)
        {
            List<IFireWall> firewalls = this.Firewalls();

            foreach (var item in firewalls)
            {
                if (this.FireWallDetection(source, target, item))
                {
                    return false;
                }
            }

            return true;
        }

        private bool FireWallDetection(IActiveNetworkController source, INetworkController target, IFireWall fireWall)
        {
            Rectangle r = new Rectangle((int)(fireWall.X - (this.TileSize / 2)), (int)(fireWall.Y - (this.TileSize / 2)), this.TileSize, this.TileSize);

            Vector sourcev = new Vector(source.X, source.Y);
            Vector targetv = new Vector(target.X, target.Y);

            Vector firewallv1 = new Vector(r.X, r.Y);
            Vector firewallv2 = new Vector(r.X + r.Width, r.Y + r.Height);
            Vector firewallv3 = new Vector(r.X + r.Width, r.Y);
            Vector firewallv4 = new Vector(r.X, r.Y + r.Height);
            Vector outv = new Vector();
            var hit1 = LineSegment.LineSegementsIntersect(sourcev, targetv, firewallv1, firewallv2, out outv);
            var hit2 = LineSegment.LineSegementsIntersect(sourcev, targetv, firewallv3, firewallv4, out outv);

            return hit1 || hit2;
        }

        private List<IFireWall> Firewalls()
        {
            return this.GameObjects.Where(x => x is IFireWall).Select(x => x as IFireWall).ToList();
        }

        private void SortGameObjects()
        {
            this.GameObjects.Sort((x, y) =>
            {
                if (x.X < y.X)
                {
                    return -1;
                }
                else if (y.X < x.X)
                {
                    return 1;
                }
                else if (x.Y < y.Y)
                {
                    return -1;
                }
                else if (y.Y < x.Y)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });
        }

        private void AddInactiveRouters()
        {
            for (int i = 0; i < random.Next(0, 2); i++)
            {
                IGameObject emptyserver = new Router(random.Next(10, Math.Min(this.LifeLimit, Router.RouterLifeLimit)), (random.Next(0, this.MapWidth) * 50) + 25, (random.Next(0, this.MapHeight) * 50) + 25, this.LifeLimit);

                if (this.EmptySpace(emptyserver))
                {
                    this.GameObjects.Add(emptyserver);
                }
                else
                {
                    --i;
                }
            }
        }

        private void AddInactiveServers()
        {
            for (int i = 0; i < random.Next(0, 5); i++)
            {
                IGameObject emptyserver = new ServerObject(random.Next(10, this.LifeLimit), (random.Next(0, this.MapWidth) * 50) + 25, (random.Next(0, this.MapHeight) * 50) + 25, this.LifeLimit);

                if (this.EmptySpace(emptyserver))
                {
                    this.GameObjects.Add(emptyserver);
                }
                else
                {
                    --i;
                }
            }
        }

        private void AddPlayer(PlayerType player)
        {
            this.Players.Add(AllPlayers[player]);
            for (int i = 0; i < random.Next(1, 6); i++)
            {
                IGameObject server = new ServerObject(AllPlayers[player], random.Next(10, this.LifeLimit), (random.Next(0, this.MapWidth) * 50) + 25, (random.Next(0, this.MapHeight) * 50) + 25, this.LifeLimit);
                if (this.EmptySpace(server))
                {
                    this.GameObjects.Add(server);
                }
                else
                {
                    --i;
                }
            }
        }

        private bool EmptySpace(IGameObject gameObject)
        {
            foreach (var item in this.GameObjects)
            {
                for (int i = -1; i < 2; i++)
                {
                    for (int j = -1; j < 2; j++)
                    {
                        if (item.X + (50 * i) == gameObject.X && item.Y + (50 * j) == gameObject.Y)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void GameOverCheck()
        {
            IEnumerable<INetworkController> networkcontrolers = this.GameObjects.Where(x => x is INetworkController).Select(x => x as INetworkController);
            if (networkcontrolers.Where(x => x.Owner == this.LocalPlayer).Count() == 0)
            {
                if (this.GameType == GameType.Random)
                {
                    this.GameOver?.Invoke(this, new GameOverEventArgs(false));
                }
                else
                {
                    this.GameOver?.Invoke(this, new GameOverEventArgs(false, this.MapNumber));
                }
            }

            if (networkcontrolers.Where(x => x.Owner != this.LocalPlayer).Count() == 0)
            {
                if (this.GameType == GameType.Random)
                {
                    this.GameOver?.Invoke(this, new GameOverEventArgs(true));
                }
                else
                {
                    this.GameOver?.Invoke(this, new GameOverEventArgs(true, this.MapNumber));
                }
            }
        }
    }
}
