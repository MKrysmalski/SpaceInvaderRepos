using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SpaceInvasion
{
    class Player : GameObject
    {
        public Rectangle Reci = new Rectangle();
        public int HealthPoints { get; set; }
        public Canvas GameDisplay { get; set; }
        public Player(Canvas GameDisplay, double v) : base(GameDisplay.ActualWidth/2,635, 0)
        {
            this.HealthPoints = 3;
            Reci.Width = 30;
            this.V = v;
            Reci.Height = 30;
            Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Player/Ship_blue.PNG",UriKind.Relative)));
            this.GameDisplay = GameDisplay;
        }
        public override void Draw()
        {
           GameDisplay.Children.Add(Reci);
           Canvas.SetLeft(Reci, X);
           Canvas.SetTop(Reci, Y);
        }
        public void Movement()
        {
            if (X >= GameDisplay.ActualWidth - GameDisplay.ActualWidth)
                if (Keyboard.IsKeyDown(Key.A))
                {
                   Canvas.SetLeft(Reci, X += -V);
                }
            if (X <= GameDisplay.ActualWidth - Reci.Width)
                if (Keyboard.IsKeyDown(Key.D))
                {
                    Canvas.SetLeft(Reci, X += V);
                }
        }
        public bool IsHit(double x, double y)
        {
            return (Reci.RenderedGeometry.FillContains(new Point(x - X, y - Y)));
        }
    }
}
