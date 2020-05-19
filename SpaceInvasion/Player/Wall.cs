using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvasion
{
    class Wall : GameObject
    {
        public Rectangle Reci { get; set; }
        private Canvas GameDisplay { get; set; }
        public Wall(Canvas GameDisplay) : base(GameDisplay.ActualWidth, GameDisplay.ActualHeight)
        {
            this.GameDisplay = GameDisplay;
            this.Reci = new Rectangle();
            Reci.Width = 20;
            Reci.Height = 20;
            Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Walls/WallYellow.png", UriKind.Relative)));
        }
        public override void Draw()
        {
            GameDisplay.Children.Add(Reci);
            Canvas.SetLeft(Reci, X);
            Canvas.SetTop(Reci, Y);
        }

        public bool IsHit(double x, double y)
        {
            return (Reci.RenderedGeometry.FillContains(new Point((x+2.5) - X, y - Y)));
        }
    }
}
