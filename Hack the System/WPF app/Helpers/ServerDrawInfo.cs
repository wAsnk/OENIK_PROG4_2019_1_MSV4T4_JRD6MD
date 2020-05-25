// <copyright file="ServerDrawInfo.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using Business_Logic.Game.Interfaces;

    /// <summary>
    /// Class to handle a playertype object and it's images
    /// </summary>
    public class ServerDrawInfo
    {
        /// <summary>
        /// HeightsPropotion values for servers
        /// </summary>
        public static readonly double[] HeightsPropotion =
            {
            347.0 / 372,
           346.0 / 392,
           346.0 / 391,
           392.0 / 393,
           435.0 / 395,
           463.0 / 390,
           532.0 / 389
        };

        /// <summary>
        /// HeightsPropotion value for router
        /// </summary>
        public static readonly double RouterHeightsPropotion = 89.0 / 113;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerDrawInfo"/> class.
        /// </summary>
        /// <param name="playerType">Get's player type</param>
        /// <param name="color">Get's player color</param>
        public ServerDrawInfo(PlayerType playerType, Color color)
        {
            this.GenerateServerPictures(playerType);
            this.SolidColor = new SolidColorBrush(color);
        }

        /// <summary>
        /// Gets or sets solid color for a playertype
        /// </summary>
        public SolidColorBrush SolidColor { get; set; }

        /// <summary>
        /// Gets or sets server images for a playertype
        /// </summary>
        public ImageBrush[] ServerImages { get; set; }

        /// <summary>
        /// Gets router image for a playertype
        /// </summary>
        public ImageBrush Router { get; private set; }

        private void GenerateServerPictures(PlayerType playerType)
        {
            this.ServerImages = new ImageBrush[7];
            string playerT = playerType.ToString();

            for (int i = 0; i < this.ServerImages.Length; i++)
            {
                this.ServerImages[i] = new ImageBrush();
                this.ServerImages[i].ImageSource = new BitmapImage(new Uri($"Resources\\Servers\\{playerT}\\LVL{i}.png", UriKind.Relative));
            }

            this.Router = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri($"Resources\\Router\\{playerT}\\Router.png", UriKind.Relative))
            };
        }
    }
}
