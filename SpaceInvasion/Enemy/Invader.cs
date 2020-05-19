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
    class Invader:GameObject
    {

        public Rectangle reci=new Rectangle();
        public Canvas Gamedisplay { get; set; }
        public Random Rnd { get; set; }
        public Invader(Canvas GameDisplay,double v) : base(GameDisplay.ActualWidth / 2, GameDisplay.ActualHeight / 2)
        {
            reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Enemies/enemyRed1.PNG", UriKind.Relative)));
            reci.Width = 30;
            reci.Height = 30;
            this.Rnd = new Random();
            this.V = v;
            this.Gamedisplay = GameDisplay;
        }
        public void Movement(int index)
        {
           
            switch (index)
            {
                case 1 : V = -V; Canvas.SetTop(reci, Y +=10); break;
                case 2 : V = -V; Canvas.SetTop(reci, Y += 10); break;
                default : break;
            }
            Canvas.SetLeft(reci, X += V);
            Canvas.SetTop(reci, Y);
            
            
        }
        public override void Draw()
        {
            Gamedisplay.Children.Add(reci);
            Canvas.SetLeft(reci, X);
            Canvas.SetTop(reci, Y);
        }
        public bool IsHit(double x, double y)
        {
            return (reci.RenderedGeometry.FillContains(new Point((x+2.5) - X, y - Y)));
        }
    }
}
