// <copyright file="GamePage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Navigation;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Business_Logic.Game.Classes;
    using Business_Logic.Game.Interfaces;
    using Wpf.View;
    using Wpf.ViewModel;
    using Wpf.ViewModels;

    /// <summary>
    /// Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GamePage"/> class.
        /// </summary>
        public GamePage()
        {
            this.InitializeComponent();

            // Comment off in case of debug.
            // this.Debug.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (this.DataContext as InGameViewModel).OnPageLoaded();
            Random r = new Random();
            Uri[] menuMusic = new Uri[3]
            {
                new Uri(@"Resources\Sounds\Menu\EasyLevel.mp3", UriKind.Relative),
                new Uri(@"Resources\Sounds\Menu\HardLevel.mp3", UriKind.Relative),
                new Uri(@"Resources\Sounds\Menu\HackingAmbient.mp3", UriKind.Relative)
            };

            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Open(menuMusic[r.Next(0, 3)]);
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Play();
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.MediaEnded += this.Soundplayer_MediaEnded;
        }

        private void Soundplayer_MediaEnded(object sender, EventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Position = TimeSpan.FromSeconds(0);
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Play();
        }

        private void Page_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as InGameViewModel).OnMouseButtonUp(e.GetPosition(this.GA));
        }

        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            (this.DataContext as InGameViewModel).OnMouseButtonDown(e.GetPosition(this.GA));
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            Point p = e.GetPosition(this.GA);

            // p = VectorHandler.VH.NormalPosition(p);
            this.MousePosText2.Content = string.Format("Game area: X = {0}, Y = {1}", p.X, (int)p.Y);
        }

        private void Page_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).Ingame.ChangeDirection();
        }
    }
}
