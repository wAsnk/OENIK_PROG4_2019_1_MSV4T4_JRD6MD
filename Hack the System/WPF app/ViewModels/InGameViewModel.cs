// <copyright file="InGameViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;
    using Business_Logic.Game.Classes;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;
    using Business_Logic.Profile.Interfaces;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using Wpf.Helpers;
    using Wpf.View;
    using Wpf.ViewModel;

    /// <summary>
    /// Represent the ingame view model
    /// </summary>
    public class InGameViewModel : ObservableObject
    {
        private const int EndgameDelayervalue = 40;
        private VectorHandler vh;
        private GameObject game;
        private IGameModel gm;
        private IGameLogic gl;
        private DispatcherTimer dt;
        private int endGameDelayer = 40; // delayer * 25 e.g: 40*25 = 1000 ms => 1 sec delay after game ends

        /// <summary>
        /// Initializes a new instance of the <see cref="InGameViewModel"/> class.
        /// </summary>
        public InGameViewModel()
        {
            this.game = new GameObject();
            this.GL = this.game;
            this.GM = this.game;
            this.GL.GameOver += this.GL_GameOver;

            this.LoadRandomMapCommand = new RelayCommand(this.LoadRandomMapMethod);
            this.LoadMapCommand = new RelayCommand(this.LoadMapMethod);
            this.Show_IngameMenuCommand = new RelayCommand(this.Show_IngameMenuMethod);
            this.Show_GamePageCommand = new RelayCommand(this.Show_GamePageMethod);
            this.Exit_FromIngameCommand = new RelayCommand(this.Exit_FromIngameMethod);
            this.SaveGameStateCommand = new RelayCommand(this.SaveGameStateMethod);
        }

        /// <summary>
        /// Gets or sets vectorhandler instance for isometric functions
        /// </summary>
        public VectorHandler VH
        {
            get
            {
                return this.vh;
            }

            set
            {
                this.vh = value;
            }
        }

        /// <summary>
        /// Gets or sets game logic
        /// </summary>
        public IGameLogic GL
        {
            get
            {
                return this.gl;
            }

            set
            {
                this.gl = value;
            }
        }

        /// <summary>
        /// Gets or sets game model
        /// </summary>
        public IGameModel GM
        {
            get
            {
                return this.gm;
            }

            set
            {
                this.gm = value;
            }
        }

        /// <summary>
        /// Gets Time property
        /// </summary>
        public int Time
        {
            get
            {
                return this.GM.Time;
            }
        }

        /// <summary>
        /// Gets power limit property
        /// </summary>
        public int PowerLimit
        {
            get
            {
                return this.GM.LifeLimit;
            }
        }

        /// <summary>
        /// Gets score
        /// </summary>
        public int Score
        {
            get
            {
                return this.GM.Score;
            }
        }

        /// <summary>
        /// Gets command property for loading a random map
        /// </summary>
        public ICommand LoadRandomMapCommand { get; private set; }

        /// <summary>
        /// Gets command property for loading a selected map
        /// </summary>
        public ICommand LoadMapCommand { get; private set; }

        /// <summary>
        /// Gets command property for showing gamepage
        /// </summary>
        public ICommand Exit_FromIngameCommand { get; private set; }

        /// <summary>
        /// Gets command property for showing gamepage
        /// </summary>
        public ICommand Show_GamePageCommand { get; private set; }

        /// <summary>
        /// Gets command property for showing ingame menu
        /// </summary>
        public ICommand Show_IngameMenuCommand { get; private set; }

        /// <summary>
        /// Gets command property for saving a game state
        /// </summary>
        public ICommand SaveGameStateCommand { get; private set; }

        /* private IProfile Profile { get; set; }

        /// <summary>
        /// Sets a selected profile as 'selected' profile
        /// </summary>
        /// <param name="profile">Profile to be selected.</param>
        public void SetSelectedProfile(IProfile profile)
        {
            this.Profile = profile;
        }*/

        /// <summary>
        /// Changes the direction in VectorHandler
        /// </summary>
        public void ChangeDirection()
        {
            this.VH.ChangeDirection();
            this.GL.SortGameObjects(this.vh.Direction);
        }

        /// <summary>
        /// Loads a selected campaign map
        /// </summary>
        /// <param name="map">Selected map</param>
        public void LoadCampaignMap(IMap map)
        {
            this.GL.LoadMap(map);
            this.CreateNewVectorHandler();
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.GameP);
        }

        /// <summary>
        /// On page load starts timer
        /// </summary>
        public void OnPageLoaded()
        {
            MainWindow.MainW.GameP.GA.SetupLogic(this.GM);

            this.dt = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(25)
            };
            this.dt.Tick += this.Dt_Tick;
            this.dt.Start();
        }

        /// <summary>
        /// On mouse button down starts an interaction.
        /// </summary>
        /// <param name="point">Interaction start point</param>
        public void OnMouseButtonDown(Point point)
        {
            INetworkController clickedcontroller = this.FindClickedContrller(point);
            if (clickedcontroller != null)
            {
                this.GL.InteractionStart(clickedcontroller);
            }
            else
            {
                Point pointToGA = this.vh.NormalPosition(point);
                System.Drawing.Point p = new System.Drawing.Point((int)pointToGA.X, (int)pointToGA.Y);
                this.GL.InteractionStart(p);
            }
        }

        /// <summary>
        /// On mouse button up starts an interaction.
        /// </summary>
        /// <param name="point">Interaction start point</param>
        public void OnMouseButtonUp(Point point)
        {
            INetworkController clickedcontroller = this.FindClickedContrller(point);
            if (clickedcontroller != null && this.GM.LocalPlayer.SelectedController != null)
            {
                this.GL.InteractionEnd(clickedcontroller);
            }
            else
            {
                Point pointToGA = this.vh.NormalPosition(point);
                System.Drawing.Point p = new System.Drawing.Point((int)pointToGA.X, (int)pointToGA.Y);
                this.GL.InteractionEnd(p);
            }
        }

        /// <summary>
        /// Loads a random map
        /// </summary>
        public void LoadRandomMapMethod()
        {
            this.gl.GenerateMap();
            this.CreateNewVectorHandler();
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.GameP);
        }

        /// <summary>
        /// Saves game state
        /// </summary>
        public void SaveGameStateMethod()
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.PL.SaveGame(this.GL.SaveGame());
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.RefresPL();
            this.GL.GameOver += this.GL_GameOver;
            MainWindow.MainW.IngameM.Helper.Visibility = Visibility.Visible;
            MainWindow.MainW.IngameM.Helper.Content = "Game saved.";
        }

        /// <summary>
        /// Navigates to the profile page
        /// </summary>
        public void Show_IngameMenuMethod()
        {
            this.dt.Stop();
            MainWindow.MainW.IngameM.Helper.Visibility = Visibility.Hidden;
            MainWindow.MainW.GameP.PauseWindowFrame.Navigate(MainWindow.MainW.IngameM);
            MainWindow.MainW.GameP.PauseWindowFrame.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Navigates to the profile page
        /// </summary>
        public void Show_GamePageMethod()
        {
            this.dt.Start();
            MainWindow.MainW.GameP.PauseWindowFrame.Visibility = Visibility.Hidden;
            MainWindow.MainW.IngameM.Helper.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Navigates to the profile page
        /// </summary>
        public void Exit_FromIngameMethod()
        {
            MainWindow.MainW.me_backgroundVideo.Source = new Uri("Resources/HTS_Main_Menu_BCKGRND_withGlitchText.mp4", UriKind.Relative);
            MainWindow.MainW.me_backgroundVideo.Play();
            MainWindow.MainW.GameP.PauseWindowFrame.Visibility = Visibility.Hidden;
            MainWindow.MainW.IngameM.Helper.Visibility = Visibility.Hidden;
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Open(new Uri(@"Resources\Sounds\Menu\Menu.mp3", UriKind.Relative));
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Play();

            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.MainMP);
        }

        private void LoadMapMethod()
        {
            if ((App.Current.Resources["Locator"] as ViewModelLocator).Main.ActualProfile != null && (App.Current.Resources["Locator"] as ViewModelLocator).Main.ActualProfile.EnableSavedGame)
            {
                this.GL.LoadGame((App.Current.Resources["Locator"] as ViewModelLocator).Main.PL.LoadGame());
                this.CreateNewVectorHandler();
                MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.GameP);
            }
        }

        private void Dt_Tick(object sender, EventArgs e)
        {
            this.GL.Tick();
            MainWindow.MainW.GameP.GA.InvalidateVisual();
        }

        private void GL_GameOver(object sender, GameOverEventArgs e)
        {
            if (this.endGameDelayer-- == 0)
            {
                this.RaisePropertyChanged(nameof(this.Score));
                this.RaisePropertyChanged(nameof(this.Time));
                this.dt.Stop();
                MainWindow.MainW.GameP.PauseWindowFrame.Navigate(MainWindow.MainW.EndGameP);
                MainWindow.MainW.GameP.PauseWindowFrame.Visibility = Visibility.Visible;

                if (e.PlayerWin)
                {
                    MainWindow.MainW.EndGameP.lb_gameOver.Content = "YOU WON!";
                    this.SaveProfileState(e);
                }
                else
                {
                    MainWindow.MainW.EndGameP.lb_gameOver.Content = "YOU LOST!";
                }

                this.endGameDelayer = EndgameDelayervalue;
            }
        }

        private void SaveProfileState(GameOverEventArgs e)
        {
            switch (e.GameType)
            {
                case GameType.Random:
                    (App.Current.Resources["Locator"] as ViewModelLocator).Main.PL.WinRandomLevel();
                    (App.Current.Resources["Locator"] as ViewModelLocator).Main.RefresPL();
                    break;
                case GameType.Campaign:
                    (App.Current.Resources["Locator"] as ViewModelLocator).Main.PL.WinCampaign(e.MapNumber - 1, this.Score);
                    (App.Current.Resources["Locator"] as ViewModelLocator).Main.RefresPL();
                    break;
            }
        }

        private INetworkController FindClickedContrller(Point point)
        {
            foreach (var item in GameArea.DrawedNetworkControllers)
            {
                if (item.Value.Contains(point))
                {
                    // TODO: Pontosabb kijelölés
                    return item.Key;
                }
            }

            return null;
        }

        private void CreateNewVectorHandler()
        {
            this.vh = new VectorHandler(2f, MainWindow.MainW.ActualWidth, MainWindow.MainW.ActualHeight, this.GM.MapWidth, this.GM.MapHeight, this.GM.TileSize);
        }
    }
}
