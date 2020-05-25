// <copyright file="CampaignPage.xaml.cs" company="PlaceholderCompany">
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
    using Business_Logic.Profile.Interfaces;
    using Wpf.ViewModel;

    /// <summary>
    /// Interaction logic for CampaignPage.xaml
    /// </summary>
    public partial class CampaignPage : Page
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignPage"/> class.
        /// </summary>
        public CampaignPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Lists campaign maps.
        /// </summary>
        public void RefresWrapPanel()
        {
            this.wp_campaign.Children.Clear();

            foreach (var item in (App.Current.Resources["Locator"] as ViewModelLocator).Main.AllMaps)
            {
                Button l = new Button
                {
                    // Content = $"{item.ID}\nScore: {item.Score}",
                    // HorizontalContentAlignment = HorizontalAlignment.Center,
                    // VerticalContentAlignment = VerticalAlignment.Center,
                    // FontSize = 30,
                    Width = 200,
                    Height = 200
                };

                StackPanel sp = new StackPanel
                {
                    Orientation = Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                Label mapId = new Label
                {
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = $"Map {item.Id}",
                    Style = Application.Current.TryFindResource("MainMenuTextStyle") as Style
                };

                Label score = new Label
                {
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Content = $"Score: {item.Score}",
                    Style = Application.Current.TryFindResource("MainMenuTextStyle") as Style
                };
                sp.Children.Add(mapId);
                sp.Children.Add(score);
                l.Content = sp;

                l.HorizontalContentAlignment = HorizontalAlignment.Center;
                l.VerticalContentAlignment = VerticalAlignment.Center;

                // l.Background = item.IsEnabled ? new ImageBrush(new BitmapImage(new Uri("Resources\\Campaign\\12.png", UriKind.Relative))) : new ImageBrush(new BitmapImage(new Uri("Resources\\Campaign\\12bw.png", UriKind.Relative)));
                l.IsEnabled = item.IsEnabled;
                l.FontWeight = FontWeights.Bold;
                l.Style = Application.Current.TryFindResource("CampaignButtonIcon") as Style;
                l.Margin = new Thickness(20);

                l.Tag = item;

                l.Click += this.L_Click;

                this.wp_campaign.Children.Add(l);
            }
        }

        private void L_Click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            (App.Current.Resources["Locator"] as ViewModelLocator).Ingame.LoadCampaignMap(b.Tag as IMap);
        }
    }
}
