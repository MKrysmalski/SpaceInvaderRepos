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
    class Ufo : GameObject
    {
        public Canvas GameDisplay { get; set; }
        public Ellipse Elli { get; set; }
        public Ufo(Canvas GameDisplay,int v) : base(-60,5,v)
        {
            this.GameDisplay = GameDisplay;
            this.Elli = new Ellipse();
            Elli.Width = 60;
            Elli.Height = 30;
            Elli.Fill= new ImageBrush(new BitmapImage(new Uri("Images/Enemies/ufoRed.PNG", UriKind.Relative)));

        }
        public override void Draw()
        {
            GameDisplay.Children.Add(Elli);
            Canvas.SetLeft(Elli, X);
            Canvas.SetTop(Elli, Y);
        }
        public void Movement()
        {
            if (X >= GameDisplay.ActualWidth)
                X = -60;
             
            X += V;
        }
        public bool IsHit(double x, double y)
        {
            return (Elli.RenderedGeometry.FillContains(new Point((x + 2.5) - X, y - Y)));
        }
    }
}
