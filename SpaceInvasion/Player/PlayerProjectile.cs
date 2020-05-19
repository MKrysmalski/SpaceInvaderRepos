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
    class PlayerProjectile : GameObject
    {
        private Canvas GameDisplay { get; set; }
        public Rectangle Reci { get; set; }
        public PlayerProjectile(Canvas GameDisplay, Player player,int v) : base(player.X+14, player.Y,v)
        {
            this.GameDisplay = GameDisplay;
            this.Reci = new Rectangle();
            Reci.Width = 5;
            Reci.Height = 15;
            Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Projectiles/laserBlue.png", UriKind.Relative)));
        }
        public override void Draw()
        {
            GameDisplay.Children.Add(Reci);
            Canvas.SetLeft(Reci, X);
            Canvas.SetTop(Reci, Y);
        }
        public void Shoot()
        {
            Canvas.SetTop(Reci, Y -= V);
        }
    }
}
