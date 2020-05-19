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
    class EnemyProjectile : GameObject
    {
       public Rectangle Reci { get; set; }
        Canvas GameDisplay { get; set; }
        

        public EnemyProjectile(Canvas gameDisplay,Invader invader,int v) : base(invader.X,invader.Y+5,v)
        {
            this.Reci = new Rectangle();
            this.GameDisplay = gameDisplay;
            Reci.Width = 5;
            Reci.Height = 15;
            Reci.Fill= new ImageBrush(new BitmapImage(new Uri("Images/Projectiles/laserGreen.png", UriKind.Relative)));
        }
        public EnemyProjectile(Canvas gameDisplay, Ufo invader, int v) : base(invader.X+10, invader.Y + 5, v)
        {
            this.Reci = new Rectangle();
            this.GameDisplay = gameDisplay;
            Reci.Width = 5;
            Reci.Height = 15;
            Reci.Fill = new ImageBrush(new BitmapImage(new Uri("Images/Projectiles/laserRed.png", UriKind.Relative)));
        }
        public override void Draw()
        {
            GameDisplay.Children.Add(Reci);
            Canvas.SetLeft(Reci, X);
            Canvas.SetTop(Reci, Y);
        }
        public void Shoot()
        {
            Canvas.SetTop(Reci, Y += V);
        }
    }
}
 