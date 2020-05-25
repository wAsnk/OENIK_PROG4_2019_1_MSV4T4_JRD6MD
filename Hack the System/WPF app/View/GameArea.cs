// <copyright file="GameArea.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Wpf.View
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    using Business_Logic.Game.Classes;
    using Business_Logic.Game.Interfaces;
    using Business_Logic.GameObjects.Interfaces;
    using Wpf.Helpers;
    using Wpf.ViewModel;

    /// <summary>
    /// Game area class
    /// </summary>
    public class GameArea : FrameworkElement
    {
        private Dictionary<PlayerType, ServerDrawInfo> sdi;

        /// <summary>
        /// Game logic private property
        /// </summary>
        private IGameModel gm;

        private VectorHandler vh;

        /// <summary>
        /// Gets or sets drawed network controllers dictionary
        /// </summary>
        public static Dictionary<INetworkController, Rect> DrawedNetworkControllers { get; set; }

        /// <summary>
        /// New instance of game area
        /// </summary>
        /// <param name="gamemodel">gets gamelogic</param>
        public void SetupLogic(IGameModel gamemodel)
        {
            this.gm = gamemodel;
            DrawedNetworkControllers = new Dictionary<INetworkController, Rect>();
            this.vh = (App.Current.Resources["Locator"] as ViewModelLocator).Ingame.VH;
            ServerDrawInfo easyCpu = new ServerDrawInfo(PlayerType.EasyCpu, Colors.Firebrick);
            ServerDrawInfo hardCpu = new ServerDrawInfo(PlayerType.HardCpu, Colors.ForestGreen);
            ServerDrawInfo localHumanPlayer = new ServerDrawInfo(PlayerType.LocalHumanPlayer, Colors.MediumTurquoise);
            ServerDrawInfo nobody = new ServerDrawInfo(PlayerType.Nobody, Colors.Gray);
            this.sdi = new Dictionary<PlayerType, ServerDrawInfo>
            {
                { PlayerType.EasyCpu, easyCpu },
                { PlayerType.HardCpu, hardCpu },
                { PlayerType.LocalHumanPlayer, localHumanPlayer },
                { PlayerType.Nobody, nobody }
            };
        }

        /// <summary>
        /// Renders the map and the objects
        /// </summary>
        /// <param name="drawingContext">Gets drawing contexts</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (this.gm != null)
            {
                this.DrawBackgroundImage(drawingContext);

                DrawedNetworkControllers.Clear();
                this.IsoDrawMap(drawingContext);
                foreach (var item in this.gm.GameObjects)
                {
                    if (item is INetworkController nCTemp)
                    {
                        this.IsoDrawCabel(nCTemp, drawingContext);
                    }
                }

                foreach (var item in this.gm.GameObjects)
                {
                    IFireWall fWTemp = item as IFireWall;
                    IInactiveNetworkController iNCTemp = item as IInactiveNetworkController;

                    if (item is IServerNetworkController sNCTemp)
                    {
                        this.IsoDrawNetworkControl(sNCTemp, drawingContext);
                    }
                    else if (fWTemp != null)
                    {
                        this.IsoDrawFirewall(fWTemp, drawingContext);
                    }
                    else if (iNCTemp != null)
                    {
                        this.IsoDrawRouter(iNCTemp, drawingContext);
                    }
                }

                this.DrawTimeScore(drawingContext);
            }
        }

        private void DrawBackgroundImage(DrawingContext drawingContext)
        {
            ImageBrush bckgroundImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Resources\\GamePage_Background.png", UriKind.Relative))
            };
            drawingContext.DrawRectangle(bckgroundImage, null, new Rect(new Point(0, 0), new Size(1920, 1080)));
        }

        private void IsoDrawMap(DrawingContext drawingContext)
        {
            /*ImageBrush tile = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"Resources\TablePattern.png", UriKind.Relative)),
                TileMode = TileMode.Tile
            };
            int[,] map = new int[15, 8];*/
            int h = 50;
            for (int i = 0; i < this.gm.MapWidth; i++)
            {
                for (int j = 0; j < this.gm.MapHeight; j++)
                {
                    StreamGeometry streamGeometry = new StreamGeometry();
                    using (StreamGeometryContext geometryContext = streamGeometry.Open())
                    {
                        geometryContext.BeginFigure(this.vh.IsoPosition(new Point(i * h, j * h)), true, true);
                        PointCollection points = this.PolygonPoint(i * h, j * h, h, h);

                        geometryContext.PolyLineTo(points, true, true);
                    }

                    // Draw the polygon visual
                    // DrawingVisual visual = new DrawingVisual();
                    drawingContext.DrawGeometry(Brushes.DarkGray, new Pen(Brushes.Gray, 1), streamGeometry);
                }
            }
        }

        private PointCollection PolygonPoint(int x, int y, int w, int h)
        {
            return new PointCollection
            {
            this.vh.IsoPosition(new Point(x, y)),
            this.vh.IsoPosition(new Point(x + w, y)),
            this.vh.IsoPosition(new Point(x + w, y + h)),
            this.vh.IsoPosition(new Point(x, y + h))
            };
        }

        private void DrawTimeScore(DrawingContext drawingContext)
        {
                FormattedText time = new FormattedText(
                    string.Format($"{(App.Current.Resources["Locator"] as ViewModelLocator).Ingame.Time} SEC"),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Chakra Petch"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                    25,
                    Brushes.White);

            ImageBrush bckgroundImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("Resources\\icon_clock.png", UriKind.Relative))
            };
            drawingContext.DrawRectangle(bckgroundImage, null, new Rect(new Point(this.ActualWidth / 6, 15), new Size(68, 80)));

            drawingContext.DrawText(
                time, new Point((this.ActualWidth / 6) + 83, 45));

            FormattedText score = new FormattedText(
                    string.Format($"SCORE {(App.Current.Resources["Locator"] as ViewModelLocator).Ingame.Score}"),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Chakra Petch"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                    25,
                    Brushes.White);
            drawingContext.DrawText(
                score, new Point(this.ActualWidth / 1.7, 45));

            FormattedText lifeLimit = new FormattedText(
                    string.Format($"POWER LIMIT {(App.Current.Resources["Locator"] as ViewModelLocator).Ingame.PowerLimit}"),
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Chakra Petch"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                    25,
                    Brushes.White);
            drawingContext.DrawText(
                lifeLimit, new Point(this.ActualWidth / 3, 45));
        }

        private void IsoDrawCabel(INetworkController networkControlObject, DrawingContext drawingContext)
        {
            foreach (var item in networkControlObject.Utps)
            {
                foreach (var cabels in item.Cables)
                {
                    // SolidColorBrush s = Brushes.Red;
                    // if (item.Creator.Owner is HumanPlayer)
                    // {
                    //    s = Brushes.Green;
                    // }

                    // draw line
                    // drawingContext.DrawLine(new Pen(Brushes.Black, 2), vh.IsoPosition(new Point((int)item.Parent.X, (int)item.Parent.Y)), vh.IsoPosition(new Point((int)item.Target.X, (int)item.Target.Y)));

                    // First parameter should be defined by the player own color
                    // drawingContext.DrawEllipse(s, null, vh.IsoPosition(new Point(cabels.X, cabels.Y)), cabels.Width / 2, cabels.Height / 2);
                    if (cabels.Previous is ICable)
                    {
                        drawingContext.DrawLine(new Pen(cabels.ChargeOwner != null ? Brushes.Yellow : this.sdi[item.Creator.Owner.Type].SolidColor, 5), this.vh.IsoPosition(new Point(cabels.X, cabels.Y)), this.vh.IsoPosition(new Point((cabels.Previous as ICable).X, (cabels.Previous as ICable).Y)));
                    }
                    else
                    {
                        drawingContext.DrawLine(new Pen(cabels.ChargeOwner != null ? Brushes.Yellow : this.sdi[item.Creator.Owner.Type].SolidColor, 5), this.vh.IsoPosition(new Point(cabels.X, cabels.Y)), this.vh.IsoPosition(new Point(item.Parent.X, item.Parent.Y)));
                    }
                }
            }
        }

        private void IsoDrawNetworkControl(INetworkController networkControlObject, DrawingContext drawingContext)
        {
            // double arány = ServerDrawInfo.HeightsPropotion[(networkControlObject as IServerNetworkController).Level];
            IServerNetworkController sNCTemp = networkControlObject as IServerNetworkController;
            Size size = new Size(this.vh.IsoLenght(), (double)this.vh.IsoLenght() * ServerDrawInfo.HeightsPropotion[sNCTemp.Level]);

            // ServerDrawInfo.PicSizes[(networkControlObject as IServerNetworkController).Level];
            Point p = this.vh.DrawPosition(new Point(networkControlObject.X, networkControlObject.Y));
            p = new Point(p.X - size.Width + 1, p.Y - size.Height + 1);
            Rect position = new Rect(p, size);
            if (networkControlObject.IsEnable)
            {
                ServerDrawInfo sDITemp = this.sdi[networkControlObject.Owner.Type] as ServerDrawInfo;
                ImageBrush serverImage = sDITemp.ServerImages[sNCTemp.Level];
                drawingContext.DrawRectangle(serverImage, null, position);
                this.LifeTextDrawTransform(networkControlObject.Life.ToString(), drawingContext, Brushes.Black, p);
            }
            else
            {
                ImageBrush serverImage = (this.sdi[PlayerType.Nobody] as ServerDrawInfo).ServerImages[sNCTemp.Level];
                drawingContext.DrawRectangle(serverImage, null, position);
                IInactiveNetworkController iNCTemp = networkControlObject as IInactiveNetworkController;
                this.LifeTextDrawTransform(
                    $"{iNCTemp.CapturePoint}/{iNCTemp.CaptureLimit}\n",
                    drawingContext,
                    networkControlObject.Owner != null ? (this.sdi[networkControlObject.Owner.Type] as ServerDrawInfo).SolidColor : Brushes.Black,
                    p);
            }

            DrawedNetworkControllers.Add(networkControlObject, position);

            // Original Drawtext
            // drawingContext.DrawText(
            //    formattedText,
            //    VectorHandler.VH.IsoPosition(new Point(networkControlObject.X, networkControlObject.Y - 50)));
        }

        private void LifeTextDrawTransform(string s, DrawingContext drawingContext, Brush brush, Point p)
        {
            bool debug = false;

            FormattedText formattedText = new FormattedText(
                    s,
                    CultureInfo.GetCultureInfo("en-us"),
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Chakra Petch"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal),
                    18,
                    brush);

            formattedText.SetFontWeight(FontWeights.Bold);

            TextAlignment text_align = TextAlignment.Center;

            // Make a transformation to center the text.
            double width = formattedText.Width -
                formattedText.OverhangLeading;
            double height = formattedText.Height;
            TranslateTransform translate1 = new TranslateTransform
            {
                Y = -height / 2
            };
            if ((text_align == TextAlignment.Left) ||
                (text_align == TextAlignment.Justify))
            {
                translate1.X = -width / 2;
            }
            else if (text_align == TextAlignment.Right)
            {
                translate1.X = width / 2;
            }
            else
            {
                translate1.X = 0;
            }

            RotateTransform rotate = new RotateTransform(-35);

            // Get the text's bounding rectangle.
            Rect rect = new Rect(0, 0, width, height);
            if (text_align == TextAlignment.Center)
            {
                rect.X -= width / 2;
            }
            else if (text_align == TextAlignment.Right)
            {
                rect.X -= width;
            }

            // Get the rotated bounding rectangle.
            Rect rotated_rect = rotate.TransformBounds(rect);

            Point origin = new Point(p.X + 30, p.Y + 10);

            // Make a transformation to center the
            // bounding rectangle at the destination.
            TranslateTransform translate2 = new TranslateTransform(origin.X, origin.Y);

            TextAlignment halign = TextAlignment.Center;
            VerticalAlignment valign = VerticalAlignment.Top;

            // Adjust the translation for the desired alignment.
            if (halign == TextAlignment.Left)
            {
                translate2.X += rotated_rect.Width / 2;
            }
            else if (halign == TextAlignment.Right)
            {
                translate2.X -= rotated_rect.Width / 2;
            }

            if (valign == VerticalAlignment.Top)
            {
                translate2.Y += rotated_rect.Height / 2;
            }
            else if (valign == VerticalAlignment.Bottom)
            {
                translate2.Y -= rotated_rect.Height / 2;
            }

            // Push transformations in reverse order. (Thanks Microsoft!)
            drawingContext.PushTransform(translate2);
            drawingContext.PushTransform(rotate);
            drawingContext.PushTransform(translate1);

            // Draw.
            drawingContext.DrawText(formattedText, new Point(0, 0));

            // Draw a rectangle around the text. (For debugging.)
            if (debug == true)
            {
                drawingContext.DrawRectangle(null, new Pen(Brushes.Red, 1), rect);
            }

            // Remove the transformations.
            drawingContext.Pop();
            drawingContext.Pop();
            drawingContext.Pop();

            // Draw the rotated bounding rectangle. (For debugging.)
            if (debug == true)
            {
                Rect transformed_rect = translate2.TransformBounds(rotate.TransformBounds(translate1.TransformBounds(rect)));
                Pen custom_pen = new Pen(Brushes.Blue, 1)
                {
                    DashStyle = new DashStyle(new double[] { 5, 5 }, 0)
                };
                drawingContext.DrawRectangle(null, custom_pen, transformed_rect);
            }
        }

        private void IsoDrawFirewall(IFireWall firewall, DrawingContext drawingContext)
        {
            Size size = new Size(this.vh.IsoLenght(), (double)this.vh.IsoLenght() * 110 / 113);
            Point p = this.vh.DrawPosition(new Point(firewall.X, firewall.Y));
            p = new Point(p.X - size.Width + 1, p.Y - size.Height + 1);
            Rect position = new Rect(p, size);
            ImageBrush fireWallImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(@"Resources\Firewall\FirewallIcon.png", UriKind.Relative))
            };

            drawingContext.DrawRectangle(fireWallImage, null, position);
        }

        private void IsoDrawRouter(INetworkController router, DrawingContext drawingContext)
        {
            Size size = new Size(this.vh.IsoLenght(), (double)this.vh.IsoLenght() * ServerDrawInfo.RouterHeightsPropotion);
            Point p = this.vh.DrawPosition(new Point(router.X, router.Y));
            p = new Point(p.X - size.Width + 1, p.Y - size.Height + 1);

            Rect position = new Rect(p, size);

            if (router.IsEnable)
            {
                ServerDrawInfo sDITemp = this.sdi[router.Owner.Type] as ServerDrawInfo;
                ImageBrush routerImage = sDITemp.Router;
                drawingContext.DrawRectangle(routerImage, null, position);
                this.LifeTextDrawTransform(router.Life.ToString(), drawingContext, Brushes.Black, p);
            }
            else
            {
                ImageBrush routerImage = (this.sdi[PlayerType.Nobody] as ServerDrawInfo).Router;
                drawingContext.DrawRectangle(routerImage, null, position);
                IInactiveNetworkController iNCTemp = router as IInactiveNetworkController;
                this.LifeTextDrawTransform($"{iNCTemp.CapturePoint}/{iNCTemp.CaptureLimit}", drawingContext, router.Owner != null ? (this.sdi[router.Owner.Type] as ServerDrawInfo).SolidColor : Brushes.Black, p);
            }

            DrawedNetworkControllers.Add(router, position);
        }
    }
}
