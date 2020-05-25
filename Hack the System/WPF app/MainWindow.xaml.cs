// <copyright file="MainWindow.xaml.cs" company="PlaceholderCompany">
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            MainW = this;
            this.PlayMP = new PlayMenuPage();
            this.ProfileP = new ProfilePage();
            this.MainMP = new MainMenuPage();
            this.GameP = new GamePage();
            this.StoryP = new StoryPage();
            this.IngameM = new IngamePauseMenu();
            this.CampaignM = new CampaignPage();
            this.EndGameP = new EndGamePage();
            this.CreditsP = new CreditsPage();

            this.MainWindowFrame.Navigate(this.MainMP);
        }

        /// <summary>
        /// Gets or sets main Window instance
        /// </summary>
        public static MainWindow MainW { get; set; }

        /// <summary>
        /// Gets or sets Main menu page instance
        /// </summary>
        public MainMenuPage MainMP { get; set; }

        /// <summary>
        /// Gets or sets Play menu page instance
        /// </summary>
        public PlayMenuPage PlayMP { get; set; }

        /// <summary>
        /// Gets or sets Profile page instance
        /// </summary>
        public ProfilePage ProfileP { get; set; }

        /// <summary>
        /// Gets or sets Ingame page instance
        /// </summary>
        public GamePage GameP { get; set; }

        /// <summary>
        /// Gets or sets story page instance
        /// </summary>
        public StoryPage StoryP { get; set; }

        /// <summary>
        /// Gets or sets ingame menu page instance
        /// </summary>
        public IngamePauseMenu IngameM { get; set; }

        /// <summary>
        /// Gets or sets campaign menu page instance
        /// </summary>
        public CampaignPage CampaignM { get; set; }

        /// <summary>
        /// Gets or sets end game menu page instance
        /// </summary>
        public EndGamePage EndGameP { get; set; }

        /// <summary>
        /// Gets or sets end game menu page instance
        /// </summary>
        public CreditsPage CreditsP { get; set; }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Open(new Uri(@"Resources\Sounds\Menu\Menu.mp3", UriKind.Relative));
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Play();
            (App.Current.Resources["Locator"] as ViewModelLocator).Main.Soundplayer.Volume = 1;

            this.me_backgroundVideo.Source = new Uri("Resources/HTS_Main_Menu_BCKGRND_withGlitchText.mp4", UriKind.Relative);
            this.me_backgroundVideo.Play();
            this.me_backgroundVideo.MediaEnded += this.Me_backgroundVideo_MediaEnded;
        }

        private void Me_backgroundVideo_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (!(MainW.MainWindowFrame.Content is GamePage))
            {
                this.me_backgroundVideo.Position = TimeSpan.FromSeconds(0);
                this.me_backgroundVideo.Play();
            }
        }
    }
}
