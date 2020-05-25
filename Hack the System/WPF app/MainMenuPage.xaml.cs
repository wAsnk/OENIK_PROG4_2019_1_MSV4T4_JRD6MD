// <copyright file="MainMenuPage.xaml.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Media;
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
    using Wpf.ViewModel;

    /// <summary>
    /// Interaction logic for MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        // private static SoundPlayer buttonClickSound = new SoundPlayer(Properties.Resources.ButtonClick);

        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuPage"/> class.
        /// </summary>
        public MainMenuPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Exit from application
        /// </summary>
        /// <param name="sender">Sender of the click</param>
        /// <param name="e">Event args</param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            Application.Current.Shutdown();
        }
    }
}
