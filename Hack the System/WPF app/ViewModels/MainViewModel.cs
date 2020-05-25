// <copyright file="MainViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf.ViewModels
{
    // TODO: Angol szöveg felmondása a magyar helyett.
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Navigation;
    using Business_Logic.Profile.Classes;
    using Business_Logic.Profile.Exceptions;
    using Business_Logic.Profile.Interfaces;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using Wpf.ViewModel;
    using static System.Net.Mime.MediaTypeNames;

    /// <summary>
    /// Mainviewmodel class
    /// </summary>
    public class MainViewModel : ObservableObject
    {
        private readonly MediaPlayer story = new MediaPlayer();
        private MediaPlayer soundplayer = new MediaPlayer();
        private ProfileObject selectedProfile;
        private IMap selectedMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            // this.profilePage = profilePage;
            this.PL = new ProfileLoader();

            this.Nav_ToMainMenuCommand = new RelayCommand(this.Nav_ToMainMenuPageMethod);
            this.Nav_ToProfilePageCommand = new RelayCommand(this.Nav_ToProfilePageMethod);
            this.Nav_ToPlayMenuPageCommand = new RelayCommand(this.Nav_ToPlayMenuPageMethod);
            this.Nav_ToGamePageCommand = new RelayCommand(this.Nav_ToGamePageMethod);
            this.Nav_ToStoryPageCommand = new RelayCommand(this.Nav_ToStoryPageMethod);
            this.Nav_ToCampaignPageCommand = new RelayCommand(this.Nav_ToCampaignPageMethod);
            this.Nav_ToCreditsPageCommand = new RelayCommand(this.Nav_ToCreditsPageMethod);
            this.Nav_ToMainMenuPageFromStroyCommand = new RelayCommand(this.Nav_ToMainMenuPageFromStroyMethod);

            this.NewProfileCommand = new RelayCommand(this.NewProfile);
            this.SaveNewProfileCommand = new RelayCommand(this.SaveNewProfileMethod, () => !this.NameIsEmpty());
            this.DelProfileCommand = new RelayCommand(this.DelProfile, () => this.SelectedProfile != null && this.SelectedProfile.Name != this.ActualProfile.Name);
            this.SelectProfileCommand = new RelayCommand(this.SelectProfile, () => this.SelectedProfile != null && this.SelectedProfile.Name != this.ActualProfile.Name);
            this.LoadSelectedMapCommand = new RelayCommand(this.LoadSelectedCampaignMapMethod, () => this.SelectedMap != null);

            try
            {
                this.PL.LoadDefaultProfile();
            }
            catch (DefaultPlayerNotFoundException)
            {
                this.PL.CreateNewProfie("Player");
                this.PL.ChangeProfile("Player");
            }

            this.Soundplayer.MediaEnded += this.Soundplayer_MediaEnded;
        }

        /// <summary>
        /// Gets or sets soundplayer element for menu music
        /// </summary>
        public MediaPlayer Soundplayer
        {
            get
            {
                return this.soundplayer;
            }

            set
            {
                this.soundplayer = value;
            }
        }

        /// <summary>
        /// Gets and sets Profile Loader instance
        /// </summary>
        public ProfileLoader PL { get; private set; }

        /// <summary>
        /// Gets, On the fly property for Actual selected profile which is choosen to play with.
        /// </summary>
        public ProfileObject ActualProfile
        {
            get
            {
                return (ProfileObject)this.PL.Profile;
            }
        }

        /// <summary>
        /// Gets or sets selected profile in the profile list
        /// </summary>
        public ProfileObject SelectedProfile
        {
            get
            {
                return this.selectedProfile;
            }

            set
            {
                this.Set(ref this.selectedProfile, value);
                this.RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the selected map
        /// </summary>
        public IMap SelectedMap
        {
            get
            {
                return this.selectedMap;
            }

            set
            {
                this.selectedMap = value;
            }
        }

        /// <summary>
        /// Gets or sets a help text if a profile already exists.
        /// </summary>
        public string ProfileAlreadyExists { get; set; }

        /// <summary>
        /// Gets or sets a new profile name
        /// </summary>
        public string NewProfileName { get; set; }

        /// <summary>
        /// Gets all the stored profiles. On the fly property
        /// </summary>
        public ObservableCollection<IProfile> ProfileAll
        {
            get
            {
                return this.PL.AllProfile;
            }
        }

        /// <summary>
        /// Gets all campaign maps
        /// </summary>
        public ObservableCollection<IMap> AllMaps
        {
            get
            {
                return this.PL.AllMap;
            }
        }

        /// <summary>
        /// Gets command property for returning to the main menu
        /// </summary>
        public ICommand Nav_ToMainMenuCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to profile page
        /// </summary>
        public ICommand Nav_ToProfilePageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to play menu page
        /// </summary>
        public ICommand Nav_ToPlayMenuPageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to play menu page
        /// </summary>
        public ICommand Nav_ToGamePageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to story page
        /// </summary>
        public ICommand Nav_ToStoryPageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to campaign menu page
        /// </summary>
        public ICommand Nav_ToCampaignPageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigation to credits menu page
        /// </summary>
        public ICommand Nav_ToCreditsPageCommand { get; private set; }

        /// <summary>
        /// Gets command property for navigating from story page to main menu
        /// </summary>
        public ICommand Nav_ToMainMenuPageFromStroyCommand { get; private set; }

        /// <summary>
        /// Gets command property for New Profiles
        /// </summary>
        public ICommand NewProfileCommand { get; private set; }

        /// <summary>
        /// Gets command property to delete profile
        /// </summary>
        public ICommand DelProfileCommand { get; private set; }

        /// <summary>
        /// Gets command property for selecting a profile
        /// </summary>
        public ICommand SelectProfileCommand { get; private set; }

        /// <summary>
        /// Gets command property for saving new profile
        /// </summary>
        public ICommand SaveNewProfileCommand { get; private set; }

        /// <summary>
        /// Gets command property for saving actual game
        /// </summary>
        public ICommand SaveActualGameCommand { get; private set; }

        /// <summary>
        /// Gets command property for saving actual game
        /// </summary>
        public ICommand LoadSelectedMapCommand { get; private set; }

        private ProfilePage ProfilePage { get; set; }

        /// <summary>
        /// RaisePropertyChanged method caller
        /// </summary>
        public void RefresPL()
        {
            this.RaisePropertyChanged(nameof(this.ActualProfile));
            this.RaisePropertyChanged(nameof(this.ActualProfile.CampaignScore));
            this.RaisePropertyChanged(nameof(this.ActualProfile.CompletedLevelCount));
            this.RaisePropertyChanged(nameof(this.ActualProfile.CompletedRandomLevelCount));
            this.RaisePropertyChanged(nameof(this.ActualProfile.TotalScore));

            this.RaisePropertyChanged(nameof(this.SelectedProfile));
            this.RaisePropertyChanged(nameof(this.SelectedProfile.CampaignScore));
            this.RaisePropertyChanged(nameof(this.SelectedProfile.CompletedLevelCount));
            this.RaisePropertyChanged(nameof(this.SelectedProfile.CompletedRandomLevelCount));
            this.RaisePropertyChanged(nameof(this.SelectedProfile.TotalScore));

            this.RaisePropertyChanged(nameof(this.ProfileAll));
        }

        private void Soundplayer_MediaEnded(object sender, EventArgs e)
        {
            this.Soundplayer.Position = TimeSpan.FromSeconds(0);
            this.Soundplayer.Play();
        }

        private void Nav_ToProfilePageMethod()
        {
            this.SelectedProfile = null;
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.ProfileP);
        }

        private void Nav_ToPlayMenuPageMethod()
        {
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.PlayMP);
        }

        private void Nav_ToGamePageMethod()
        {
            MainWindow.MainW.me_backgroundVideo.Stop();
            this.Soundplayer.Stop();
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.GameP);
        }

        private void Nav_ToCampaignPageMethod()
        {
            MainWindow.MainW.CampaignM.RefresWrapPanel();
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.CampaignM);
        }

        private void Nav_ToStoryPageMethod()
        {
            this.Soundplayer.Pause();
            this.story.Open(new Uri(@"Resources\Sounds\Story\Story.wav", UriKind.Relative));
            this.story.Play();
            this.story.Volume = 1;
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.StoryP);
        }

        private void Nav_ToMainMenuPageFromStroyMethod()
        {
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.MainMP);
            this.story.Stop();
            this.Soundplayer.Play();
        }

        private void Nav_ToMainMenuPageMethod()
        {
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.MainMP);
            this.Soundplayer.Volume = 1;
        }

        private void Nav_ToCreditsPageMethod()
        {
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.CreditsP);
            this.Soundplayer.Volume = 0;
        }

        private void NewProfile()
        {
                MainWindow.MainW.ProfileP.tb_NewProfileName.Text = string.Empty;
                MainWindow.MainW.ProfileP.sp_Statistics.Visibility = Visibility.Hidden;
                MainWindow.MainW.ProfileP.sp_NewProfile.Visibility = Visibility.Visible;
                this.ProfileAlreadyExists = string.Empty;
        }

        private void SaveNewProfileMethod()
        {
            try
            {
                this.PL.CreateNewProfie(this.NewProfileName);

                // this.ProfilePage.sp_Statistics.Visibility = Visibility.Visible;
                // this.ProfilePage.sp_NewProfile.Visibility = Visibility.Hidden;
                // this.ProfilePage.tb_NewProfileName.Text = string.Empty;
                MainWindow.MainW.ProfileP.sp_Statistics.Visibility = Visibility.Visible;
                MainWindow.MainW.ProfileP.sp_NewProfile.Visibility = Visibility.Hidden;
                MainWindow.MainW.ProfileP.tb_NewProfileName.Text = string.Empty;
            }
            catch (ProfileAlreadyExistException)
            {
                this.ProfileAlreadyExists = "The profile already exists on this name.";
                this.RaisePropertyChanged(nameof(this.ProfileAlreadyExists));
            }
        }

        private void DelProfile()
        {
            this.PL.DeleteProfile(this.SelectedProfile.Name);
        }

        private void SelectProfile()
        {
            this.PL.ChangeProfile(this.SelectedProfile.Name);
            this.RaisePropertyChanged(nameof(this.ActualProfile));
        }

        private bool NameIsEmpty()
        {
            if (MainWindow.MainW.ProfileP != null)
            {
                return MainWindow.MainW.ProfileP.tb_NewProfileName.Text.Equals(string.Empty);
            }

            return false;
        }

        private void LoadSelectedCampaignMapMethod()
        {
            (App.Current.Resources["Locator"] as ViewModelLocator).Ingame.GL.LoadMap(this.SelectedMap);
            MainWindow.MainW.MainWindowFrame.Navigate(MainWindow.MainW.GameP);
        }
    }
}
